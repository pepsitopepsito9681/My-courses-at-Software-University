using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Reports.Models;
using System;
using System.Linq;

namespace LogisticsSystem.Services.Reports
{
    public class ReportsService:IReportsService
    {
        private readonly LogisticsSystemDbContext data;

        private readonly IConfigurationProvider mapper;

        public ReportsService(LogisticsSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Add(string content,
            ReportKind reportKind,
            string loadId,
            string userId)
        {

            var report = new Report
            {
                Content = content,
                ReportKind = reportKind,
                LoadId = loadId,
                UserId = userId,
                PublishedOn = DateTime.UtcNow
            };

            this.data.Reports.Add(report);

            this.data.SaveChanges();

        }

        public ReportQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int reportsPerPage = int.MaxValue,
            string loadId = null)
        {
            var reportsQuery = this.data.Reports
               .AsQueryable();

            if (!string.IsNullOrEmpty(loadId))
            {

                reportsQuery = reportsQuery.Where(x => x.LoadId == loadId);

            }


            if (!string.IsNullOrEmpty(searchTerm))
            {

                reportsQuery = reportsQuery
                                         .Where(x =>
                                         x.Content.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.ReportKind.ToString().ToLower().Contains(searchTerm.ToLower()) ||
                                         x.Load.Title.ToLower().Contains(searchTerm.ToLower()));

            }

            var totalReports = reportsQuery.Count();

            var reports = reportsQuery
                  .Skip((currentPage - 1) * reportsPerPage)
                    .Take(reportsPerPage)
                    .OrderByDescending(x => x.PublishedOn)
                    .ProjectTo<ReportServiceModel>(mapper)
                    .ToList();

            return new ReportQueryModel
            {
                Reports = reports,
                CurrentPage = currentPage,
                TotalReports = totalReports,
                ReportsPerPage = reportsPerPage,
            };

        }

        public bool Delete(int id)
        {
            var report = this.data.Reports.Find(id);

            if (report == null)
            {
                return false;
            }

            this.data.Reports.Remove(report);

            this.data.SaveChanges();

            return true;

        }

        public bool ReportExistsForLoad(string loadId, string userId)
        => this.data.Reports.Any(x => x.LoadId == loadId && x.UserId == x.UserId);
    }
}
