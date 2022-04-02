using LogisticsSystem.Services.Responses.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Areas.Admin.Models.Responses
{
    public class ResponsesQueryModel
    {
        public const int ResponsesPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int TotalResponses { get; set; }

        public IEnumerable<ResponseServiceModel> Responses { get; set; }
    }
}
