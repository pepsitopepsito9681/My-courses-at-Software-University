using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;

namespace LogisticsSystem.Test.Data
{
    public class Dealers
    {
        public const string TelephoneNumber = "0888888888";

        public static Trader GetDealer(string telephoneNumber = TelephoneNumber, bool sameUser = true)
        {
            var user = new User
            {
                FullName = TestUser.Identifier,
                UserName = TestUser.Identifier
            };

            var dealer = new Trader()
            {
                User = sameUser ? user : new User
                {
                    Id = "DifferentId",
                    UserName = "DifferentName"
                },
                UserId = sameUser ? TestUser.Identifier : "DifferentId",
                TelephoneNumber = telephoneNumber

            };

            return dealer;
        }
    }
}
