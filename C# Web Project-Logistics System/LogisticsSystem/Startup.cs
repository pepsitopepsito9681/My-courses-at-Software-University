using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Infrastructure;
using LogisticsSystem.Services.Comments;
using LogisticsSystem.Services.DeliveryCarts;
using LogisticsSystem.Services.Favourites;
using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Orders;
using LogisticsSystem.Services.Questions;
using LogisticsSystem.Services.Reports;
using LogisticsSystem.Services.Responses;
using LogisticsSystem.Services.Reviews;
using LogisticsSystem.Services.Statistics;
using LogisticsSystem.Services.Traders;
using LogisticsSystem.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogisticsSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<LogisticsSystemDbContext>(options => options
                  .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LogisticsSystemDbContext>();

            services
                .AddControllersWithViews(options =>
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddAutoMapper(typeof(Startup));
            services.AddMemoryCache();

            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ILoadsService, LoadsService>();
            services.AddTransient<ITradersService, TradersService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IResponsesService, ResponsesService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IFavouritesService, FavouritesService>();
            services.AddTransient<IDeliveryCartService, DeliveryCartService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IUsersService, UsersService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultAreaRoute();

                    endpoints.MapReviewsDetailsRoute();
                    endpoints.MapQuestionsDetailsRoute();

                    endpoints.MapResponsesAddRoute();
                    endpoints.MapCommentsAddRoute();

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
