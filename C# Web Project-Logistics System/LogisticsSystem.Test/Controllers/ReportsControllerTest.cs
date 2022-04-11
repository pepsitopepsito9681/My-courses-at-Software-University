using LogisticsSystem.Controllers;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Models.Reports;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Models.Loads;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Reports;

namespace LogisticsSystem.Test.Controllers
{

    public class ReportsControllerTest
    {

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
            => MyController<ReportsController>
                  .Instance(controller => controller
                       .WithUser()
                       .WithData(GetLoad()))
                  .Calling(c => c.Add(LoadTestId))
                 .ShouldHave()
                  .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .View();


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnNotFound()
         => MyController<ReportsController>
            .Instance(controller => controller
                 .WithUser()
                 .WithData(GetLoad(IsPublic: false)))
            .Calling(c => c.Add(LoadTestId))
           .ShouldHave()
            .ActionAttributes(attributes => attributes
            .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
            .NotFound();


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnBadRequest()
        => MyController<ReportsController>
           .Instance(controller => controller
                .WithUser()
                .WithData(GetLoad()).WithData(GetReports(1)))
           .Calling(c => c.Add(LoadTestId))
          .ShouldHave()
           .ActionAttributes(attributes => attributes
           .RestrictingForAuthorizedRequests())
          .AndAlso()
          .ShouldReturn()
          .BadRequest();

        [Theory]
        [InlineData(ReportKind.Nudity, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
         ReportKind reportKind,
         string content)
         => MyController<ReportsController>
             .Instance(controller => controller
                       .WithUser()
                       .WithData(GetLoad()))
              .Calling(c => c.Add(LoadTestId, new ReportFormModel
              {
                  ReportKind = reportKind,
                  Content = content

              }))
              .ShouldHave()
              .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests()
                   .RestrictingForHttpMethod(HttpMethod.Post))
               .ValidModelState()
               .Data(data => data
                    .WithSet<Report>(set => set
                            .Any(x =>
                            x.ReportKind == reportKind &&
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
        [InlineData(ReportKind.Nudity, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnNotFoundWhenLoadIsNotPublic(
        ReportKind reportKind,
        string content
        )
         => MyController<ReportsController>
          .Instance(controller => controller
                    .WithUser()
                    .WithData(GetLoad(IsPublic: false)))
        .Calling(c => c.Add(LoadTestId, new ReportFormModel
        {
            ReportKind = reportKind,
            Content = content

        }))
        .ShouldHave()
        .ActionAttributes(attributes => attributes
            .RestrictingForAuthorizedRequests()
             .RestrictingForHttpMethod(HttpMethod.Post))
          .AndAlso()
          .ShouldReturn()
          .NotFound();


        [Theory]
        [InlineData(ReportKind.Nudity, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnBadRequestWhenUserAlreadyReportedLoad(
        ReportKind reportKind,
        string content
          )
        => MyController<ReportsController>
            .Instance(controller => controller
                .WithUser()
                .WithData(GetLoad()).WithData(GetReports(1)))
        .Calling(c => c.Add(LoadTestId, new ReportFormModel
        {
            ReportKind = reportKind,
            Content = content
        }))
        .ShouldHave()
        .ActionAttributes(attributes => attributes
            .RestrictingForAuthorizedRequests()
             .RestrictingForHttpMethod(HttpMethod.Post))
          .AndAlso()
          .ShouldReturn()
          .BadRequest();

    }
}
