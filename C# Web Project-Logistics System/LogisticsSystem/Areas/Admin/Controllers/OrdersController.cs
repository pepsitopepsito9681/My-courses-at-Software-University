using LogisticsSystem.Areas.Admin.Models.Orders;
using LogisticsSystem.Services.DeliveryCarts;
using LogisticsSystem.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Areas.Admin.Controllers
{
    public class OrdersController : AdminController
    {
        private readonly IOrdersService orders;
        private readonly IDeliveryCartService deliveryCarts;

        public OrdersController(IOrdersService orders, IDeliveryCartService deliveryCarts)
        {
            this.orders = orders;
            this.deliveryCarts = deliveryCarts;
        }

        public IActionResult UnAccomplished([FromQuery] OrdersQueryModel query)
        {
            var queryResult = this.orders.All(
                      query.SearchTerm,
                      query.CurrentPage,
                      OrdersQueryModel.OrdersPerPage);

            query.Orders = queryResult.Orders;
            query.TotalOrders = queryResult.TotalOrders;

            return this.View(query);
        }

        public IActionResult Accomplished([FromQuery] OrdersQueryModel query)
        {
            var queryResult = this.orders.All(
            query.SearchTerm,
            query.CurrentPage,
            OrdersQueryModel.OrdersPerPage,
            IsAccomplished: true);

            query.Orders = queryResult.Orders;
            query.TotalOrders = queryResult.TotalOrders;

            return this.View(query);
        }

        public IActionResult Details(int id)
        {
            var order = this.orders.Details(id);

            var orderItems = this.deliveryCarts.GetCartItemsbyOrder(id);

            return this.View(new OrderDetailsModel
            {
                Order = order,
                OrderItems = orderItems
            });
        }

        public IActionResult Delete(int id)
        {
            if (!this.orders.OrderExists(id))
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id, OrderDeleteFormModel order)
        {
            if (!order.ConfirmDeletion)
            {
                return RedirectToAction(nameof(UnAccomplished));
            }

            var IsDeleted = this.orders.Delete(id);

            if (!IsDeleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Order was deleted succesfully!";

            return RedirectToAction(nameof(UnAccomplished));

        }

        public IActionResult Accomplish(int id)
        {
            var IsAccomplished = this.orders.Accomplish(id);

            if (!IsAccomplished)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Order was accomplished succesfully!";

            return RedirectToAction(nameof(Accomplished));
        }

        public IActionResult Cancel(int id)
        {
            var IsCanceled = this.orders.Cancel(id);

            if (!IsCanceled)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Order was canceled and deleted succesfully!";

            return RedirectToAction(nameof(UnAccomplished));
        }
    }
}
