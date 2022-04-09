using LogisticsSystem.Controllers;
using LogisticsSystem.Models.Loads;
using MyTested.AspNetCore.Mvc;
using Xunit;

using static LogisticsSystem.Test.Data.Loads;

namespace LogisticsSystem.Test.Routing
{
    public class LoadsRoutingTest
    {
        [Fact]
        public void PostAddShouldBeMapped()
          => MyRouting
              .Configuration()
               .ShouldMap(request => request
                  .WithPath($"/Loads/Add")
                   .WithMethod(HttpMethod.Post))
               .To<LoadsController>(c => c
               .Add(With.Any<LoadFormModel>()));

        [Fact]
        public void PostEditShoulBeMapped()
          => MyRouting
              .Configuration()
               .ShouldMap(request => request
                  .WithPath($"/Loads/Edit/{LoadTestId}")
                   .WithMethod(HttpMethod.Post))
               .To<LoadsController>(c => c
               .Edit(LoadTestId, With.Any<LoadFormModel>()));

        [Fact]
        public void PostDeleteShoulBeMapped()
      => MyRouting
          .Configuration()
           .ShouldMap(request => request
              .WithPath($"/Loads/Delete/{LoadTestId}")
               .WithMethod(HttpMethod.Post))
           .To<LoadsController>(c => c
           .Delete(LoadTestId, With.Any<LoadDeleteFormModel>()));
    }
}
