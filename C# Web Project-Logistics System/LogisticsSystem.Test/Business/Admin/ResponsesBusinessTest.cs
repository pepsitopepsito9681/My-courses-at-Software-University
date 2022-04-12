using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Areas.Admin.Models.Responses;
using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using ResponsesController = LogisticsSystem.Areas.Admin.Controllers.ResponsesController;
using static LogisticsSystem.Test.Data.Responses;

namespace LogisticsSystem.Test.Business.Admin
{

    public class ResponsesBusinessTest
    {

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request.WithPath("/Admin/Responses/All")
                 .WithUser(new[] { AdminConstants.AdministratorRoleName })
                 .WithAntiForgeryToken())
                 .To<ResponsesController>(c => c.All(With.Default<ResponsesQueryModel>()))
                  .Which(controller => controller
                     .WithData(GetResponses()))
                   .ShouldReturn()
                   .View(view => view
                   .WithModelOfType<ResponsesQueryModel>()
                   .Passing(model => model.Responses.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeResponseAndRedirectToAll()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request
                    .WithPath($"/Admin/Responses/ChangeVisibility/{1}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                   .To<ResponsesController>(c => c.ChangeVisibility(1))
                   .Which(controller => controller
                    .WithData(GetResponse()))
                   .ShouldHave()
                    .Data(data => data
                         .WithSet<Response>(set => set
                            .Any(x => x.Id == 1 && !x.IsPublic)))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<ResponsesController>(c => c.All(With.Any<ResponsesQueryModel>())));


        [Fact]
        public void DeleteShouldDeleteResponseAndRedirectToAll()
            => MyPipeline
               .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Admin/Responses/Delete/{1}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                       .To<ResponsesController>(c => c.Delete(1))
                       .Which(controller => controller.WithData(GetResponse()))
                    .ShouldHave()
                    .TempData(tempData => tempData
                               .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                    .Data(data => data.WithSet<Response>(set =>
                    {
                        set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                        set.Should().BeEmpty();

                    }))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<ResponsesController>(c => c.All(With.Any<ResponsesQueryModel>())));

        [Fact]
        public void DeleteShouldReturnNotFoundWhenResponseIdIsInvalid()
           => MyPipeline
              .Configuration()
               .ShouldMap(request => request
                   .WithPath($"/Admin/Responses/Delete/{1}")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                      .To<ResponsesController>(c => c.Delete(1))
                    .Which()
                    .ShouldReturn()
                    .NotFound();

    }

}
