using FluentAssertions;
using LogisticsSystem.Controllers;
using LogisticsSystem.Services.Loads.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using static LogisticsSystem.WebConstants.Cache;
using static LogisticsSystem.Test.Data.Loads;
using System;

namespace LogisticsSystem.Test.Controllers
{

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectModelAndView()
        => MyController<HomeController>
                  .Instance(controller => controller
                  .WithData(GetLoads()))
            .Calling(c => c.Index())
            .ShouldHave()
            .MemoryCache(cache => cache
            .ContainingEntry(entry => entry
                  .WithKey(LatestLoadsCacheKey)
                  .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
                  .WithValueOfType<List<LoadServiceModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view.WithModelOfType<List<LoadServiceModel>>()
            .Passing(model => model.Should().HaveCount(5)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
            .Instance()
            .Calling(x => x.Error())
            .ShouldReturn()
            .View();

    }
}
