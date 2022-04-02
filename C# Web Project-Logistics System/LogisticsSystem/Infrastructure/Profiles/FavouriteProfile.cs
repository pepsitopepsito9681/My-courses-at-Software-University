using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Favourites.Models;
using System.Linq;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class FavouriteProfile:Profile
    {
        public FavouriteProfile()
        {
            this.CreateMap<Favourite, FavouriteServiceModel>()
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.Load.Price.ToString(PriceFormat)))
                .ForMember(x => x.LoadCondition, cfg => cfg.MapFrom(x => x.Load.LoadCondition.ToString()))
                .ForMember(x => x.LoadDelivery, cfg => cfg.MapFrom(x => x.Load.PersonType.ToString()))
                .ForMember(x => x.Quantity, cfg => cfg.MapFrom(x => x.Load.Quantity))
                .ForMember(x => x.ReviewsCount, cfg => cfg.MapFrom(x => x.Load.Reviews.Count))
                .ForMember(x => x.QuestionsCount, cfg => cfg.MapFrom(x => x.Load.Questions.Count))
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Load.Images.Select(x => x.ImageUrl).FirstOrDefault()));
        }
    }
}
