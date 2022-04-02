using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Orders.Models;
using LogisticsSystem.Services.Traders;
using LogisticsSystem.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Services.Orders
{
    public class OrdersService:IOrdersService
    {
        private readonly LogisticsSystemDbContext data;
        private readonly IConfigurationProvider mapper;
        private readonly ITradersService traders;
        private readonly IUsersService users;

        public OrdersService(LogisticsSystemDbContext data, ITradersService traders, IUsersService users, IMapper mapper)
        {
            this.data = data;
            this.traders = traders;
            this.users = users;
            this.mapper = mapper.ConfigurationProvider;
        }

        public bool Accomplish(int id)
        {
            var order = this.data.Orders
                .Include(x => x.DeliveryCart)
                .ThenInclude(x => x.Load)
                .FirstOrDefault(x => x.Id == id);

            if (order == null || order.IsAccomplished)
            {
                return false;
            }

            foreach (var cartItem in order.DeliveryCart)
            {
                cartItem.Load.Quantity -= cartItem.Quantity;
                cartItem.UserId = null;
            }

            order.IsAccomplished = true;

            this.data.SaveChanges();

            return true;


        }

        public void Add(string fullName,
            string telephoneNumber,
            string state,
            string city,
            string address,
            string postCode,
            string userId)
        {
            var deliveryCart = this.data
                .DeliveryCarts
                .Where(x => x.UserId == userId && x.OrderId == null);


            var order = new Order
            {
                FullName = fullName,
                PhoneNumber = telephoneNumber,
                State = state,
                City = city,
                Address = address,
                PostCode = postCode,
                UserId = userId,
                DeliveryCart = deliveryCart.ToList(),
                OrderedOn = DateTime.UtcNow,
                IsAccomplished = false
            };


            this.data.Orders.Add(order);

            this.data.SaveChanges();

        }

        public OrderQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int ordersPerPage = int.MaxValue,
            bool IsAccomplished = false)
        {
            var ordersQuery = this.data.Orders
               .Where(x => x.IsAccomplished == IsAccomplished)
               .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {

                ordersQuery = ordersQuery
                                         .Where(x =>
                                         x.FullName.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.Address.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.City.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.State.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.Id.ToString().Contains(searchTerm) ||
                                         x.PhoneNumber.Contains(searchTerm));

            }


            var totalOrders = ordersQuery.Count();

            var orders = ordersQuery
                  .Skip((currentPage - 1) * ordersPerPage)
                    .Take(ordersPerPage)
                    .OrderByDescending(x => x.OrderedOn)
                    .ProjectTo<OrderServiceModel>(mapper)
                    .ToList();

            return new OrderQueryModel
            {
                Orders = orders,
                CurrentPage = currentPage,
                TotalOrders = totalOrders,
                OrdersPerPage = ordersPerPage,
            };

        }

        public bool Cancel(int id)
        {
            var order = this.data.Orders
               .Include(x => x.DeliveryCart)
               .ThenInclude(x => x.Load)
               .FirstOrDefault(x => x.Id == id);

            if (order == null || !(order.IsAccomplished))
            {
                return false;
            }

            foreach (var cartItem in order.DeliveryCart)
            {
                cartItem.Load.Quantity += cartItem.Quantity;

            }

            order.IsAccomplished = false;

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var order = this.data.Orders.Find(id);

            if (order == null)
            {
                return false;
            }

            this.data.Orders.Remove(order);

            this.data.SaveChanges();

            return true;
        }

        public OrderDetailsServiceModel Details(int id)
        => this.data.Orders.Where(x => x.Id == id)
            .ProjectTo<OrderDetailsServiceModel>(mapper)
            .FirstOrDefault();

        public OrderFormServiceModel GetOrderAddFormModel(string userId)
        {
            var userFullName = this.users.GetFullName(userId);

            var telephoneNumber = this.traders.TelephoneNumberByUser(userId);

            return new OrderFormServiceModel
            {
                FullName = userFullName,
                TelephoneNumber = telephoneNumber
            };

        }

        public IEnumerable<OrderServiceModel> GetOrders(bool IsAccomplished = false)
        => this.data.Orders
            .Where(x => x.IsAccomplished == IsAccomplished)
            .OrderBy(x => x.OrderedOn)
            .ProjectTo<OrderServiceModel>(mapper)
            .ToList();

        public bool OrderExists(int id)
        => this.data.Orders.Any(x => x.Id == id);
    }
}
