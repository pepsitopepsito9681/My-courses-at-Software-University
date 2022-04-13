using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Models.Loads;
using LogisticsSystem.Models.Questions;
using LogisticsSystem.Services.Questions.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Responses;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.Questions;

namespace LogisticsSystem.Test.Business
{


    public class QuestionsBusinessTest
    {
        public string Information = GetInformation();

        [Theory]
        [InlineData(1, 3)]
        public void DetailsShouldReturnCorrectModelAndView(
            int questionId,
            int responsesCount)
            => MyRouting
                .Configuration()
                  .ShouldMap(request => request
                   .WithPath($"/Questions/Details/{questionId}/{Information}")
                .WithUser()
                .WithAntiForgeryToken())
                  .To<QuestionsController>(c => c.Details(questionId, Information))
               .Which(controller => controller
                     .WithData(GetQuestions(questionId))
                     .AndAlso().
                      WithData(GetResponses(responsesCount))
               .ShouldReturn()
               .View(view => view.WithModelOfType<QuestionDetailsWithResponsesModel>()
                    .Passing(model =>
                    {
                        model.Should().NotBeNull();
                        model.Responses.Should().HaveCount(responsesCount);
                        model.Question.Id.Should().Be(questionId);
                        model.Question.Content.Should().Be($"Question Content {questionId}");

                    }
                    )));

        [Fact]
        public void DetailsShouldReturnNotFoundWhenLoadDoesNotExists()
           => MyRouting
               .Configuration()
                 .ShouldMap(request => request
                  .WithPath($"/Questions/Details/{1}/{Information}"))
                 .To<QuestionsController>(c => c.Details(1, Information))
              .Which()
              .ShouldReturn()
              .NotFound();


        [Fact]
        public void MyQuestionsShouldBeForAuthorizedUsersAndReturnViewWithCorrectDataAndModel()
            => MyRouting
                   .Configuration()
                    .ShouldMap(request => request
                        .WithPath("/Questions/MyQuestions")
                        .WithUser()
                        .WithAntiForgeryToken())
                    .To<QuestionsController>(c => c.MyQuestions())
                    .Which(controller => controller.WithData(GetQuestions()))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                                   .RestrictingForAuthorizedRequests())
                    .AndAlso()
                    .ShouldReturn()
                    .View(view => view
                        .WithModelOfType<List<QuestionListingServiceModel>>()
                         .Passing(model => model.Should().HaveCount(5)));


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request.WithPath($"/Questions/Add/{LoadTestId}")
                      .WithUser()
                      .WithAntiForgeryToken())
                    .To<QuestionsController>(c => c.Add(LoadTestId))
                    .Which(controller => controller
                          .WithData(GetLoad()))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                           .RestrictingForAuthorizedRequests())
                     .AndAlso()
                     .ShouldReturn()
                      .View();

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnBadRequestWhenLoadIsNotPublic()
           => MyPipeline
                 .Configuration()
                  .ShouldMap(request => request.WithPath($"/Questions/Add/{LoadTestId}")
                     .WithUser())
                   .To<QuestionsController>(c => c.Add(LoadTestId))
                   .Which(controller => controller
                         .WithData(GetLoad(IsPublic: false)))
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                    .AndAlso()
                    .ShouldReturn()
                     .BadRequest();


        [Theory]
        [InlineData("MyTestContent", LoadTestId)]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectToWithCorrectDataAndModel(
            string content,
            string loadId)
            => MyRouting
                .Configuration()
                 .ShouldMap(request => request
                        .WithPath($"/Questions/Add/{loadId}")
                       .WithMethod(HttpMethod.Post)
                       .WithFormFields(new
                       {
                           Content = content
                       })
                       .WithUser()
                       .WithAntiForgeryToken())
                       .To<QuestionsController>(c => c.Add(loadId, new QuestionFormModel
                       {
                           Content = content
                       }))
                       .Which(controller => controller.WithData(GetLoad()))
                       .ShouldHave()
                        .ActionAttributes(attributes => attributes
                              .RestrictingForAuthorizedRequests()
                              .RestrictingForHttpMethod(HttpMethod.Post))
                        .ValidModelState()
                         .TempData(tempData => tempData
                               .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                             .Data(data => data.WithSet<Question>(set => set
                                     .Any(x => x.Content == content &&
                                               x.LoadId == loadId &&
                                               x.UserId == TestUser.Identifier)))
                              .AndAlso()
                               .ShouldReturn()
                                 .Redirect(redirect =>
                                           redirect
                                           .To<LoadsController>(c =>
                                           c.Details(loadId, With.Any<LoadsDetailsQueryModel>())));
        [Fact]
        public void PostAddShouldBeForAuthorizedUsersAndReturnBadRequestWhenLoadIsNotPublic()
           => MyRouting
               .Configuration()
                .ShouldMap(request => request
                       .WithPath($"/Questions/Add/{LoadTestId}")
                      .WithMethod(HttpMethod.Post)
                      .WithFormFields(new
                      {
                          Content = "TestContent"
                      })
                      .WithUser()
                      .WithAntiForgeryToken())
                      .To<QuestionsController>(c => c.Add(LoadTestId, new QuestionFormModel
                      {
                          Content = "TestContent"
                      }))
                      .Which(controller => controller.WithData(GetLoad(IsPublic: false)))
                      .ShouldHave()
                       .ActionAttributes(attributes => attributes
                             .RestrictingForAuthorizedRequests()
                             .RestrictingForHttpMethod(HttpMethod.Post))
                     .AndAlso()
                     .ShouldReturn()
                      .BadRequest();


        [Theory]
        [InlineData(1)]
        public void DeleteShouldDeleteQuestionAndRedirectToMyQuestions(
            int questionId)
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request.WithPath($"/Questions/Delete/{questionId}")
                 .WithUser()
                 .WithAntiForgeryToken())
                 .To<QuestionsController>(c => c.Delete(questionId))
                 .Which(controller => controller.WithData(GetQuestions(1)))
                 .ShouldHave()
                  .ActionAttributes(attributes => attributes
                              .RestrictingForAuthorizedRequests())
                    .Data(data => data.WithSet<Question>(set =>
                    {
                        set.FirstOrDefault(x => x.Id == questionId).Should().BeNull();
                        set.Should().BeEmpty();
                    }))
                    .TempData(tempData => tempData
                             .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                    .AndAlso()
                    .ShouldReturn()
                     .Redirect(redirect => redirect
                               .To<QuestionsController>(c => c.MyQuestions()));


        [Fact]
        public void DeleteShouldReturnBadRequestWhenQuestionIsNotOfUser()
     => MyPipeline
          .Configuration()
          .ShouldMap(request => request.WithPath($"/Questions/Delete/{2}")
          .WithUser())
          .To<QuestionsController>(c => c.Delete(2))
          .Which(controller => controller.WithData(GetQuestions(1)))
          .ShouldHave()
           .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
             .BadRequest();


        [Fact]
        public void DeleteShouldReturnNotFoundWhenQuestionDoesNotExist()
   => MyPipeline
        .Configuration()
        .ShouldMap(request => request.WithPath($"/Questions/Delete/{2}")
        .WithUser(new[] { AdminConstants.AdministratorRoleName }))
        .To<QuestionsController>(c => c.Delete(2))
        .Which(controller => controller.WithData(GetQuestions(1)))
        .ShouldHave()
         .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .NotFound();

    }
}
