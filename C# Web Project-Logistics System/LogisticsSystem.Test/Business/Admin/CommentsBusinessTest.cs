using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Areas.Admin.Models.Comments;
using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Comments;
using CommentsController = LogisticsSystem.Areas.Admin.Controllers.CommentsController;

namespace LogisticsSystem.Test.Business.Admin
{


    public class CommentsBusinessTest
    {
        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
         => MyPipeline
              .Configuration()
              .ShouldMap(request => request.WithPath("/Admin/Comments/All")
              .WithUser(new[] { AdminConstants.AdministratorRoleName })
              .WithAntiForgeryToken())
              .To<CommentsController>(c => c.All(With.Default<CommentsQueryModel>()))
               .Which((System.Action<MyTested.AspNetCore.Mvc.Builders.Contracts.Pipeline.IWhichControllerInstanceBuilder<CommentsController>>)(controller => controller
                  .WithData(Data.Comments.GetComments())))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<CommentsQueryModel>()
                .Passing(model => model.Comments.Should().NotBeEmpty()));

        [Fact]
        public void ChangeVisibilityShouldChangeResponseAndRedirectToAll()
          => MyPipeline
                .Configuration()
                 .ShouldMap(request => request
                  .WithPath($"/Admin/Comments/ChangeVisibility/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
                 .To<CommentsController>(c => c.ChangeVisibility(1))
                 .Which(controller => controller
                  .WithData(GetComments(1)))
                 .ShouldHave()
                  .Data(data => data
                       .WithSet<Comment>(set => set
                            .Any(x => x.Id == 1 && !x.IsPublic)))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                      .To<CommentsController>(c => c.All(With.Any<CommentsQueryModel>())));


        [Fact]
        public void DeleteShouldDeleteCommentAndRedirectToAll()
          => MyPipeline
             .Configuration()
              .ShouldMap(request => request
                  .WithPath($"/Admin/Comments/Delete/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
                     .To<CommentsController>(c => c.Delete(1))
                   .Which(controller => controller
                         .WithData(GetComments(1)))
                  .ShouldHave()
                  .TempData(tempData => tempData
                               .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                  .Data(data => data.WithSet<Comment>(set =>
                  {
                      set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                      set.Should().BeEmpty();

                  }))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                      .To<CommentsController>(c => c.All(With.Any<CommentsQueryModel>())));


        [Fact]
        public void DeleteShouldReturnNotFoundWhenCommentIdIsInvalid()
          => MyPipeline
             .Configuration()
              .ShouldMap(request => request
                  .WithPath($"/Admin/Comments/Delete/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                     .To<CommentsController>(c => c.Delete(1))
                   .Which()
                   .ShouldReturn()
                   .NotFound();


    }
}
