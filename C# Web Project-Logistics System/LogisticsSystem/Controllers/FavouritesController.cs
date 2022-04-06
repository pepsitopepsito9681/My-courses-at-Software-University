using LogisticsSystem.Infrastructure;
using LogisticsSystem.Services.Favourites;
using LogisticsSystem.Services.Loads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Controllers
{
    public class FavouritesController : Controller
    {
        private readonly IFavouritesService favourites;
        private readonly ILoadsService loads;

        public FavouritesController(IFavouritesService favourites, ILoadsService loads)
        {
            this.favourites = favourites;
            this.loads = loads;
        }
        /*
        [Authorize]
        public IActionResult Add(string id)
        {
            if (!this.loads.LoadExists(id))
            {
                return NotFound();
            }
            var userId = this.User.Id();

            if (this.User.IsAdmin() || this.favourites.IsFavouriteExists(id, userId))
            {
                return BadRequest();
            }

            this.favourites.Add(id, userId);

            this.TempData[WebConstants.GlobalMessageKey] = "Load was added succesfully to favourites!";

            return RedirectToAction(nameof(LoadsController.Details), "Loads", new { id });
        }
        */
        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.Id();

            if (this.User.IsAdmin() || !this.favourites.IsFavouriteByUser(id, userId))
            {
                return BadRequest();
            }

            this.favourites.Delete(id);

            this.TempData[WebConstants.GlobalMessageKey] = "Load was deleted succesfully from favourites!";


            return RedirectToAction(nameof(MyFavourites));
        }

        [Authorize]
        public IActionResult MyFavourites()
        {
            var favourites = this.favourites.MyFavourites(this.User.Id());

            return View(favourites);
        }
    }
}
