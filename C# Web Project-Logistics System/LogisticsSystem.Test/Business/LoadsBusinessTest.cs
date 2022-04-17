using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Models.Loads;
using LogisticsSystem.Services.Loads.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using static LogisticsSystem.Test.Data.Kinds;
using static LogisticsSystem.Test.Data.Dealers;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Questions;
using static LogisticsSystem.Test.Data.Reviews;

namespace LogisticsSystem.Test.Business
{


    public class LoadsBusinessTest
    {
        [Fact]
        public void MyLoadsShouldBeForAuthorizedUsersReturnViewWithCorrectDataAndModel()
        => MyPipeline
             .Configuration()
             .ShouldMap(request => request.WithPath("/Loads/MyLoads")
             .WithUser())
            .To<LoadsController>(c => c.MyLoads())
            .Which(controller => controller.WithData(GetLoads()))
            .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                     .WithModelOfType<List<LoadServiceModel>>()
                     .Passing(model => model.Should().HaveCount(5)));


        [Fact]
        public void MyLoadsShouldRedirectToAllWhenUserIsAdmin()
        => MyPipeline
              .Configuration()
               .ShouldMap(request => request
                   .WithPath("/Loads/MyLoads")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .To<LoadsController>(c => c.MyLoads())
               .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                              .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                   .To<LoadsController>(c => c
                   .All(With.Any<LoadsQueryModel>())));

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersReturnViewAndCorrectModel()
            => MyPipeline
               .Configuration()
               .ShouldMap(request => request.WithPath("/Loads/Add")
               .WithUser())
               .To<LoadsController>(c => c.Add())
               .Which(controller => controller
                   .WithData(GetDealer())
                   .AndAlso()
                   .WithData(GetKinds()))
               .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                  .View(view => view
                     .WithModelOfType<LoadFormModel>()
                     .Passing(model =>
                     {
                         model.Kinds.Should().HaveCount(5);
                         model.SubKinds.Should().HaveCount(5);

                     }));


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnRedirectToBecomeDealerWhenUserIsNotDealer()
            => MyPipeline
               .Configuration()
               .ShouldMap(request => request.WithPath("/Loads/Add")
               .WithUser())
               .To<LoadsController>(c => c.Add())
               .Which()
               .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                      .To<TradersController>(c => c
                      .Become()));


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersReturnViewWithCorrectDataAndModel()
          => MyPipeline
              .Configuration()
               .ShouldMap(request => request.WithPath($"/Loads/Edit/{LoadTestId}")
               .WithUser())
              .To<LoadsController>(c => c.Edit(LoadTestId))
            .Which(controller => controller
                .WithData(GetLoad())
           .ShouldHave()
               .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
            .View(view => view
                 .WithModelOfType<LoadFormModel>()));

        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsNotDealer()
          => MyPipeline
                .Configuration()
             .ShouldMap(request => request.WithPath($"/Loads/Edit/{LoadTestId}")
             .WithUser())
             .To<LoadsController>(c => c.Edit(LoadTestId))
             .Which()
             .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .BadRequest();


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersReturnBadRequestWhenLoadIsNotOfUser()
            => MyPipeline
                .Configuration()
             .ShouldMap(request => request.WithPath($"/Loads/Edit/{LoadTestId}")
             .WithUser())
             .To<LoadsController>(c => c.Edit(LoadTestId))
             .Which(controller => controller
                  .WithData(GetDealer())
                  .AndAlso()
                  .WithData(GetLoad(userSame: false))
             .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .BadRequest());


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersReturnNotFoundWhenLoadDoesNotExist()
           => MyPipeline
               .Configuration()
            .ShouldMap(request => request.WithPath($"/Loads/Edit/{LoadTestId}")
            .WithUser())
            .To<LoadsController>(c => c.Edit(LoadTestId))
            .Which(controller => controller
                 .WithData(GetDealer()))
            .ShouldHave()
                .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
              .AndAlso()
              .ShouldReturn()
              .NotFound();


        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request.WithPath($"/Loads/Delete/{LoadTestId}")
                   .WithUser())
             .To<LoadsController>(c => c.Delete(LoadTestId))
             .Which(controller => controller
                  .WithData(GetLoad())
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View());



        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsNotDealer()
       => MyPipeline
             .Configuration()
          .ShouldMap(request => request.WithPath($"/Loads/Delete/{LoadTestId}")
          .WithUser())
          .To<LoadsController>(c => c.Delete(LoadTestId))
          .Which()
          .ShouldHave()
              .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .BadRequest();



        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersReturnBadRequestWhenLoadIsNotOfDealer()
            => MyPipeline
                .Configuration()
             .ShouldMap(request => request.WithPath($"/Loads/Delete/{LoadTestId}")
             .WithUser())
             .To<LoadsController>(c => c.Delete(LoadTestId))
             .Which(controller => controller
                  .WithData(GetDealer())
                  .AndAlso()
                  .WithData(GetLoad(LoadTestId, false))
             .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .BadRequest());

        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersReturnNotFoundWhenLoadDoesNotExist()
          => MyPipeline
              .Configuration()
           .ShouldMap(request => request.WithPath($"/Loads/Delete/{LoadTestId}")
           .WithUser())
           .To<LoadsController>(c => c.Delete(LoadTestId))
           .Which(controller => controller
                .WithData(GetDealer()))
           .ShouldHave()
               .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
             .NotFound();


        [Fact]
        public void DetailsShouldReturnViewWithCorrectDataAndModel()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                        .WithPath($"/Loads/Details/{LoadTestId}")
                 .WithUser())
                 .To<LoadsController>(c => c.Details(LoadTestId, With.Default<LoadsDetailsQueryModel>()))
                 .Which(controller => controller
                       .WithData(GetLoad())
                        .AndAlso()
                        .WithData(GetQuestionsByLoad())
                        .AndAlso()
                        .WithData(GetReviewsByLoad()))
             .ShouldReturn()
             .View(view => view.WithModelOfType<LoadsDetailsQueryModel>()
              .Passing(model =>
              {
                  model.Load.Should().NotBeNull();
                  model.LoadReviewsStatistics.Should().NotBeNull();
                  model.SimilarLoads.Should().NotBeNull();
                  model.Questions.Should().HaveCount(5);
                  model.Reviews.Should().HaveCount(5);

              }));

        [Fact]
        public void DetailsShouldReturnNotFoundWhenLoadDoesNotExist()
          => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                      .WithPath($"/Loads/Details/{LoadTestId}")
               .WithUser())
               .To<LoadsController>(c => c.Details(LoadTestId, With.Default<LoadsDetailsQueryModel>()))
               .Which()
               .ShouldReturn()
               .NotFound();



        [Fact]
        public void DetailsShouldReturnBadRequestWhenLoadIsNotPublic()
        => MyPipeline
             .Configuration()
             .ShouldMap(request => request
                    .WithPath($"/Loads/Details/{LoadTestId}")
             .WithUser())
             .To<LoadsController>(c => c.Details(LoadTestId, With.Default<LoadsDetailsQueryModel>()))
             .Which(controller => controller.WithData(GetLoad(LoadTestId, false, false, false))
             .ShouldReturn()
             .BadRequest());


        [Fact]
        public void AllShouldReturnDefaultViewWithCorrectModel()
            => MyPipeline
             .Configuration()
             .ShouldMap(request => request
                    .WithPath($"/Loads/All"))
                .To<LoadsController>(c => c.All(With.Default<LoadsQueryModel>()))
               .Which(controller => controller
                   .WithData(GetLoads(10, false, false))
                   .AndAlso()
                   .WithData(GetKinds()))
                .ShouldReturn()
               .View(view => view
                    .WithModelOfType<LoadsQueryModel>()
                     .Passing(model =>
                     {
                         model.Loads.Should().HaveCount(9);
                         model.CurrentPage.Should().Be(1);
                         model.TotalLoads.Should().Be(10);
                         model.LoadSorting.Should().Be(LoadSorting.Default);
                         model.Kinds.Should().HaveCount(5);
                         model.SubKinds.Should().HaveCount(5);
                     }));


        [Theory]
        [InlineData("TestKind", "TestSubKind", 0, "Title")]
        public void AllWithKindSubKindAndSearchTermShouldReturnDefaultViewWithCorrectModel(
            string kindName,
            string subKindName,
            int loadSorting,
            string searchTerm)
          => MyPipeline
           .Configuration()
           .ShouldMap(request => request
           .WithLocation($"/Loads/All?Kind={kindName}&SubKind={subKindName}&LoadSorting={loadSorting}&SearchTerm={searchTerm}"))
              .To<LoadsController>(c => c.All(new LoadsQueryModel
              {
                  Kind = kindName,
                  SubKind = subKindName,
                  SearchTerm = searchTerm,
                  LoadSorting = (LoadSorting)loadSorting,
              }))
             .Which(controller => controller
                 .WithData(GetLoad()))
              .ShouldReturn()
             .View(view => view
                  .WithModelOfType<LoadsQueryModel>()
                   .Passing(model =>
                   {
                       model.Loads.Should().HaveCount(1);
                       model.CurrentPage.Should().Be(1);
                       model.TotalLoads.Should().Be(1);
                       model.LoadSorting.Should().Be((LoadSorting)loadSorting);
                       model.Kind.Should().Be(kindName);
                       model.SubKind.Should().Be(subKindName);

                   }));

        [Theory]
        [InlineData(12, 2)]
        public void GetAllWithPageShouldReturnDefaultViewWithCorrectModel(
            int loadsCount,
            int page)
          => MyPipeline
           .Configuration()
           .ShouldMap(request => request
           .WithLocation($"/Loads/All?CurrentPage={page}"))
              .To<LoadsController>(c => c.All(new LoadsQueryModel
              {
                  CurrentPage = page

              }))
             .Which(controller => controller
                 .WithData(GetLoads(loadsCount)))
              .ShouldReturn()
               .View(view => view
                  .WithModelOfType<LoadsQueryModel>()
                   .Passing(model =>
                   {
                       model.Loads.Should().HaveCount(loadsCount - LoadsQueryModel.LoadsPerPage);
                       model.CurrentPage.Should().Be(page);


                   }));


        [Fact]
        public void AllShouldReturnBadRequestWhenKindSubKindConditionIsInvalid()
            => MyMvc
            .Pipeline()
              .ShouldMap(request => request
        .WithLocation($"/Loads/All?Kind=&SubKind={"TestSubKind"}&LoadSorting=0&SearchTerm="))
               .To<LoadsController>(c => c.All(new LoadsQueryModel
               {
                   SubKind = "TestSubKind"
               }))
               .Which(controller => controller
                   .WithData(GetKinds()))
                .ShouldReturn()
                .BadRequest();


    }
}
