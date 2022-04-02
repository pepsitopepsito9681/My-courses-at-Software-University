using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Favourites.Models;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Services.Favourites
{
    public class FavouritesService:IFavouritesService
    {
        private readonly LogisticsSystemDbContext data;

        private readonly IConfigurationProvider mapper;

        public FavouritesService(LogisticsSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Add(string loadId, string userId)
        {
            var favourite = new Favourite
            {
                LoadId = loadId,
                UserId = userId
            };

            this.data.Favourites.Add(favourite);

            this.data.SaveChanges();

        }

        public IEnumerable<FavouriteServiceModel> MyFavourites(string userId)
        => this.data.Favourites.Where(x => x.UserId == userId)
            .ProjectTo<FavouriteServiceModel>(mapper)
            .ToList();

        public bool Delete(int id)
        {
            var favourite = this.data.Favourites.Find(id);

            if (favourite == null)
            {
                return false;
            }

            this.data.Favourites.Remove(favourite);

            this.data.SaveChanges();

            return true;

        }

        public bool IsFavouriteByUser(int id, string userId)
         => this.data.Favourites.Any(x => x.Id == id && x.UserId == userId);

        public bool IsFavouriteExists(string loadId, string userId)
        => this.data.Favourites.Any(x => x.LoadId == loadId && x.UserId == userId);
    }
}
