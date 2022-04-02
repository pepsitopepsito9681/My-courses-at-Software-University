using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Models.Loads;
using LogisticsSystem.Services.Loads.Models;
using System;
using System.Linq;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class LoadProfile:Profile
    {
        public LoadProfile()
        {
            this.CreateMap<LoadDetailsServiceModel, LoadFormModel>()
          .ForMember(p => p.Condition, pd => pd.MapFrom(x => Enum.Parse<LoadCondition>(x.Condition)))
          .ForMember(p => p.Delivery, pd => pd.MapFrom(x => Enum.Parse<PersonType>(x.Delivery)))
          .ForMember(p => p.FirstImageUrl, pd => pd.MapFrom(x => x.MainImageUrl));

            this.CreateMap<Load, LoadServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.LoadCondition.ToString()))
            .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
            .ForMember(x => x.DeletedOn, cfg => cfg.MapFrom(x => x.DeletedOn.HasValue ? x.DeletedOn.Value.ToString(DateTimeFormat) : string.Empty))

            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<Load, LoadDetailsServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.LoadCondition.ToString()))
            .ForMember(p => p.Delivery, pd => pd.MapFrom(x => x.PersonType.ToString()))
            .ForMember(p => p.UserId, pd => pd.MapFrom(x => x.Trader.UserId))
            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.SecondImageUrl, pd => pd.MapFrom(x => x.Images.Skip(1).Take(1).Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.ThirdImageUrl, pd => pd.MapFrom(x => x.Images.Skip(2).Take(1).Select(x => x.ImageUrl).FirstOrDefault()));
        }
    }
}
