using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static LogisticsSystem.Test.Data.Loads;

namespace LogisticsSystem.Test.Data
{
    public static class Favourites
    {
        public static List<Favourite> GetFavourites(
            int count = 5,
            bool sameUser = true,
            bool sameLoad = true)
        {

            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };

            var load = new Load
            {
                Id = LoadTestId,
                LoadCondition = LogisticsSystem.Data.Models.Enums.LoadCondition.New
            };


            var favourites = Enumerable
             .Range(1, count)
             .Select(i => new Favourite
             {
                 Id = i,
                 User = sameUser ? user : new User
                 {
                     Id = $"Author Id {i}",
                     UserName = $"Author {i}"
                 },
                 Load = sameLoad ? load : new Load
                 {
                     LoadCondition = LogisticsSystem.Data.Models.Enums.LoadCondition.New
                 },
             })
             .ToList();


            return favourites;

        }
    }
}
