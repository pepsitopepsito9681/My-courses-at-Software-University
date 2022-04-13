using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Models.Reviews;
using LogisticsSystem.Services.Reviews.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using static LogisticsSystem.Test.Data.Comments;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Reviews;

namespace LogisticsSystem.Test.Business
{


    public class ReviewsBusinessTest
    {
        public string Information = GetInformation();

        [Theory]
        [InlineData(1, 3)]
        public void DetailsShouldReturnViewWithCorrectDataAndModel(
            int detailsId,
            int commentsCount)
            => MyPipeline
                  .Configuration()
                  .ShouldMap(request => request
                  .WithPath($"/Reviews/Details/{detailsId}/{Information}")
                  .WithUser())
            .To<ReviewsController>(c => c.Details(detailsId, Information))
            .Which(controller => controller
                      .WithData(GetReviews(detailsId))
                      .AndAlso()
                      .WithData(GetComments(commentsCount))
            .ShouldReturn()
            .View(view => view.WithModelOfType<ReviewDetailsWithCommentsModel>()
                  .Passing(model =>
                  {
                      model.Should().NotBeNull();
                      model.Review.Should().NotBeNull();
                      model.Review.Id.Should().Be(1);
                      model.Review.Title.Should().Be(Title);
                      model.Comments.Should().HaveCount(commentsCount);
                  })));



        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
          => MyPipeline
                .Configuration()
                 .ShouldMap(request => request.WithPath($"/Reviews/Add/{LoadTestId}")
                    .WithUser()
                    .WithAntiForgeryToken())
                  .To<ReviewsController>(c => c.Add(LoadTestId))
                  .Which(controller => controller
                        .WithData(GetLoad()))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                    .View();


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnBadRequestWhenReviewIsNotPublic()
          => MyPipeline
                .Configuration()
                 .ShouldMap(request => request.WithPath($"/Reviews/Add/{LoadTestId}")
                    .WithUser()
                    .WithAntiForgeryToken())
                  .To<ReviewsController>(c => c.Add(LoadTestId))
                  .Which(controller => controller
                        .WithData(GetLoad(LoadTestId, true, false, false)))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                    .BadRequest();




        [Fact]
        public void MyReviewsShouldBeForAuthorizedUsersAndReturnViewWithCorrectDataAndModel()
           => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request
                       .WithPath("/Reviews/MyReviews")
                       .WithUser()
                       .WithAntiForgeryToken())
                   .To<ReviewsController>(c => c.MyReviews())
                   .Which(controller => controller.WithData(GetReviews()))
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view
                       .WithModelOfType<List<ReviewListingServiceModel>>()
                        .Passing(model => model.Should().HaveCount(5)));


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnViewAndCorrectModelAndData()
          => MyPipeline
                  .Configuration()
                 .ShouldMap(request => request.WithPath($"/Reviews/Edit/{1}")
                  .WithUser())
                .To<ReviewsController>(c => c.Edit(1))
                .Which(controller => controller.WithData(GetReviews(1)))
                .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view.WithModelOfType<ReviewFormModel>()
                   .Passing(model =>
                   {
                       model.Content.Should().Be(TestContent);
                       model.Rating.Should().Be(ReviewKind.Excellent);
                   }));

        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenReviewIsNotOfUser()
        => MyPipeline
                .Configuration()
               .ShouldMap(request => request.WithPath($"/Reviews/Edit/{1}")
                .WithUser())
              .To<ReviewsController>(c => c.Edit(1))
              .Which(controller => controller.WithData(GetReviews(1, sameUser: false)))
              .ShouldHave()
                 .ActionAttributes(attributes => attributes
                                .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .BadRequest();

        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnNotFoundWhenReviewDoesNotExistsAndUserIsAdmin()
   => MyPipeline
           .Configuration()
          .ShouldMap(request => request.WithPath($"/Reviews/Edit/{1}")
           .WithUser(new[] { AdminConstants.AdministratorRoleName }))
         .To<ReviewsController>(c => c.Edit(1))
         .Which()
         .ShouldHave()
            .ActionAttributes(attributes => attributes
                           .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .NotFound();

        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                 .ShouldMap(request => request.WithPath($"/Reviews/Delete/{1}")
                 .WithUser())
                 .To<ReviewsController>(c => c.Delete(1))
                 .Which(controller => controller.WithData(GetReviews(1)))
                .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View();


        [Fact]
        public void GetDeleteShouldReturnBadRequestWhenReviewIsNotOfUser()
    => MyPipeline
         .Configuration()
         .ShouldMap(request => request.WithPath($"/Reviews/Delete/{2}")
         .WithUser())
         .To<ReviewsController>(c => c.Delete(2))
         .Which(controller => controller.WithData(GetReviews(1)))
         .ShouldHave()
          .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .BadRequest();


        [Fact]
        public void GetDeleteShouldReturnNotFoundWhenQuestionDoesNotExist()
   => MyPipeline
        .Configuration()
        .ShouldMap(request => request.WithPath($"/Reviews/Delete/{2}")
        .WithUser(new[] { AdminConstants.AdministratorRoleName }))
        .To<ReviewsController>(c => c.Delete(2))
        .Which(controller => controller.WithData(GetReviews(1)))
        .ShouldHave()
         .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .NotFound();


    }
}
