using FluentAssertions;
using LogisticsSystem.Controllers.Api;
using LogisticsSystem.Services.Statistics.Models;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Users;

namespace LogisticsSystem.Test.Business.Api
{


    public class StatisticsBusinessTest
    {
        [Theory]
        [InlineData(5, 5)]
        public void TotalShouldReturnCorrectModel(
            int usersCount,
            int loadsCount)
            => MyPipeline
                .Configuration()
                 .ShouldMap(request => request
                    .WithPath("/api/statistics")
                    .WithMethod(HttpMethod.Get))
                 .To<StatisticsApiController>(c => c.GetStatistics())
                 .Which(controller => controller
                       .WithData(GetUsers())
                       .AndAlso()
                       .WithData(GetLoads())
                  .ShouldReturn()
                  .ResultOfType<StatisticsServiceModel>(result => result
                        .Passing(model =>
                        {
                            model.TotalLoads.Should().Be(loadsCount);

                            model.TotalUsers.Should().Be(usersCount);

                        })));

    }
}
