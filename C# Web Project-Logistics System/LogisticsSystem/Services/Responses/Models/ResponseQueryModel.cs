using System.Collections.Generic;

namespace LogisticsSystem.Services.Responses.Models
{
    public class ResponseQueryModel
    {
        public int CurrentPage { get; init; }

        public int ResponsesPerPage { get; init; }

        public int TotalResponses { get; init; }

        public IEnumerable<ResponseServiceModel> Responses { get; init; }
    }
}
