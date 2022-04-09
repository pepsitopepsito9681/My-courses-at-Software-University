using LogisticsSystem.Controllers;
using LogisticsSystem.Models.Reports;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static LogisticsSystem.Test.Data.Loads;

namespace LogisticsSystem.Test.Routing
{
    

   public class ReportsRoutingTest
    {
        [Fact]
        public void GetAddShouldBeMapped()
           => MyRouting
                .Configuration()
                 .ShouldMap($"/Reports/Add/{LoadTestId}")
                  .To<ReportsController>(c => c.Add(LoadTestId));


        [Fact]
        public void PostAddShouldBeMapped()
           => MyRouting
                .Configuration()
                 .ShouldMap(request=>request
                      .WithPath($"/Reports/Add/{LoadTestId}")
                      .WithMethod(HttpMethod.Post))
                  .To<ReportsController>(c => c.Add(LoadTestId, With.Any<ReportFormModel>()));



    }
}
