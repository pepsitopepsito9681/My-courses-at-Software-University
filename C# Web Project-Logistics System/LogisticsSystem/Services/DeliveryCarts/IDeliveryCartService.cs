using LogisticsSystem.Services.DeliveryCarts.Models;
using System.Collections.Generic;

namespace Logistics_System.Services.DeliveryCarts
{
    public interface IDeliveryCartService
    {
        void Add(
            string loadId,
            string userId);

        bool Delete(int id);

        bool Edit(
            int id,
            byte Quantity);

        bool ItemIsByUser(int id, string userId);

        bool ItemExists(string loadId, string userId);

        DeliveryCartItemServiceModel Details(int id);

        CartItemServiceModel GetQuantityAndLoadQuantity(int id);

        IEnumerable<DeliveryCartItemServiceModel> MyDeliveryCart(string userId);

        bool UserHasAnyUnOrderedDeliveryCartItem(string userId);

        IEnumerable<string> GetInformationAboutInvalidDeliveryCartItemsOfUser(string userId);

        IEnumerable<string> ValidateDeliveryCartOfUser(string userId);

        IEnumerable<DeliveryCartItemServiceModel> GetCartItemsbyOrder(int orderId);
    }
}
