using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.DeliveryCarts.Models;
using System.Linq;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class DeliveryCartItemProfile:Profile
    {
        public DeliveryCartItemProfile()
        {
            this.CreateMap<DeliveryCartItem, DeliveryCartItemServiceModel>()
               .ForMember(x => x.LoadCondition, cfg => cfg.MapFrom(x => x.Load.LoadCondition.ToString()))
               .ForMember(x => x.LoadDelivery, cfg => cfg.MapFrom(x => x.Load.PersonType.ToString()))
               .ForMember(x => x.Quantity, cfg => cfg.MapFrom(x => x.Quantity))
               .ForMember(x => x.LoadQuantity, cfg => cfg.MapFrom(x => x.Load.Quantity))
               .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Load.Images.Select(x => x.ImageUrl).FirstOrDefault()))
               .ForMember(x => x.Price, cfg => cfg.MapFrom(x => (x.Load.Price * (decimal)x.Quantity)))
               .ForMember(x => x.TraderName, cfg => cfg.MapFrom(x => x.Load.Trader != null ? x.Load.Trader.Name : null))
               .ForMember(x => x.TraderTelephoneNumber, cfg => cfg.MapFrom(x => x.Load.Trader != null ? x.Load.Trader.TelephoneNumber : null));
        }
    }
}
