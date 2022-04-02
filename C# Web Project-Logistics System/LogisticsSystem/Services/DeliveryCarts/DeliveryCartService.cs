using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.DeliveryCarts.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Services.DeliveryCarts
{
    public class DeliveryCartService:IDeliveryCartService
    {
        private readonly LogisticsSystemDbContext data;

        private readonly IConfigurationProvider mapper;

        public DeliveryCartService(LogisticsSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }


        public void Add(string loadId, string userId)
        {
            var deliveryCartItem = new DeliveryCartItem
            {
                LoadId = loadId,
                UserId = userId,
                Quantity = 1
            };

            this.data.DeliveryCarts.Add(deliveryCartItem);

            this.data.SaveChanges();

        }

        public bool Delete(int id)
        {
            var item = this.data.DeliveryCarts.Find(id);

            if (item == null)
            {
                return false;
            }

            this.data.DeliveryCarts.Remove(item);

            this.data.SaveChanges();

            return true;
        }

        public DeliveryCartItemServiceModel Details(int id)
        => this.data.DeliveryCarts.Where(x => x.Id == id)
            .ProjectTo<DeliveryCartItemServiceModel>(mapper)
            .FirstOrDefault();

        public bool Edit(int id, byte Quantity)
        {
            var deliveryCartItem = this.data.DeliveryCarts.Include(x => x.Load).FirstOrDefault(x => x.Id == id);

            if (deliveryCartItem == null)
            {
                return false;
            }

            if (deliveryCartItem.Load.Quantity < Quantity || Quantity == 0)
            {
                return false;
            }

            deliveryCartItem.Quantity = Quantity;

            this.data.SaveChanges();

            return true;

        }



        public CartItemServiceModel GetQuantityAndLoadQuantity(int id)
        {
            var cartItem = this.Details(id);

            if (cartItem == null)
            {
                return null;
            }

            return new CartItemServiceModel
            {
                Quantity = cartItem.Quantity,
                LoadQuantity = cartItem.LoadQuantity
            };
        }

        public bool ItemExists(string loadId, string userId)
        => this.data.DeliveryCarts.Any(x => x.LoadId == loadId && x.UserId == userId);

        public bool ItemIsByUser(int id, string userId)
        => this.data.DeliveryCarts.Any(x => x.Id == id && x.UserId == userId);

        public IEnumerable<DeliveryCartItemServiceModel> MyDeliveryCart(string userId)
          => this.data.DeliveryCarts
            .Where(x => x.UserId == userId && x.OrderId == null)
            .ProjectTo<DeliveryCartItemServiceModel>(mapper)
            .ToList();

        public bool UserHasAnyUnOrderedDeliveryCartItem(string userId)
       => this.data.DeliveryCarts
            .Where(x => x.UserId == userId && x.OrderId == null)
            .Any();


        public IEnumerable<string> GetInformationAboutInvalidDeliveryCartItemsOfUser(string userId)
     => MyDeliveryCart(userId)
         .Where(x => x.Quantity > x.LoadQuantity ||
               x.Quantity == 0 ||
               x.LoadQuantity == 0)
        .Select(x => $"You should edit quantity or delete \" {x.LoadTitle} \" to continue to order!")
        .ToList();

        public IEnumerable<string> ValidateDeliveryCartOfUser(string userId)
        {
            var errorsList = new List<string>();

            var IsUserDeliveryCartContainsAnyCartItem = this
               .UserHasAnyUnOrderedDeliveryCartItem(userId);

            if (!IsUserDeliveryCartContainsAnyCartItem)
            {
                errorsList.Add("Your delivery cart is empty");

                return errorsList;
            }

            var cartItemsErrorMessages = this
                .GetInformationAboutInvalidDeliveryCartItemsOfUser(userId);

            if (cartItemsErrorMessages.Any())
            {
                errorsList.AddRange(cartItemsErrorMessages);

            }

            return errorsList;
        }

        public IEnumerable<DeliveryCartItemServiceModel> GetCartItemsbyOrder(int orderId)
        => this.data.DeliveryCarts
            .Where(x => x.OrderId.HasValue && x.OrderId == orderId)
            .ProjectTo<DeliveryCartItemServiceModel>(mapper)
            .ToList();
    }
}
