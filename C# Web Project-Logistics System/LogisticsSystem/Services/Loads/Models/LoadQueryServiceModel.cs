using System.Collections.Generic;

namespace LogisticsSystem.Services.Loads.Models
{
    public class LoadQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int LoadsPerPage { get; init; }

        public int TotalLoads { get; init; }

        public IEnumerable<LoadServiceModel> Loads { get; init; }
    }
}
