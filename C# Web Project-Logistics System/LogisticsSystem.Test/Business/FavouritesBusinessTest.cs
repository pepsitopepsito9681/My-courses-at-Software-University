using FluentAssertions;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using LogisticsSystem.Services.Favourites.Models;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Models.Loads;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Favourites;

namespace LogisticsSystem.Test.Business
{


    public class FavouritesBusinessTest
    {
        [Fact]
        public void AddShouldReturnCorrectDataAndRedirectTo()
            => MyPipeline
                    .Configuration()
                    .ShouldMap(request => request
                       .WithPath($"/Favourites/Add/{LoadTestId}")
                       .WithUser())
                     .To<FavouritesController>(c => c.Add(LoadTestId))
                      .Which(controller => controller.WithData(GetLoad()))
                      .ShouldHave()
                      .ActionAttributes(attributes => attributes
                            .RestrictingForAuthorizedRequests())
                       .Data(data => data.WithSet<Favourite>(set =>
                       {
                           var favourite = set.FirstOrDefault();

                           favourite.LoadId.Should().Be(LoadTestId);

                           favourite.UserId.Should().Be(TestUser.Identifier);

                       }))
                       .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                       .AndAlso()
                       .ShouldReturn()
                       .Redirect(redirect => redirect
                             .To<LoadsController>(c => c
                             .Details(LoadTestId, With.Any<LoadsDetailsQueryModel>())));

        [Fact]
        public void AddShouldReturnNotFoundWhenLoadDoesNotExists()
      => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                 .WithPath($"/Favourites/Add/{LoadTestId}")
                 .WithUser())
               .To<FavouritesController>(c => c.Add(LoadTestId))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
                  .AndAlso()
                 .ShouldReturn()
                 .NotFound();

        [Fact]
        public void AddShouldReturnBadRequestWhenUserIsAdmin()
          => MyPipeline
                  .Configuration()
                  .ShouldMap(request => request
                     .WithPath($"/Favourites/Add/{LoadTestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                   .To<FavouritesController>(c => c.Add(LoadTestId))
                    .Which(controller => controller.WithData(GetLoad()))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                      .AndAlso()
                     .ShouldReturn()
                     .BadRequest();


        [Fact]
        public void MyFavouritesShouldReturnViewAndCorrectDataAndModel()
            => MyPipeline
                   .Configuration()
                   .ShouldMap(request => request
                       .WithPath("/Favourites/MyFavourites")
                        .WithUser())
                    .To<FavouritesController>(c => c.MyFavourites())
                    .Which(controller => controller.WithData(GetFavourites(5, true, false)))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                            .RestrictingForAuthorizedRequests())
                     .AndAlso()
                     .ShouldReturn()
                     .View(view => view
                           .WithModelOfType<List<FavouriteServiceModel>>()
                           .Passing(model => model.Should().HaveCount(5)));




        [Fact]
        public void DeleteShouldDeleteFavouriteAndReturnRedirectToAllWithCorrectData()
            => MyPipeline
                   .Configuration()
                   .ShouldMap(request => request
                       .WithPath($"/Favourites/Delete/{1}")
                        .WithUser())
                    .To<FavouritesController>(c => c.Delete(1))
                    .Which(controller => controller.WithData(GetFavourites(1)))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                            .RestrictingForAuthorizedRequests())
                      .Data(data => data.WithSet<Favourite>(set =>
                      {
                          set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                          set.Should().BeEmpty();
                      }))
                     .TempData(tempData => tempData
                            .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                      .AndAlso()
                      .ShouldReturn()
                      .Redirect(redirect => redirect
                              .To<FavouritesController>(c => c
                              .MyFavourites()));


        [Fact]
        public void DeleteShouldReturnBadRequestWhenUserIsAdmin()
         => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Favourites/Delete/{1}")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                  .To<FavouritesController>(c => c.Delete(1))
                   .Which()
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                     .AndAlso()
                    .ShouldReturn()
                    .BadRequest();

    }
}
