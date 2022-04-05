using LogisticsSystem.Infrastructure;
using LogisticsSystem.Models.Responses;
using LogisticsSystem.Services.Questions;
using LogisticsSystem.Services.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Controllers
{
    public class ResponsesController : Controller
    {
        private readonly IResponsesService responses;
        private readonly IQuestionsService questions;

        public ResponsesController(IQuestionsService questions, IResponsesService responses)
        {
            this.questions = questions;
            this.responses = responses;
        }

        [Authorize]
        public IActionResult Add(int id, string information)
        {
            var questionModel = this.questions.QuestionById(id);

            if (questionModel == null || questionModel.GetInformation() != information || !(questionModel.IsPublic))
            {
                return NotFound();
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(int id, string information, ResponseFormModel response)
        {
            var questionModel = this.questions.QuestionById(id);

            if (questionModel == null || questionModel.GetInformation() != information || !(questionModel.IsPublic))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return this.View(response);
            }

            var IsUserAdmin = this.User.IsAdmin();

            this.responses.Create(
                id,
                this.User.Id(),
                response.Content,
               IsUserAdmin);

            this.TempData[WebConstants.GlobalMessageKey] = $"Your response was added { (IsUserAdmin ? string.Empty : "and is awaiting for approval!") }";

            return RedirectToAction("Details", "Questions", new { id = questionModel.Id, information = questionModel.GetInformation() });
        }
    }
}
