using LogisticsSystem.Infrastructure;
using LogisticsSystem.Services.DeliveryCarts;
using LogisticsSystem.Services.Orders;
using LogisticsSystem.Services.Orders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static LogisticsSystem.WebConstants;

namespace LogisticsSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IDeliveryCartService deliveryCarts;
        private readonly IOrdersService orders;

        public OrdersController(IDeliveryCartService deliveryCarts, IOrdersService orders)
        {
            this.deliveryCarts = deliveryCarts;
            this.orders = orders;
        }

        [Authorize]
        public IActionResult Add()
        {
            var deliveryCartErrorsMessages = this.deliveryCarts
                .ValidateDeliveryCartOfUser(this.User.Id());

            if (deliveryCartErrorsMessages.Any())
            {
                this.TempData[GlobalErrorMessageKey] = deliveryCartErrorsMessages;

                return RedirectToAction(nameof(DeliveryCartController.MyDeliveryCart), "DeliveryCart");
            }

            var orderFormModel = this.orders.GetOrderAddFormModel(this.User.Id());

            return View(orderFormModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Add(OrderFormServiceModel order)
        {
            var deliveryCartErrorsMessages = this.deliveryCarts
                .ValidateDeliveryCartOfUser(this.User.Id());

            if (deliveryCartErrorsMessages.Any())
            {
                this.TempData[GlobalErrorMessageKey] = deliveryCartErrorsMessages;

                return RedirectToAction(nameof(DeliveryCartController.MyDeliveryCart), "DeliveryCart");
            }

            if (!ModelState.IsValid)
            {
                return View(order);
            }

            this.orders.Add(
                order.FullName,
                order.TelephoneNumber,
                order.State,
                order.City,
                order.Address,
                order.PostCode,
                this.User.Id());

            this.TempData[GlobalMessageKey] = "Your order was created succesfully and it is awaiting for accomplishment!";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
