using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Models.Responses;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Questions;

namespace LogisticsSystem.Test.Business
{

    public class ResponsesBusinessTest
    {
        public string Information = GetInformation();

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
         => MyPipeline
               .Configuration()
                .ShouldMap(request => request
                   .WithPath($"/Responses/Add/{1}/{Information}")
                .WithUser())
                .To<ResponsesController>(c => c.Add(1, Information))
               .Which(controller => controller.WithData(GetQuestions(1))
               .ShouldHave()
            .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View());


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnNotFoundWhenQuestionDoesNotExists()
         => MyPipeline
               .Configuration()
                .ShouldMap(request => request
                   .WithPath($"/Responses/Add/{1}/{Information}")
                .WithUser())
                .To<ResponsesController>(c => c.Add(1, Information))
               .Which()
               .ShouldHave()
            .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .NotFound();



        [Theory]
        [InlineData(1, "ContentTest")]
        [InlineData(2, "ContentTest2")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectToViewWithCorrectModel(
            int questionId,
            string content)
            => MyPipeline
               .Configuration()
                .ShouldMap(request => request.WithPath($"/Responses/Add/{questionId}/{Information}")
                   .WithUser()
                    .WithAntiForgeryToken()
                   .WithMethod(HttpMethod.Post)
                   .WithFormFields(new
                   {
                       Content = content,

                   }))
                  .To<ResponsesController>(c => c.Add(questionId, Information, new ResponseFormModel
                  {
                      Content = content
                  }))
                 .Which(controller => controller.WithData(GetQuestions(questionId)))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                   .AndAlso()
                    .ShouldHave()
                     .Data(data => data.WithSet<Response>(set =>
                                        set.Any(x => x.Content == content
                                        && x.QuestionId == questionId)))
                        .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                  .Redirect(redirect => redirect
                          .To<QuestionsController>(c => c
                          .Details(questionId, Information)));


        [Theory]

        [InlineData(2, "ContentTest2")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnNotFoundWhenQuestionDoesNotExists(
         int questionId,
         string content)
         => MyPipeline
            .Configuration()
             .ShouldMap(request => request.WithPath($"/Responses/Add/{questionId}/{Information}")
                .WithUser()
                 .WithAntiForgeryToken()
                .WithMethod(HttpMethod.Post)
                .WithFormFields(new
                {
                    Content = content,

                }))
               .To<ResponsesController>(c => c.Add(questionId, Information, new ResponseFormModel
               {
                   Content = content
               }))
              .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
               .AndAlso()
               .ShouldReturn()
               .NotFound();

    }
}
