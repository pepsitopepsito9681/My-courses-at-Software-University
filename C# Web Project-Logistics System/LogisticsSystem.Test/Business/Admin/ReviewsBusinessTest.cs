using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Areas.Admin.Models.Reviews;
using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using ReviewsController = LogisticsSystem.Areas.Admin.Controllers.ReviewsController;
using static LogisticsSystem.Test.Data.Reviews;

namespace LogisticsSystem.Test.Business.Admin
{

    public class ReviewsBusinessTest
    {
        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
          => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                  .WithPath("/Admin/Reviews/All")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
              .To<ReviewsController>(c => c.All(With.Default<ReviewsQueryModel>()))
              .Which(controller => controller
                  .WithData(GetReviews()))
              .ShouldReturn()
              .View(view => view
                 .WithModelOfType<ReviewsQueryModel>()
               .Passing(model => model.Reviews.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeReviewAndRedirectToAll()
         => MyPipeline
               .Configuration()
                .ShouldMap(request => request
                 .WithPath($"/Admin/Reviews/ChangeVisibility/{1}")
                  .WithUser(new[] { AdminConstants.AdministratorRoleName })
                  .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.ChangeVisibility(1))
                .Which(controller => controller
                 .WithData(GetReviews(1)))
                .ShouldHave()
                 .Data(data => data
                      .WithSet<Review>(set =>
                      {
                          var review = set.FirstOrDefault(r => !r.IsPublic);

                          review.Should().NotBeNull();
                          review.Title.Should().NotBeNull();
                          review.Title.Should().Be(Data.Reviews.Title);
                      }))
                  .AndAlso()
                  .ShouldReturn()
                  .Redirect(redirect => redirect
                     .To<ReviewsController>(c => c.All(With.Any<ReviewsQueryModel>())));
    }
}
