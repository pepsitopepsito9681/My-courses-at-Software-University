using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LogisticsSystem.Areas.Identity.IdentityHostingStartup))]
namespace LogisticsSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
