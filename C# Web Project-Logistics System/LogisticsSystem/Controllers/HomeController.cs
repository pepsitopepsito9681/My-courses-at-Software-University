using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Loads.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using static LogisticsSystem.WebConstants.Cache;

namespace LogisticsSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoadsService loads;
        private readonly IMemoryCache cache;

        public HomeController(ILoadsService loads, IMemoryCache cache)
        {
            this.loads = loads;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            var latestLoads = this.cache.Get<List<LoadServiceModel>>(LatestLoadsCacheKey);

            if (latestLoads == null)
            {
                var loads = this.loads
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                latestLoads = this.cache.Set(LatestLoadsCacheKey, loads, cacheOptions);
            }

            return this.View(latestLoads);
        }

        public IActionResult Error() => View();
    }
}
