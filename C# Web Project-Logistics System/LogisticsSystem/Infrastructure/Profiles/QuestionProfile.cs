using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Questions.Models;
using System.Linq;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class QuestionProfile:Profile
    {
        public QuestionProfile()
        {
            this.CreateMap<Question, QuestionListingServiceModel>()
                .ForMember(x => x.ResponsesCount, cfg => cfg.MapFrom(x => x.Responses.Count(c => c.IsPublic)))
                .ForMember(x => x.LoadCondition, cfg => cfg.MapFrom(x => (int)x.Load.LoadCondition))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
                .ForMember(x => x.LoadTitle, cfg => cfg.MapFrom(x => x.Load.Title))
                .ForMember(x => x.LoadImage, cfg => cfg.MapFrom(x => x.Load.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<Question, QuestionDetailsServiceModel>()
                .ForMember(x => x.ResponsesCount, cfg => cfg.MapFrom(x => x.Responses.Count))
                .ForMember(x => x.LoadCondition, cfg => cfg.MapFrom(x => (int)x.Load.LoadCondition))
                .ForMember(x => x.LoadPrice, cfg => cfg.MapFrom(x => x.Load.Price.ToString(PriceFormat)))
                .ForMember(x => x.ResponsesCount, cfg => cfg.MapFrom(x => x.Responses.Count))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
                .ForMember(x => x.LoadTitle, cfg => cfg.MapFrom(x => x.Load.Title))
                .ForMember(x => x.LoadImage, cfg => cfg.MapFrom(x => x.Load.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<Question, QuestionServiceModel>()
                .ForMember(x => x.ResponsesCount, cfg => cfg.MapFrom(x => x.Responses.Count(c => c.IsPublic)))
                .ForMember(x => x.LoadCondition, cfg => cfg.MapFrom(x => (int)x.Load.LoadCondition))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));
        }
    }
}
