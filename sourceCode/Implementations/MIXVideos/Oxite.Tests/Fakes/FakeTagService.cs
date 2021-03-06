﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Models;
using Oxite.Services;

namespace Oxite.Tests.Fakes
{
    public class FakeTagService : ITagService
    {
        public Dictionary<string, Tag> StoredTags { get; set; }

        public FakeTagService()
        {
            this.StoredTags = new Dictionary<string, Tag>();
        }

        #region ITagService Members

        public IList<Tag> GetTags()
        {
            return this.StoredTags.Select(t => t.Value).ToList();
        }

        public IList<KeyValuePair<Tag, int>> GetTagsWithPostCount()
        {
            throw new NotImplementedException();
        }

        public Tag GetTag(string name)
        {
            if (this.StoredTags.ContainsKey(name))
                return this.StoredTags[name];
            else
                return null;
        }

        #endregion

        #region ITagService Members


        public IList<KeyValuePair<Tag,int>> GetTagsUsedIn(Area area)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
