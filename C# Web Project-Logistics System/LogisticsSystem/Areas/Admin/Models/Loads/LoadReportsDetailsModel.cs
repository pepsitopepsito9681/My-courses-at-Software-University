using LogisticsSystem.Services.Reports.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Areas.Admin.Models.Loads
{
    public class LoadReportsDetailsModel
    {
        public LoadModel Load { get; set; }

        public IEnumerable<ReportServiceModel> Reports { get; set; }
    }
}
