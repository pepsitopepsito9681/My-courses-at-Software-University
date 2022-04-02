using System.Collections.Generic;

namespace LogisticsSystem.Services.Reports.Models
{
    public class ReportQueryModel
    {
        public int CurrentPage { get; init; }

        public int ReportsPerPage { get; init; }

        public int TotalReports { get; init; }

        public IEnumerable<ReportServiceModel> Reports { get; init; }
    }
}
