using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Comments.Models;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class CommentProfile:Profile
    {
        public CommentProfile()
        {
            this.CreateMap<Comment, CommentServiceModel>()
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));
        }
    }
}
