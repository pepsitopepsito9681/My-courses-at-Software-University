using LogisticsSystem.Data;
using System.Linq;

namespace LogisticsSystem.Services.Users
{
    public class UsersService:IUsersService
    {
        private readonly LogisticsSystemDbContext data;

        public UsersService(LogisticsSystemDbContext data)
        {
            this.data = data;
        }

        public string GetFullName(string userId)
        => this.data.Users
            .Where(x => x.Id == userId)
            .Select(x => x.FullName)
            .FirstOrDefault();
    }
}
