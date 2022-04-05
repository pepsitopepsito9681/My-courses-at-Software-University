using LogisticsSystem.Infrastructure;
using LogisticsSystem.Models.Reports;
using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ILoadsService loads;
        private readonly IReportsService reports;

        public ReportsController(ILoadsService loads, IReportsService reports)
        {
            this.loads = loads;
            this.reports = reports;
        }

        [Authorize]
        public IActionResult Add(string id)
        {
            if (!this.loads.IsLoadPublic(id))
            {
                return NotFound();
            }

            if (this.reports.ReportExistsForLoad(id, this.User.Id()))
            {
                return BadRequest();
            }

            return View();

        }


        [Authorize]
        [HttpPost]
        public IActionResult Add(string id, ReportFormModel report)
        {
            if (!this.loads.IsLoadPublic(id))
            {
                return NotFound();
            }

            if (this.reports.ReportExistsForLoad(id, this.User.Id()))
            {
                return BadRequest();
            }

            if (!(ModelState.IsValid))
            {
                return this.View(report);

            }

            this.reports.Add(
                report.Content,
                report.ReportKind.Value,
                id,
                this.User.Id());

            this.TempData[WebConstants.GlobalMessageKey] = $"Report was added and send to administration!";

            return RedirectToAction(nameof(LoadsController.Details), "Loads", new { id });
        }
    }
}
