﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Oxite.Extensions;
using Oxite.Infrastructure;
using Oxite.Models;
using Oxite.Modules.Blogs.Extensions;
using Oxite.Modules.Blogs.Models;
using Oxite.Modules.Blogs.Repositories;
using Oxite.Modules.Comments.Models;
using Oxite.Modules.Comments.Repositories;
using Oxite.Modules.Messages.Models;
using Oxite.Modules.Messages.Repositories;
using Oxite.Modules.Tags.Models;
using Oxite.Plugins.Extensions;
using Oxite.Plugins.Models;
using Oxite.Repositories;
using Oxite.Validation;

namespace Oxite.Modules.Blogs.Services
{
    public class BlogsCommentService : IBlogsCommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IBlogsCommentRepository blogsCommentRepository;
        private readonly IPostRepository postRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly ILocalizationRepository localizationRepository;
        private readonly IMessageOutboundRepository messageOutboundRepository;
        private readonly UrlHelper urlHelper;
        private readonly IValidationService validator;
        private readonly IPluginEngine pluginEngine;
        private readonly IOxiteCacheModule cache;
        private readonly OxiteContext context;

        public BlogsCommentService(ICommentRepository commentRepository, IBlogsCommentRepository blogsCommentRepository, IPostRepository postRepository, ILanguageRepository languageRepository, ILocalizationRepository localizationRepository, IMessageOutboundRepository messageOutboundRepository, UrlHelper urlHelper, IValidationService validator, IPluginEngine pluginEngine, IModulesLoaded modules, OxiteContext context)
        {
            this.commentRepository = commentRepository;
            this.blogsCommentRepository = blogsCommentRepository;
            this.postRepository = postRepository;
            this.languageRepository = languageRepository;
            this.localizationRepository = localizationRepository;
            this.messageOutboundRepository = messageOutboundRepository;
            this.urlHelper = urlHelper;
            this.validator = validator;
            this.pluginEngine = pluginEngine;
            this.cache = modules.GetModules<IOxiteCacheModule>().Reverse().First();
            this.context = context;
        }

        #region IBlogsCommentService Members

        public IPageOfItems<PostComment> GetComments(PagingInfo pagingInfo, bool includePending, bool sortDescending)
        {
            return cache.GetItems<IPageOfItems<PostComment>, PostComment>(
                string.Format("GetComments-IncludePending:{0},SortDescending{1}", includePending, sortDescending),
                pagingInfo.ToCachePartition(),
                () => processDisplayOfComments(() => getComments(blogsCommentRepository.GetComments(pagingInfo, includePending, sortDescending))),
                c => getCommentDependencies(c, new SiteSmall(context.Site.ID))
                );
        }

        public IPageOfItems<PostComment> GetComments(PagingInfo pagingInfo, Blog blog)
        {
            return cache.GetItems<IPageOfItems<PostComment>, PostComment>(
                string.Format("GetComments-{0}", blog.GetCacheItemKey()),
                pagingInfo.ToCachePartition(),
                () => processDisplayOfComments(() => getComments(blogsCommentRepository.GetComments(pagingInfo, blog))),
                c => getCommentDependencies(c, blog)
                );
        }

        public IPageOfItems<PostComment> GetComments(PagingInfo pagingInfo, Post post, bool includeUnapproved)
        {
            return cache.GetItems<IPageOfItems<PostComment>, PostComment>(
                string.Format("GetComments-{0},IncludeUnapproved:{1}", post, includeUnapproved),
                pagingInfo.ToCachePartition(),
                () => processDisplayOfComments(() => getComments(blogsCommentRepository.GetComments(pagingInfo, post, includeUnapproved))),
                c => getCommentDependencies(c, post)
                );
        }

        public IPageOfItems<PostComment> GetComments(PagingInfo pagingInfo, Tag tag)
        {
            return cache.GetItems<IPageOfItems<PostComment>, PostComment>(
                string.Format("GetComments-{0}", tag.GetCacheItemKey()),
                pagingInfo.ToCachePartition(),
                () => processDisplayOfComments(() => getComments(blogsCommentRepository.GetComments(pagingInfo, tag))),
                c => getCommentDependencies(c, tag)
                );
        }

        public ValidationStateDictionary ValidateCommentInput(CommentInput commentInput)
        {
            ValidationStateDictionary validationState = new ValidationStateDictionary();

            validationState.Add(typeof(CommentInput), validator.Validate(commentInput));

            return validationState;
        }

        public ModelResult<PostComment> AddComment(Post post, CommentInput commentInput)
        {
            CommentIn commentIn = new CommentIn(commentInput);

            pluginEngine.ExecuteAll("ProcessInputOfComment", new { context, comment = commentIn });
            commentInput = commentIn.ToCommentInput();

            commentInput = pluginEngine.Process("ProcessInputOfCommentOnAdd", new CommentIn(commentInput)).ToCommentInput();

            if (pluginEngine.AnyTrue("IsCommentSpam", new { context, comment = commentIn }))
                return new ModelResult<PostComment>(new ValidationStateDictionary(typeof(CommentInput), new ValidationState(new[] { new ValidationError("Comment.IsSpam", commentInput, "The supplied comment was considered to be spam and was not added") })));

            ValidationStateDictionary validationState = ValidateCommentInput(commentInput);

            if (!validationState.IsValid) return new ModelResult<PostComment>(validationState);

            EntityState commentState;

            try
            {
                commentState = context.User.IsAuthenticated ? EntityState.Normal : (EntityState)Enum.Parse(typeof(EntityState), context.Site.CommentStateDefault);
            }
            catch
            {
                commentState = EntityState.PendingApproval;
            }

            //TODO: (erikpo) Replace with some logic to set the language from the user's browser or from a dropdown list
            Language language = languageRepository.GetLanguage(context.Site.LanguageDefault ?? "en");
            PostComment comment;

            using (TransactionScope transaction = new TransactionScope())
            {
                string commentSlug = generateUniqueCommentSlug(post);

                comment = commentInput.ToComment(context.User.Cast<User>(), context.HttpContext.Request.GetUserIPAddress().ToLong(), context.HttpContext.Request.UserAgent, language, commentSlug, commentState);

                comment = blogsCommentRepository.Save(comment, post.Blog.Name, post.Slug);

                if (comment.State == EntityState.Normal)
                    invalidateCachedCommentDependencies(comment);

                transaction.Complete();
            }

            //TODO: (erikpo) The following calls to setup the subscription and send out emails for those subscribed needs to happen in the transaction (but can't currently because of issues with them being in different repositories

            //TODO: (erikpo) Move into a module
            if (commentInput.Subscribe)
            {
                if (context.User.IsAuthenticated)
                    postRepository.AddSubscription(post, comment.CreatorUserID);
                else
                    postRepository.AddSubscription(post, comment);
            }

            //TODO: (erikpo) Move into a module
            messageOutboundRepository.Save(generateMessages(post, comment));

            PostSmallReadOnly postProxy = new PostSmallReadOnly(comment.Post);
            CommentReadOnly commentProxy = new CommentReadOnly(comment, urlHelper.AbsolutePath(urlHelper.Comment(comment)));

            pluginEngine.ExecuteAll("CommentAdded", new { context, parent = postProxy, comment = commentProxy });

            if (comment.State == EntityState.Normal)
                pluginEngine.ExecuteAll("CommentApproved", new { context, parent = postProxy, comment = commentProxy });

            return new ModelResult<PostComment>(comment, validationState);
        }

        public ModelResult<PostComment> AddComment(Post post, CommentInputForImport commentInput)
        {
            ValidationStateDictionary validationState = new ValidationStateDictionary();

            validationState.Add(typeof(CommentInputForImport), validator.Validate(commentInput));

            if (!validationState.IsValid) return new ModelResult<PostComment>(validationState);

            PostComment comment;

            using (TransactionScope transaction = new TransactionScope())
            {
                string commentSlug = generateUniqueCommentSlug(post);

                comment = commentInput.ToComment(commentSlug);

                comment = blogsCommentRepository.Save(comment, post.Blog.Name, post.Slug);

                invalidateCachedCommentDependencies(comment);

                transaction.Complete();
            }

            PostSmallReadOnly postProxy = new PostSmallReadOnly(comment.Post);
            CommentReadOnly commentProxy = new CommentReadOnly(comment, urlHelper.AbsolutePath(urlHelper.Comment(comment)));

            pluginEngine.ExecuteAll("CommentAddedFromImport", new { context, parent = postProxy, comment = commentProxy });

            return new ModelResult<PostComment>(comment, validationState);
        }

        public ModelResult<PostComment> EditComment(PostComment comment, CommentInput commentInput)
        {
            commentInput = pluginEngine.Process("ProcessInputOfComment", new CommentIn(commentInput)).ToCommentInput();
            commentInput = pluginEngine.Process("ProcessInputOfCommentOnEdit", new CommentIn(commentInput)).ToCommentInput();

            if (pluginEngine.AnyTrue("IsCommentSpam", commentInput))
                return new ModelResult<PostComment>(new ValidationStateDictionary(typeof(CommentInput), new ValidationState(new[] { new ValidationError("Comment.IsSpam", commentInput, "The supplied comment was considered to be spam and was not added") })));

            ValidationStateDictionary validationState = ValidateCommentInput(commentInput);

            if (!validationState.IsValid) return new ModelResult<PostComment>(validationState);

            PostComment newComment;
            PostComment originalComment = comment;

            using (TransactionScope transaction = new TransactionScope())
            {
                newComment = originalComment.Apply(commentInput, context.User.Cast<User>());

                newComment = blogsCommentRepository.Save(newComment, newComment.Post.BlogName, newComment.Post.Slug);

                invalidateCachedCommentForEdit(newComment, originalComment);

                transaction.Complete();
            }

            PostSmallReadOnly postProxy = new PostSmallReadOnly(newComment.Post);
            CommentReadOnly newCommentProxy = new CommentReadOnly(newComment, urlHelper.AbsolutePath(urlHelper.Comment(newComment)));
            CommentReadOnly originalCommentProxy = new CommentReadOnly(originalComment, urlHelper.AbsolutePath(urlHelper.Comment(originalComment)));

            pluginEngine.ExecuteAll("CommentEdited", new { context, parent = postProxy, comment = newCommentProxy, commentOriginal = originalCommentProxy });

            return new ModelResult<PostComment>(newComment, validationState);
        }

        public bool RemoveComment(PostComment comment)
        {
            return changeState(comment, EntityState.Removed, "CommentRemoved");
        }

        public bool ApproveComment(PostComment comment)
        {
            return changeState(comment, EntityState.Normal, "CommentApproved");
        }

        #endregion

        #region Private Methods

        private void invalidateCachedCommentDependencies(PostComment comment)
        {
            if (comment.Parent != null)
                cache.InvalidateItem(new PostComment(comment.Parent.ID));

            cache.InvalidateItem(new Post(comment.Post.ID));
        }

        private void invalidateCachedCommentForEdit(PostComment newComment, PostComment originalComment)
        {
            if (originalComment.Parent != null && newComment.Parent == null)
                cache.InvalidateItem(new PostComment(originalComment.Parent.ID));
            else if (originalComment.Parent == null && newComment.Parent != null)
                cache.InvalidateItem(new PostComment(newComment.Parent.ID));

            if (originalComment.Post.ID != newComment.Post.ID)
            {
                cache.InvalidateItem(new Post(originalComment.Post.ID));
                cache.InvalidateItem(new Post(newComment.Post.ID));
            }

            cache.InvalidateItem(newComment);
        }

        private void invalidateCachedCommentForRemove(PostComment comment)
        {
            invalidateCachedCommentDependencies(comment);

            cache.InvalidateItem(comment);
        }

        private IEnumerable<ICacheEntity> getCommentDependencies(PostComment comment, ICacheEntity dependency)
        {
            List<ICacheEntity> dependencies = new List<ICacheEntity>();

            if (comment != null)
            {
                dependencies.Add(new Post(comment.Post.ID));

                dependencies.Add(comment);
            }
            else
                dependencies.Add(dependency);

            return dependencies;
        }

        private IPageOfItems<PostComment> processDisplayOfComments(Func<IPageOfItems<PostComment>> getComments)
        {
            IPageOfItems<PostComment> comments = getComments();
            List<PostComment> newComments = new List<PostComment>();

            foreach (PostComment comment in comments)
            {
                CommentOut commentProxy = new CommentOut(comment, urlHelper.AbsolutePath(urlHelper.Comment(comment)));

                pluginEngine.ExecuteAll("ProcessDisplayOfComment", new { context, post = new PostSmallReadOnly(comment.Post), comment = commentProxy });

                newComments.Add(commentProxy.ToPostComment(comment.Parent, comment.Post));
            }

            return new PageOfItems<PostComment>(newComments, comments.PageIndex, comments.PageSize, comments.TotalItemCount);
        }

        private IPageOfItems<PostComment> getComments(IPageOfItems<PostCommentShell> postCommentShellList)
        {
            List<PostComment> comments = new List<PostComment>(postCommentShellList.Count());

            foreach (PostCommentShell postCommentShell in postCommentShellList)
                comments.Add(getComment(postCommentShell));

            return new PageOfItems<PostComment>(comments, postCommentShellList.PageIndex, postCommentShellList.PageSize, postCommentShellList.TotalItemCount);
        }

        private PostComment getComment(PostCommentShell postCommentShell)
        {
            if (postCommentShell == null) return null;

            return new PostComment(postCommentShell.Post, commentRepository.GetComment(postCommentShell.CommentID), postCommentShell.CommentSlug);
        }

        private string generateUniqueCommentSlug(Post post)
        {
            string commentSlug = null;
            bool isUnique = false;

            while (!isUnique)
            {
                commentSlug = Guid.NewGuid().ToString("N").Substring(0, 5);

                PostComment foundComment = GetComment(post.Blog.Name, post.Slug, commentSlug);

                isUnique = foundComment == null;
            }

            return commentSlug;
        }

        public PostComment GetComment(string blogName, string postSlug, string commentSlug)
        {
            return getComment(blogsCommentRepository.GetComment(blogName, postSlug, commentSlug));
        }

        private bool changeState(PostComment comment, EntityState state, string pluginEventName)
        {
            bool commentStateChanged = false;

            using (TransactionScope transaction = new TransactionScope())
            {
                if (comment != null && comment.State != state)
                {
                    commentRepository.ChangeState(comment.ID, state);

                    commentStateChanged = commentRepository.GetComment(comment.ID).State == state;
                }

                if (commentStateChanged)
                    cache.InvalidateItem(comment);

                transaction.Complete();
            }

            if (commentStateChanged)
                pluginEngine.ExecuteAll(pluginEventName, new { context, parent = new PostSmallReadOnly(comment.Post), comment = new CommentReadOnly(comment, urlHelper.AbsolutePath(urlHelper.Comment(comment))) });

            return commentStateChanged;
        }

        private IEnumerable<MessageOutbound> generateMessages(Post post, PostComment comment)
        {
            IEnumerable<PostSubscription> subscriptions = postRepository.GetSubscriptions(post);
            List<MessageOutbound> messages = new List<MessageOutbound>();
            //TODO: (erikpo) Once the plugin model is done, get this from the plugin
            int retryCount = 4;

            foreach (PostSubscription subscription in subscriptions)
            {
                string userName = subscription.UserName;

                MessageOutbound message = new MessageOutbound
                {
                    ID = Guid.NewGuid(),
                    To = !string.IsNullOrEmpty(userName) ? string.Format("{0} <{1}>", userName, subscription.UserEmail) : subscription.UserEmail,
                    Subject = string.Format(getPhrase("Messages.Formats.ReplySubject", context.Site.LanguageDefault, "RE: {0}"), post.Title),
                    Body = generateMessageBody(post, comment, context.Site),
                    RemainingRetryCount = retryCount
                };

                messages.Add(message);
            }

            return messages;
        }

        private string generateMessageBody(Post post, PostComment comment, Site site)
        {
            string body = getPhrase("Messages.NewComment", site.LanguageDefault, getDefaultBody());
            //TODO: (erikpo) Change this to come from the user this message is going to if applicable
            double timeZoneOffset = site.TimeZoneOffset;

            body = body.Replace("{Site.Name}", site.DisplayName);
            body = body.Replace("{User.Name}", comment.CreatorName);
            body = body.Replace("{Post.Title}", post.Title);
            //TODO: (erikpo) Change the published date to be relative (e.g. 5 minutes ago)
            body = body.Replace("{Comment.Created}", comment.Created.AddHours(timeZoneOffset).ToLongTimeString());
            body = body.Replace("{Comment.Body}", comment.Body);
            body = body.Replace("{Comment.Permalink}", urlHelper.AbsolutePath(urlHelper.Comment(comment)));

            return body;
        }

        private static string getDefaultBody()
        {
            return
                "<h1>New Comment on {Site.Name}</h1>" +
                "<h2>{User.Name} commented on '{Post.Title}' at {Comment.Created}</h2>" +
                "<p>{Comment.Body}</p>" +
                "<a href=\"{Comment.Permalink}\">{Comment.Permalink}</a>";
        }

        private string getPhrase(string key, string language)
        {
            return getPhrase(key, language, null);
        }

        private string getPhrase(string key, string language, string defaultValue)
        {
            Phrase phrase = localizationRepository.GetPhrases().Where(p => p.Key == key && p.Language == language).FirstOrDefault();

            if (phrase != null)
                return phrase.Value;

            if (defaultValue == null)
                return key;

            return defaultValue;
        }

        #endregion
    }
}
