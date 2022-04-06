using AutoMapper;
using LogisticsSystem.Areas.Admin.Models.Loads;
using LogisticsSystem.Models.Loads;
using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Reports;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Areas.Admin.Controllers
{
    public class LoadsController : AdminController
    {
        private readonly ILoadsService loads;
        private readonly IReportsService reports;
        private readonly IMapper mapper;

        public LoadsController(ILoadsService loads, IMapper mapper, IReportsService reports)
        {
            this.loads = loads;
            this.mapper = mapper;
            this.reports = reports;
        }

        public IActionResult Existing([FromQuery] LoadsQueryModel query)
        {
            ;
            if (!this.loads.SubKindIsValid(query.SubKind, query.Kind))
            {
                return BadRequest();
            }

            var queryResult = this.loads.All(
            query.Kind,
            query.SubKind,
            query.SearchTerm,
            query.CurrentPage,
            LoadsQueryModel.LoadsPerPage,
            query.LoadSorting,
            IsPublicOnly: false);


            var kinds = this.loads.AllKinds();
            var subKinds = this.loads.AllSubKinds();

            query.Loads = queryResult.Loads;
            query.Kinds = kinds;
            query.SubKinds = subKinds;
            query.TotalLoads = queryResult.TotalLoads;

            return this.View(query);

        }

        public IActionResult Deleted([FromQuery] LoadsQueryModel query)
        {
            if (!this.loads.SubKindIsValid(query.SubKind, query.Kind))
            {
                return BadRequest();
            }

            var queryResult = this.loads.All(
            query.Kind,
            query.SubKind,
            query.SearchTerm,
            query.CurrentPage,
            LoadsQueryModel.LoadsPerPage,
            query.LoadSorting,
            IsPublicOnly: false,
            IsDeleted: true);

            var kinds = this.loads.AllKinds();
            var subKinds = this.loads.AllSubKinds();

            query.Loads = queryResult.Loads;
            query.Kinds = kinds;
            query.SubKinds = subKinds;
            query.TotalLoads = queryResult.TotalLoads;

            return this.View(query);
        }

        public IActionResult ChangeVisibility(string id)
        {
            this.loads.ChangeVisibility(id);

            return RedirectToAction(nameof(this.Existing));
        }

        public IActionResult Revive(string id)
        {
            this.loads.ReviveLoad(id);

            return RedirectToAction(nameof(this.Existing));
        }

        public IActionResult Reports(string id)
        {
            var load = this.loads.Details(id);

            if (load == null)
            {
                return NotFound();
            }

            var loadModel = this.mapper.Map<LoadModel>(load);

            var reports = this.reports.All(loadId: id).Reports;

            return View(new LoadReportsDetailsModel
            {
                Load = loadModel,
                Reports = reports
            });
        }
    }
}
