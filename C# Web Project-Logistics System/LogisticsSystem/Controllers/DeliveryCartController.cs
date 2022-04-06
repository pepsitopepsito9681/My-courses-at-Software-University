using LogisticsSystem.Infrastructure;
using LogisticsSystem.Services.DeliveryCarts;
using LogisticsSystem.Services.DeliveryCarts.Models;
using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Traders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LogisticsSystem.Areas.Admin.AdminConstants;

namespace LogisticsSystem.Controllers
{
    public class DeliveryCartController : Controller
    {
        private readonly ILoadsService loads;
        private readonly ITradersService traders;
        private readonly IDeliveryCartService deliveryCarts;

        public DeliveryCartController(ILoadsService loads, IDeliveryCartService deliveryCarts, ITradersService traders)
        {
            this.loads = loads;
            this.deliveryCarts = deliveryCarts;
            this.traders = traders;
        }

        [Authorize]
        public IActionResult Add(string id)
        {
            var load = this.loads.Details(id);

            if (load == null)
            {
                return NotFound();
            }

            string traderId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (traderId != null &&
                !isUserAdmin &&
                this.loads.LoadIsByTrader(id, traderId))
            {
                return BadRequest();
            }


            if (load.Quantity == 0 || this.deliveryCarts.ItemExists(id, this.User.Id()))
            {
                return BadRequest();
            }


            this.deliveryCarts.Add(id, this.User.Id());


            this.TempData[WebConstants.GlobalMessageKey] = "Load was added succesfully to Delivery Cart!";

            return RedirectToAction(nameof(LoadsController.Details), "Loads", new { id });


        }

        [Authorize]
        public IActionResult MyDeliveryCart()
        {

            var cartItems = this.deliveryCarts.MyDeliveryCart(this.User.Id());

            return View(cartItems);

        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!this.deliveryCarts.ItemIsByUser(id, this.User.Id()) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

            var cartItemQuantityLoadQuantityModel = this.deliveryCarts.GetQuantityAndLoadQuantity(id);

            if (cartItemQuantityLoadQuantityModel == null)
            {
                return NotFound();
            }


            return View(cartItemQuantityLoadQuantityModel);

        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, CartItemServiceModel cartItemEdit)
        {
            var isUserAdmin = this.User.IsAdmin();

            if (!this.deliveryCarts.ItemIsByUser(id, this.User.Id()) && !isUserAdmin)
            {
                return BadRequest();
            }

            var IsEdited = this.deliveryCarts.Edit(id, cartItemEdit.Quantity);

            if (!IsEdited)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Cart item quantity was edited succesfully!";

            if (isUserAdmin)
            {
                return RedirectToAction(nameof(Areas.Admin.Controllers.OrdersController.UnAccomplished), "Orders", new { area = AreaName });
            }


            return RedirectToAction(nameof(MyDeliveryCart));
        }



        [Authorize]
        public IActionResult Delete(int id)
        {
            var isUserAdmin = this.User.IsAdmin();


            if (!this.deliveryCarts.ItemIsByUser(id, this.User.Id()) && !isUserAdmin)
            {
                return BadRequest();
            }

            var IsDeleted = this.deliveryCarts.Delete(id);

            if (!IsDeleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Load was deleted succesfully from Delivery Cart!";

            if (isUserAdmin)
            {
                return RedirectToAction(nameof(Areas.Admin.Controllers.OrdersController.UnAccomplished), "Orders", new { area = AreaName });
            }

            return RedirectToAction(nameof(MyDeliveryCart));

        }
    }
}
