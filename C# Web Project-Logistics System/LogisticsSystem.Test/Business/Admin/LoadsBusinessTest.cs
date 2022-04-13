using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Areas.Admin.Models.Loads;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Models.Loads;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Reports;
using LoadsController = LogisticsSystem.Areas.Admin.Controllers.LoadsController;

namespace LogisticsSystem.Test.Business.Admin
{

    public class LoadsBusinessTest
    {

        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Loads/Existing")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<LoadsController>(c => c.Existing(With.Default<LoadsQueryModel>()))
                .Which(controller => controller
                    .WithData(GetLoads()))
                .ShouldReturn()
            .View(view => view.WithModelOfType<LoadsQueryModel>()
               .Passing(model => model.Loads.Should().NotBeEmpty()));



        [Fact]
        public void DeletedShouldReturnCorrectViewAndModel()
             => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Loads/Deleted")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<LoadsController>(c => c.Deleted(With.Default<LoadsQueryModel>()))
                .Which(controller => controller
                    .WithData(GetLoads(3, true)))
                .ShouldReturn()
            .View(view => view.WithModelOfType<LoadsQueryModel>()
               .Passing(model => model.Loads.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeLoadAndRedirectToAll()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request
                    .WithPath($"/Admin/Loads/ChangeVisibility/{LoadTestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                   .To<LoadsController>(c => c.ChangeVisibility(LoadTestId))
                   .Which(controller => controller
                    .WithData(GetLoad()))
                   .ShouldHave()
                    .Data(data => data
                         .WithSet<Load>(set => set
                              .Any(x => x.Id == LoadTestId && !x.IsPublic)))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<LoadsController>(c => c.Existing(With.Any<LoadsQueryModel>())));



        [Fact]
        public void ReviveShouldChangeLoadAndRedirectToAll()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                     .WithPath($"/Admin/Loads/Revive/{LoadTestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<LoadsController>(c => c.Revive(LoadTestId))
                .Which(controller => controller
                    .WithData(GetLoad()))
                 .ShouldHave()
                  .Data(data => data
                         .WithSet<Load>(set => set
                              .Any(x => x.Id == LoadTestId && !x.IsDeleted)))
                    .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                         .To<LoadsController>(c => c.Existing(With.Any<LoadsQueryModel>())));


        [Fact]
        public void ReportShouldReturnViewWithCorectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                     .WithPath($"/Admin/Loads/Reports/{LoadTestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                .To<LoadsController>(c => c.Reports(LoadTestId))
                .Which(controller => controller
                    .WithData(GetLoad()).AndAlso().WithData(GetReports(5, sameUser: false)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<LoadReportsDetailsModel>()
                    .Passing(model =>
                    {
                        model.Load.Title.Should().Be("Title");
                        model.Load.Image.Should().Be("TestUrl");
                        model.Reports.Should().HaveCount(5);
                    }));


        [Fact]
        public void ReportShouldReturnNotFoundWhenLoadDoesNotExist()
           => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                    .WithPath($"/Admin/Loads/Reports/{LoadTestId}")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .To<LoadsController>(c => c.Reports(LoadTestId))
               .Which()
               .ShouldReturn()
               .NotFound();

    }
}
