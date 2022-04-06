using LogisticsSystem.Areas.Admin.Models.Responses;
using LogisticsSystem.Services.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Areas.Admin.Controllers
{
    public class ResponsesController:AdminController
    {
        private readonly IResponsesService responses;

        public ResponsesController(IResponsesService responses)
        {
            this.responses = responses;
        }

        public IActionResult All([FromQuery] ResponsesQueryModel query)
        {
            var queryResult = this.responses.All(
            query.SearchTerm,
            query.CurrentPage,
            ResponsesQueryModel.ResponsesPerPage,
            IsPublicOnly: false);

            query.Responses = queryResult.Responses;
            query.TotalResponses = queryResult.TotalResponses;

            return this.View(query);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.responses.ChangeVisibility(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            var deleted = this.responses.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Response was deleted successfully!";

            return RedirectToAction(nameof(All));
        }
    }
}
