using LogisticsSystem.Services.Favourites.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Services.Favourites
{
    public interface IFavouritesService
    {
        void Add(string productId, string userId);

        bool Delete(int id);

        bool IsFavouriteExists(string loadId, string userId);

        bool IsFavouriteByUser(int id, string userId);

        IEnumerable<FavouriteServiceModel> MyFavourites(string userId);
    }
}
