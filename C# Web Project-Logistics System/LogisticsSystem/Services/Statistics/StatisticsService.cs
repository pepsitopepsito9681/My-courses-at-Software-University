using LogisticsSystem.Data;
using LogisticsSystem.Services.Statistics.Models;
using System.Linq;

namespace LogisticsSystem.Services.Statistics
{
    public class StatisticsService:IStatisticsService
    {
        private readonly LogisticsSystemDbContext data;

        public StatisticsService(LogisticsSystemDbContext data)
        => this.data = data;


        public StatisticsServiceModel Total()
        {
            var totalLoads = this.data.Loads.Count(c => c.IsPublic);
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalLoads = totalLoads,
                TotalUsers = totalUsers

            };
        }
    }
}
