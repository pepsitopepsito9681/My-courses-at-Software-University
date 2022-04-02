using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Models.Reviews;
using LogisticsSystem.Services.Reviews.Models;
using System.Linq;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class ReviewProfile:Profile
    {
        public ReviewProfile()
        {
            this.CreateMap<Review, ReviewListingServiceModel>()
                 .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (int)x.Rating))
                 .ForMember(x => x.TotalComments, cfg => cfg.MapFrom(x => x.Comments.Count(c => c.IsPublic)))
                 .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
                 .ForMember(x => x.LoadImage, cfg => cfg.MapFrom(x => x.Load.Images.Select(x => x.ImageUrl).FirstOrDefault())
                 );

            this.CreateMap<Review, ReviewServiceModel>()
                .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (int)x.Rating))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.TotalComments, cfg => cfg.MapFrom(x => x.Comments.Count(c => c.IsPublic)))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));


            this.CreateMap<Review, ReviewDetailsServiceModel>()
                .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (int)x.Rating))
                .ForMember(x => x.LoadPrice, cfg => cfg.MapFrom(x => x.Load.Price.ToString(PriceFormat)))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
                .ForMember(x => x.LoadImage, cfg => cfg.MapFrom(x => x.Load.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<ReviewDetailsServiceModel, ReviewFormModel>()
                .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (ReviewKind)x.Rating));
        }
    }
}
