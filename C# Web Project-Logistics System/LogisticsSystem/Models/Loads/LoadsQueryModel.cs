using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Loads.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Models.Loads
{
    public class LoadsQueryModel
    {
        public const int LoadsPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        public string Kind { get; set; }

        public string SubKind { get; set; }

        public IEnumerable<LoadKindServiceModel> Kinds { get; set; }

        public IEnumerable<LoadSubKindServiceModel> SubKinds { get; set; }


        public string SearchTerm { get; init; }

        public LoadSorting LoadSorting { get; init; }

        public int TotalLoads { get; set; }

        public IEnumerable<LoadServiceModel> Loads { get; set; }
    }
}
