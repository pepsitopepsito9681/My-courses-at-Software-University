using LogisticsSystem.Infrastructure;
using LogisticsSystem.Models.Traders;
using LogisticsSystem.Services.Traders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LogisticsSystem.WebConstants;

namespace LogisticsSystem.Controllers
{
    public class TradersController : Controller
    {
        private readonly ITradersService traders;

        public TradersController(ITradersService traders)
        => this.traders = traders;


        [Authorize]
        public IActionResult Become()
        {
            if (this.traders.IsUserTrader(this.User.Id()))
            {
                return BadRequest();
            }

            return View();
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeTraderFormModel dealer)
        {
            var userId = this.User.Id();

            if (this.traders.IsUserTrader(userId))
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(dealer);
            }

            this.traders.Create(
                 userId,
                 dealer.Name,
                 dealer.TelephoneNumber);


            TempData[GlobalMessageKey] = "Thank you for becoming a Trader.";

            return RedirectToAction(nameof(LoadsController.All), "Loads");
        }
        
    }
}
