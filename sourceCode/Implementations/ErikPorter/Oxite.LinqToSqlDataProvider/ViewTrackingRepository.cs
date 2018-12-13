﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using Oxite.Repositories;

namespace Oxite.LinqToSqlDataProvider
{
    public class ViewTrackingRepository : IViewTrackingRepository
    {
        private OxiteLinqToSqlDataContext context;

        public ViewTrackingRepository(OxiteLinqToSqlDataContext context)
        {
            this.context = context;
        }

        #region IViewTrackingRepository Members

        //public string[] GetViewTypes()
        //{
        //    return (
        //        from pvt in context.oxite_PostViewTypes
        //        orderby pvt.PostViewTypeName ascending
        //        select pvt.PostViewTypeName
        //        ).ToArray();
        //}

        //public void Save(Guid postID, string viewType, int count)
        //{
        //    Guid viewTypeID = (from pvt in context.oxite_PostViewTypes where string.Compare(pvt.PostViewTypeName, viewType, true) == 0 select pvt.PostViewTypeID).First();

        //    context.oxite_PostViews.InsertOnSubmit(
        //        new oxite_PostView()
        //        {
        //            PostID = postID,
        //            PostViewID = Guid.NewGuid(),
        //            PostViewTypeID = viewTypeID,
        //            PostViewCount = count,
        //            PostViewDate = DateTime.UtcNow
        //        }
        //        );

        //    context.SubmitChanges();
        //}

        #endregion
    }
}
