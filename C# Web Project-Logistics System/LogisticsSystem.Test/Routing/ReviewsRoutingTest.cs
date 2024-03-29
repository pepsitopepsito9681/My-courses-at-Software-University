﻿using LogisticsSystem.Controllers;
using LogisticsSystem.Models.Reviews;
using MyTested.AspNetCore.Mvc;
using Xunit;

using static LogisticsSystem.Test.Data.Loads;

namespace LogisticsSystem.Test.Routing
{
    

    public class ReviewsRoutingTest
    {
        [Fact]
        public void PostAddShouldBeMapped()
            => MyRouting
                .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Reviews/Add/{LoadTestId}")
                     .WithMethod(HttpMethod.Post))
                 .To<ReviewsController>(c => c
                 .Add(LoadTestId, With.Any<ReviewFormModel>()));

        [Fact]
        public void PostEditShoulBeMapped()
            =>  MyRouting
                .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Reviews/Edit/{1}")
                     .WithMethod(HttpMethod.Post))
                 .To<ReviewsController>(c => c
                 .Edit(1, With.Any<ReviewFormModel>()));

        [Fact]
        public void PostDeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Reviews/Delete/{1}")
                     .WithMethod(HttpMethod.Post))
                 .To<ReviewsController>(c => c
                 .Delete(1, With.Any<ReviewDeleteFormModel>()));
    }
}
