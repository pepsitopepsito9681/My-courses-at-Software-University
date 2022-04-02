using System.Collections.Generic;

namespace LogisticsSystem.Services.Orders.Models
{
    public class OrderQueryModel
    {
        public int CurrentPage { get; init; }

        public int OrdersPerPage { get; init; }

        public int TotalOrders { get; init; }

        public IEnumerable<OrderServiceModel> Orders { get; init; }
    }
}
