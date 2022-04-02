using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Reports.Models;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class ReportProfile:Profile
    {
        public ReportProfile()
        {
            this.CreateMap<Report, ReportServiceModel>()
              .ForMember(x => x.ReportKind, cfg => cfg.MapFrom(x => x.ReportKind.ToString()))
              .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
              .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));
        }
    }
}
