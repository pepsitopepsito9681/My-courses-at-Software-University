using LogisticsSystem.Data.Models.Enums;

namespace LogisticsSystem.Models.Api.Loads
{
    public class AllLoadsApiRequestModel
    {
        public int CurrentPage { get; set; } = 1;

        public string Kind { get; init; }

        public string SubKind { get; init; }

        public string SearchTerm { get; init; }

        public int LoadsPerPage { get; set; } = 10;

        public LoadSorting LoadSorting { get; init; }
    }
}
