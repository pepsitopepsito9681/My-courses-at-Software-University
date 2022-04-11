using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Models.Loads;
using LogisticsSystem.Models.Reviews;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Reviews;

namespace LogisticsSystem.Test.Controllers
{

    public class ReviewsControllerTest
    {
        [Theory]
        [InlineData(ReviewKind.Excellent, Title, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
          ReviewKind rating,
          string title,
          string content
            )
          => MyController<ReviewsController>
              .Instance(controller => controller
                        .WithUser()
                        .WithData(GetLoad()))
               .Calling(c => c.Add(LoadTestId, new ReviewFormModel
               {
                   Rating = rating,
                   Title = title,
                   Content = content

               }))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Review>(set => set
                             .Any(x =>
                             x.Rating == rating &&
                             x.Title == title &&
                             x.Content == content &&
                             x.UserId == TestUser.Identifier)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                  .Redirect(redirect => redirect
                        .To<LoadsController>(c => c
                         .Details(LoadTestId, With.Any<LoadsDetailsQueryModel>())));

        [Theory]
        [InlineData(ReviewKind.Excellent, Title, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnBadRequestWhenLoadIsNotPublic(
        ReviewKind rating,
        string title,
        string content
          )
        => MyController<ReviewsController>
            .Instance(controller => controller
                      .WithUser()
                      .WithData(GetLoad(IsPublic: false)))
             .Calling(c => c.Add(LoadTestId, new ReviewFormModel
             {
                 Rating = rating,
                 Title = title,
                 Content = content

             }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests()
                  .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
            .ShouldReturn()
            .BadRequest();


        [Theory]
        [InlineData(ReviewKind.Excellent, Title, TestContent)]
        public void PostEditShouldBeForAuthorizedUsersAndReturnCorrectDataAndModelAndRedirectTo(
            ReviewKind rating,
            string title,
            string content)
          => MyController<ReviewsController>
               .Instance(controller => controller
                        .WithUser()
                        .WithData(GetReviews(1)))
               .Calling(c => c.Edit(1, new ReviewFormModel
               {
                   Rating = rating,
                   Title = title,
                   Content = content

               }))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests()
                      .RestrictingForHttpMethod(HttpMethod.Post))
               .ValidModelState()
               .Data(data => data.WithSet<Review>(set => set.Any(x =>
                                     x.Rating == rating &&
                                    x.Title == title &&
                                    x.Content == content)))
               .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                       .To<ReviewsController>(c => c
                       .MyReviews()));

        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenReviewIsNotOfUser()
        => MyController<ReviewsController>
             .Instance(controller => controller
                      .WithUser()
                      .WithData(GetReviews(1, sameUser: false)))
             .Calling(c => c.Edit(1, With.Any<ReviewFormModel>()))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
            .ShouldReturn()
            .BadRequest();

        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndWhenUserIsAdminReturnBadRequestWhenReviewDoesNotExist()
        => MyController<ReviewsController>
             .Instance(controller => controller
                      .WithUser(new[] { AdminConstants.AdministratorRoleName }))
             .Calling(c => c.Edit(1, new ReviewFormModel
             {
                 Rating = ReviewKind.Excellent,
                 Title = Title,
                 Content = TestContent,

             }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
             .ValidModelState()
             .AndAlso()
            .ShouldReturn()
            .NotFound();



        [Fact]
        public void PostDeleteShouldDeleteReviewAndReturnCorrectDataAndRedirectToMyReviews()
            => MyController<ReviewsController>
               .Instance(controller => controller
                        .WithUser()
                        .WithData(GetReviews(1)))
                 .Calling(c => c.Delete(1, new ReviewDeleteFormModel
                 {
                     ConfirmDeletion = true

                 }))
              .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests()
                                  .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                   .Data(data => data.WithSet<Review>(set =>
                   {
                       set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                       set.Should().BeEmpty();
                   }))
               .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                       .To<ReviewsController>(c => c
                       .MyReviews()));
    }
}
