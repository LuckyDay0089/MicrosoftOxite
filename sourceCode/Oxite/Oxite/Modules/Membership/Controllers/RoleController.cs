﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Web.Mvc;
using Oxite.Extensions;
using Oxite.Infrastructure;
using Oxite.Models;
using Oxite.Modules.Membership.Models;
using Oxite.Modules.Membership.Services;
using Oxite.Services;
using Oxite.ViewModels;

namespace Oxite.Modules.Membership.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public OxiteViewModelItems<Role> Find()
        {
            return new OxiteViewModelItems<Role>();
        }

        [ActionName("Find"), AcceptVerbs(HttpVerbs.Post)]
        public OxiteViewModelItems<Role> FindQuery(RoleSearchCriteria searchCriteria)
        {
            IEnumerable<Role> foundRoles = roleService.FindRoles(searchCriteria);
            //TODO: (erikpo) Before the list is set into the model, filter it from the AuthorizationManager class (like make sure anonymous doesn't ever get sent down, etc)
            OxiteViewModelItems<Role> model = new OxiteViewModelItems<Role>(foundRoles);

            model.AddModelItem(searchCriteria);

            return model;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public OxiteViewModelItem<Role> ItemEdit(Role role)
        {
            if (role == null) return null;

            //TODO: (erikpo) Check permissions

            return new OxiteViewModelItem<Role>(role);
        }

        [ActionName("ItemEdit"), AcceptVerbs(HttpVerbs.Post)]
        public object ItemSave(Role role, RoleInput roleInput)
        {
            //TODO: (erikpo) Check permissions

            ModelResult<Role> results;

            if (role != null)
                results = roleService.EditRole(role, roleInput);
            else
                results = roleService.AddRole(roleInput);

            if (!results.IsValid)
            {
                ModelState.AddModelErrors(results.ValidationState);

                return ItemEdit(role);
            }

            return Redirect(Url.AppPath(Url.ManageUsers()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public object ItemRemove(Role role)
        {
            //TODO: (erikpo) Check permissions

            roleService.RemoveRole(role);

            return Redirect(Url.AppPath(Url.ManageUsers()));
        }
    }
}
