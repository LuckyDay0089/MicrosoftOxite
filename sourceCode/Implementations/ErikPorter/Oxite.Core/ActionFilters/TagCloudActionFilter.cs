﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Services;
using Oxite.ViewModels;

namespace Oxite.ActionFilters
{
    public class TagCloudActionFilter : IActionFilter
    {
        private readonly ITagService tagService;

        public TagCloudActionFilter(ITagService areaService)
        {
            this.tagService = areaService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            OxiteModel model = filterContext.Controller.ViewData.Model as OxiteModel;

            if (model == null) return;

            model.AddModelItem(new TagCloudViewModel(tagService.GetTagsWithPostCount()));
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
