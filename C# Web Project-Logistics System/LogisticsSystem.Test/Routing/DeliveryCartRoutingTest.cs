using LogisticsSystem.Controllers;
using LogisticsSystem.Services.DeliveryCarts.Models;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace LogisticsSystem.Test.Routing
{
    public class DeliveryCartRoutingTest
    {
        [Fact]
        public void PostEditShouldBeMapped()
           => MyRouting
               .Configuration()
                .ShouldMap(request => request
                   .WithPath($"/DeliveryCart/Edit/{1}")
                    .WithMethod(HttpMethod.Post))
                .To<DeliveryCartController>(c => c
                .Edit(1, With.Any<CartItemServiceModel>()));
    }
}
