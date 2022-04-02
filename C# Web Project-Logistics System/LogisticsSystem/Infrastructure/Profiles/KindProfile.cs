using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Loads.Models;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class KindProfile:Profile
    {
        public KindProfile()
        {
            this.CreateMap<Kind, LoadKindServiceModel>();
            this.CreateMap<SubKind, LoadSubKindServiceModel>();
        }
    }
}
