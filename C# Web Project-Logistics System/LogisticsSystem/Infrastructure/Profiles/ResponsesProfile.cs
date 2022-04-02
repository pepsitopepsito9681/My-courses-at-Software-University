using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Responses.Models;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class ResponsesProfile:Profile
    {
        public ResponsesProfile()
        {
            this.CreateMap<Response, ResponseServiceModel>()
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));
        }
    }
}
