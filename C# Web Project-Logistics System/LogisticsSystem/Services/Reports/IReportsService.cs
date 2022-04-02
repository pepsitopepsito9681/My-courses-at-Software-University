using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Reports.Models;

namespace LogisticsSystem.Services.Reports
{
    public interface IReportsService
    {
        void Add(string content,
            ReportKind reportKind,
            string loadId,
            string userId);

        ReportQueryModel All(
           string searchTerm = null,
           int currentPage = 1,
           int reportsPerPage = int.MaxValue,
           string loadId = null);

        bool Delete(int id);


        bool ReportExistsForLoad(string loadId, string userId);
    }
}
