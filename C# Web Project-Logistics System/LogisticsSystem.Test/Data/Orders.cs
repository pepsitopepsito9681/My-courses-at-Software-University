using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static LogisticsSystem.Test.Data.DeliveryCartItems;

namespace LogisticsSystem.Test.Data
{
    public static class Orders
    {
        public static Order GetOrder(
             bool accomplished = false,
            bool sameUser = true,
            int cartItemsCount = 5,
            byte quantityPerItem = 1,
            byte quantityPerLoad = 5,
            int orderId = 1)

        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            var order = new Order
            {
                Id = orderId,
                FullName = TestUser.Username,
                State = $"Sofia {orderId}",
                City = $"Sofia {orderId}",
                Address = $"My Address {orderId}",
                OrderedOn = new DateTime(1, 1, 1),
                IsAccomplished = accomplished,
                User = sameUser ? user : new User
                {
                    Id = $"Author Id {orderId}",
                    UserName = $"Author {orderId}"
                },
                DeliveryCart = GetDeliveryCartItems(cartItemsCount, quantityPerItem, quantityPerLoad)
                                           .ToList()
            };

            return order;

        }




        public static List<Order> GetOrders(
            int count = 5,
            bool accomplished = false,
            bool sameUser = true)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };


            var orders = Enumerable
             .Range(1, count)
             .Select(i => new Order
             {
                 Id = i,
                 FullName = TestUser.Username,
                 State = $"Sofia {i}",
                 City = $"Sofia {i}",
                 Address = $"My Address {i}",
                 OrderedOn = new DateTime(1, 1, 1),
                 IsAccomplished = accomplished,
                 User = sameUser ? user : new User
                 {
                     Id = $"Author Id {i}",
                     UserName = $"Author {i}"
                 },


             })
             .ToList();

            return orders;
        }
    }
}
