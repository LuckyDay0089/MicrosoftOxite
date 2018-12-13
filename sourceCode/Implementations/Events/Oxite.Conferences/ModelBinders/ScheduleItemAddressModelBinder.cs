﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Modules.Conferences.Models;

namespace Oxite.Modules.Conferences.ModelBinders
{
    public class ScheduleItemAddressModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string eventName = controllerContext.RouteData.Values["eventName"] as string;
            string scheduleItemSlug = controllerContext.RouteData.Values["scheduleItemSlug"] as string;

            if (!string.IsNullOrEmpty(eventName) || !string.IsNullOrEmpty(scheduleItemSlug))
                return new ScheduleItemAddress(eventName, scheduleItemSlug);

            return null;
        }
    }
}