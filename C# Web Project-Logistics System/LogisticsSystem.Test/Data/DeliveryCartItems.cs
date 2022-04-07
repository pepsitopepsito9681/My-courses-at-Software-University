using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    

    public static class DeliveryCartItems
    {
        public static IEnumerable<DeliveryCartItem> GetDeliveryCartItems(int count = 5,
            byte cartQuantity=1,
            byte loadQuantity=5,
            bool sameUser = true)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };


            var cartItems = Enumerable
               .Range(1, count)
               .Select(i => new DeliveryCartItem
               {
                   Id = i,
                   Quantity=cartQuantity,
                   User = sameUser ? user : new User
                   {
                       Id = $"DeliveryCart Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Load = new Load
                   {
                       Title = "Test",
                       LoadCondition = LogisticsSystem.Data.Models.Enums.LoadCondition.New,
                       Quantity=loadQuantity
                   },
                   LoadId = "TestId"
               })
               .ToList();

            return cartItems;

        }
    }
}
