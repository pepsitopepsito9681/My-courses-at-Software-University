using FluentAssertions;
using LogisticsSystem.Controllers.Api;
using LogisticsSystem.Models.Api.Loads;
using LogisticsSystem.Services.Loads.Models;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static LogisticsSystem.Test.Data.Loads;

namespace LogisticsSystem.Test.Business.Api
{


    public class LoadsApiBusinessTest
    {
        [Fact]
        public void AllShouldReturnCorrectModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/api/loads")
                    .WithMethod(HttpMethod.Get))
                .To<LoadsApiController>(c => c.All(With.Default<AllLoadsApiRequestModel>()))
                .Which(controller => controller.WithData(GetLoads(2)))

                  .ShouldReturn()
                  .ActionResult<LoadQueryServiceModel>(result => result
                     .Passing(model =>
                     {
                         model.Loads.Should().HaveCount(2);

                         model.TotalLoads.Should().Be(2);

                     }));



    }
}
