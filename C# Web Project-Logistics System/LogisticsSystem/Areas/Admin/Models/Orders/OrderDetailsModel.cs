using LogisticsSystem.Services.DeliveryCarts.Models;
using LogisticsSystem.Services.Orders.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Areas.Admin.Models.Orders
{
    public class OrderDetailsModel
    {
        public OrderDetailsServiceModel Order { get; set; }

        public IEnumerable<DeliveryCartItemServiceModel> OrderItems { get; set; }
    }
}
