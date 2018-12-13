﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Models;
using Oxite.Modules.Tags.Models;

namespace Oxite.Modules.Tags.Repositories.SqlServer
{
    public class SqlServerTagRepository : ITagRepository
    {
        private readonly OxiteTagsDataContext context;
        private readonly Guid siteID;

        public SqlServerTagRepository(OxiteTagsDataContext context, Site site)
        {
            this.context = context;
            this.siteID = site.ID;
        }

        #region ITagRepository Members

        public IQueryable<Tag> GetTags()
        {
            return
                from t in context.oxite_Tags
                join pt in context.oxite_Tags on t.ParentTagID equals pt.TagID
                select new Tag(t.TagID, t.TagName, t.CreatedDate);
        }

        public Tag GetTag(Guid id)
        {
            return (
                from t in context.oxite_Tags
                where t.TagID == id
                select new Tag(t.TagID, t.TagName, t.CreatedDate)
                ).FirstOrDefault();
        }

        public Tag GetTag(string tagName)
        {
            return (
                from t in context.oxite_Tags
                where t.TagName == tagName
                select new Tag(t.TagID, t.TagName, t.CreatedDate)
                ).FirstOrDefault();
        }

        public IEnumerable<Tag> GetTags(IEnumerable<Guid> ids)
        {
            return
                from t in context.oxite_Tags
                where ids.Contains(t.TagID)
                select new Tag(t.TagID, t.TagName, t.CreatedDate);
        }

        #endregion
    }
}
