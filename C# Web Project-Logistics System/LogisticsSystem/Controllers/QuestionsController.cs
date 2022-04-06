using LogisticsSystem.Infrastructure;
using LogisticsSystem.Models.Questions;
using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Questions;
using LogisticsSystem.Services.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ILoadsService loads;
        private readonly IQuestionsService questions;
        private readonly IResponsesService responses;

        public QuestionsController(ILoadsService loads, IQuestionsService questions, IResponsesService responses)
        {
            this.loads = loads;
            this.questions = questions;
            this.responses = responses;
        }

        [Authorize]
        public IActionResult Add(string Id)
        {
            if (!this.loads.IsLoadPublic(Id))
            {
                return BadRequest();
            }

            return View();

        }
        
        [Authorize]
        [HttpPost]
        public IActionResult Add(string Id, QuestionFormModel question)
        {
            if (!this.loads.IsLoadPublic(Id))
            {
                return BadRequest();
            }

            if (!(ModelState.IsValid))
            {
                return this.View(question);

            }

            var IsUserAdmin = this.User.IsAdmin();

            this.questions.Create(
                Id,
                this.User.Id(),
                question.Content,
                IsUserAdmin);

            this.TempData[WebConstants.GlobalMessageKey] = $"Your question was added  { (IsUserAdmin ? string.Empty : "and is awaiting for approval!") }";

            return RedirectToAction(nameof(LoadsController.Details), "Loads", new { id = Id });

        }
        
        public IActionResult Details(int id, string information)
        {
            var question = this.questions.Details(id);

            if (question == null || question.GetInformation() != information)
            {
                return NotFound();
            }


            var questionResponses = this.responses.ResponsesOfQuestion(id);

            return this.View(new QuestionDetailsWithResponsesModel
            {
                Question = question,
                Responses = questionResponses

            });

        }

        [Authorize]
        public IActionResult MyQuestions()
        {
            var questions = this.questions.MyQuestions(this.User.Id());

            return this.View(questions);
        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!this.questions.QuestionIsByUser(id, this.User.Id()) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

            var isSuccesfullyDeleted = this.questions.Delete(id);

            if (!isSuccesfullyDeleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = $"Your question was deleted { (this.User.IsAdmin() ? string.Empty : "and is awaiting for approval!") } ";

            return RedirectToAction(nameof(MyQuestions));

        }
    }
}
