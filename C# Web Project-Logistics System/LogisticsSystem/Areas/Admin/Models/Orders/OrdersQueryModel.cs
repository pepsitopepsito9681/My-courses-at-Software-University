using LogisticsSystem.Services.Orders.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Areas.Admin.Models.Orders
{
    public class OrdersQueryModel
    {
        public const int OrdersPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int TotalOrders { get; set; }

        public IEnumerable<OrderServiceModel> Orders { get; set; }
    }
}
