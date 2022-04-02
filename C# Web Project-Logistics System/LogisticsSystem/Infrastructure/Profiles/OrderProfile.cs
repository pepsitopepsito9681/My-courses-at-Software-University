using AutoMapper;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Orders.Models;
using System.Linq;
using static LogisticsSystem.Infrastructure.ProfileConstants;

namespace LogisticsSystem.Infrastructure.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            this.CreateMap<Order, OrderServiceModel>()
                  .ForMember(x => x.OrderedOn, cfg => cfg.MapFrom(x => x.OrderedOn.ToString(DateTimeFormat)))
                  .ForMember(x => x.TotalAmount, cfg => cfg.MapFrom(x => x.DeliveryCart.Sum(x => (decimal)x.Quantity * x.Load.Price).ToString(PriceFormat)));


            this.CreateMap<Order, OrderDetailsServiceModel>()
            .ForMember(x => x.OrderedOn, cfg => cfg.MapFrom(x => x.OrderedOn.ToString(DateTimeFormat)))
            .ForMember(x => x.OrderedItems, cfg => cfg.MapFrom(x => x.DeliveryCart.Sum(x => x.Quantity)))
            .ForMember(x => x.TotalAmount, cfg => cfg.MapFrom(x => x.DeliveryCart.Sum(x => (decimal)x.Quantity * x.Load.Price).ToString(PriceFormat)));
        }
    }
}
