using LogisticsSystem.Models.Api.Loads;
using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Loads.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Controllers.Api
{
    [ApiController]
    [Route("api/loads")]
    public class LoadsApiController:ControllerBase
    {
        private readonly ILoadsService loads;

        public LoadsApiController(ILoadsService loads)
        => this.loads = loads;

        [HttpGet]
        public ActionResult<LoadQueryServiceModel> All([FromQuery] AllLoadsApiRequestModel query)
        => this.loads.All(
             query.Kind,
             query.SubKind,
             query.SearchTerm,
             query.CurrentPage,
             query.LoadsPerPage,
             query.LoadSorting);
    }
}
