﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Oxite.ActionFilters;
using Oxite.Models;
using Oxite.Results;
using Oxite.Tests.Fakes;
using Oxite.ViewModels;
using Xunit;

namespace Oxite.Tests.Filters
{
    public class RssResultFilterTests
    {
        [Fact]
        public void OnExecutedReplacesResultWithRss()
        {
            RssResultActionFilter filter = new RssResultActionFilter();

            ActionExecutedContext context = new ActionExecutedContext()
                {
                    Result = new ViewResult(),
                    Controller = new FakeController() { ViewData = new ViewDataDictionary(new OxiteModelList<Post>() { List = new List<Post>(new[] { new Post() }) }) }
                };

            filter.OnActionExecuted(context);

            FeedResult result = context.Result as FeedResult;

            Assert.NotNull(result);
            Assert.True(string.Equals("Rss", result.ViewName, StringComparison.OrdinalIgnoreCase));
            Assert.False(result.IsClientCached);
        }

        [Fact]
        public void OnExecutedSetsClientCachedToTrueForEmptyList()
        {
            RssResultActionFilter filter = new RssResultActionFilter();

            ActionExecutedContext context = new ActionExecutedContext()
            {
                Result = new ViewResult(),
                Controller = new FakeController() { ViewData = new ViewDataDictionary(new OxiteModelList<Post>() { List = Enumerable.Empty<Post>().ToList() }) }
            };

            filter.OnActionExecuted(context);

            FeedResult result = context.Result as FeedResult;

            Assert.NotNull(result);
            Assert.True(result.IsClientCached);
        }
    }
}
