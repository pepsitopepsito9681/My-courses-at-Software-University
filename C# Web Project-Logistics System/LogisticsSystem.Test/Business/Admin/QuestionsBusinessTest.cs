using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Areas.Admin.Models.Questions;
using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Questions;
using QuestionsController = LogisticsSystem.Areas.Admin.Controllers.QuestionsController;

namespace LegoM.Test.Business.Admin
{

    public class QuestionsBusinessTest
    {
        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
           => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                   .WithPath("/Admin/Questions/All")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName })
                    .WithAntiForgeryToken())
               .To<QuestionsController>(c => c.All(With.Default<QuestionsQueryModel>()))
               .Which(controller => controller
                   .WithData(GetQuestions()))
               .ShouldReturn()
               .View(view => view
                  .WithModelOfType<QuestionsQueryModel>()
                .Passing(model => model.Questions.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeQuestionAndRedirectToAll()
          => MyPipeline
                .Configuration()
                 .ShouldMap(request => request
                  .WithPath($"/Admin/Questions/ChangeVisibility/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
                 .To<QuestionsController>(c => c.ChangeVisibility(1))
                 .Which(controller => controller
                  .WithData(GetQuestions(1)))
                 .ShouldHave()
                  .Data(data => data
                       .WithSet<Question>(set =>
                       {
                           var question = set.FirstOrDefault(r => !r.IsPublic);
                           question.Should().NotBeNull();
                           question.Content.Should().NotBeNull();
                           question.Content.Should().Be($"Question Content {1}");
                       }
                       ))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                      .To<QuestionsController>(c => c.All(With.Any<QuestionsQueryModel>())));

    }
}
