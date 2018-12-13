﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.Models;
using Oxite.Models.Extensions;

namespace Oxite.Infrastructure
{
    public class AbsolutePathHelper
    {
        private readonly Site site;
        private readonly RouteCollection routes;

        public AbsolutePathHelper(Site site, RouteCollection routes)
        {
            this.site = site;
            this.routes = routes;
        }

        public string GetAbsolutePath(Post post)
        {
            if (post == null) throw new ArgumentNullException("post");
            if (string.IsNullOrEmpty(post.Slug) || post.Area == null || string.IsNullOrEmpty(post.Area.Name)) throw new ArgumentException();

            return GetAbsolutePath(new PostAddress(post.Area.Name, post.Slug));
        }

        public string GetAbsolutePath(PostAddress postAddress)
        {
            UriBuilder builder = new UriBuilder(site.Host.Scheme, site.Host.Host, site.Host.Port, site.Host.AbsolutePath);
            UrlHelper urlHelper = new UrlHelper(new RequestContext(new DummyHttpContext(null, site.Host), new RouteData()), routes);

            //TODO: (erikpo) It might make sense to move UrlHelperExtensions back into Oxite so this isn't duplicated
            builder.Path = urlHelper.RouteUrl("Post", new { areaName = postAddress.AreaName != null ? postAddress.AreaName : "", slug = postAddress.Slug, dataFormat = "" });

            return builder.Uri.ToString();
        }

        public string GetAbsolutePath(Post post, Comment comment)
        {
            if (post == null) throw new ArgumentNullException("post");
            if (comment == null) throw new ArgumentNullException("comment");
            if (string.IsNullOrEmpty(post.Slug) || post.Area == null || string.IsNullOrEmpty(post.Area.Name)) throw new ArgumentException();

            UriBuilder builder = new UriBuilder(site.Host.Scheme, site.Host.Host, site.Host.Port, site.Host.AbsolutePath);
            UrlHelper urlHelper = new UrlHelper(new RequestContext(new DummyHttpContext(null, site.Host), new RouteData()), routes);

            //TODO: (erikpo) It might make sense to move UrlHelperExtensions back into Oxite so this isn't duplicated
            builder.Path = urlHelper.RouteUrl("PostCommentPermalink", new { areaName = post.Area.Name, slug = post.Slug, comment = comment.GetSlug() });

            return builder.Uri.ToString();
        }

        public PostAddress GetPostAddressFromUri(Uri permalink)
        {
            if (permalink == null)
                throw new ArgumentNullException();

            if (!permalink.ToString().StartsWith(site.Host.ToString(), StringComparison.OrdinalIgnoreCase))
                return null;

            RouteData data = routes["Post"].GetRouteData(new DummyHttpContext(permalink, site.Host));

            if (data != null)
                return new PostAddress(data.GetRequiredString("areaName"), data.GetRequiredString("slug"));

            return null;
        }

        private class DummyHttpContext : HttpContextBase 
        {
            private readonly Uri requestUrl;
            private readonly Uri hostUrl;
            public DummyHttpContext(Uri requestUrl, Uri hostUrl)
            {
                this.requestUrl = requestUrl;
                this.hostUrl = hostUrl;
            }

            public override HttpRequestBase Request
            {
                get
                {
                    return new DummyHttpRequest(requestUrl, hostUrl);
                }
            }

            public override HttpResponseBase Response
            {
                get
                {
                    return new DummyHttpResponse();
                }
            }
        }

        private class DummyHttpRequest : HttpRequestBase
        {
            private readonly Uri requestUrl;
            private readonly Uri hostUrl;
            public DummyHttpRequest(Uri requestUrl, Uri hostUrl)
            {
                this.requestUrl = requestUrl;
                this.hostUrl = hostUrl;
            }

            public override Uri Url
            {
                get
                {
                    return requestUrl;
                }
            }

            public override string ApplicationPath
            {
                get
                {
                    return hostUrl.AbsolutePath;
                }
            }

            public override string AppRelativeCurrentExecutionFilePath
            {
                get
                {
                    if (hostUrl.AbsolutePath.Length > 1)
                        return "~" + requestUrl.AbsolutePath.Remove(0, hostUrl.AbsolutePath.Length);

                    return "~" + requestUrl.AbsolutePath;
                }
            }

            public override string PathInfo
            {
                get
                {
                    return "";
                }
            }

            public override System.Collections.Specialized.NameValueCollection ServerVariables
            {
                get
                {
                    return new System.Collections.Specialized.NameValueCollection();
                }
            }
        }

        private class DummyHttpResponse : HttpResponseBase
        {
            public override string ApplyAppPathModifier(string virtualPath)
            {
                return virtualPath;
            }
        }
    }
}
