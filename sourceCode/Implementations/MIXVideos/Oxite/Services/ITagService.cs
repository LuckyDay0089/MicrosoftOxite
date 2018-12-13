﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using Oxite.Models;

namespace Oxite.Services
{
    public interface ITagService
    {
        IList<Tag> GetTags();
        IList<KeyValuePair<Tag, int>> GetTagsWithPostCount();
        Tag GetTag(string name);
        IList<KeyValuePair<Tag, int>> GetTagsUsedIn(Area area);
    }
}
