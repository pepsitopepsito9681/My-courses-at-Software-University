using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Areas.Admin.Models.Orders;
using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using OrdersController = LogisticsSystem.Areas.Admin.Controllers.OrdersController;
using static LogisticsSystem.Test.Data.Orders;

namespace LogisticsSystem.Test.Controllers.Admin
{


    public class OrdersControllerTest
    {
        [Fact]
        public void PostDeleteShouldDeleteOrderAndReturnRedirectToUnAccomplished()
            => MyController<OrdersController>
                  .Instance(controller => controller
                        .WithUser(new[] { AdminConstants.AdministratorRoleName })
                        .WithData(GetOrder()))
                 .Calling(c => c.Delete(1, new OrderDeleteFormModel
                 {
                     ConfirmDeletion = true

                 }))
              .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                   .Data(data => data.WithSet<Order>(set =>
                   {
                       set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                       set.Should().BeEmpty();
                   }))
               .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(OrdersController.UnAccomplished));

        [Fact]
        public void PostDeleteShouldReturnRedirectToUnAccomplishedWhenNotConfirmDeletionOrder()
            => MyController<OrdersController>
                  .Instance(controller => controller
                        .WithUser(new[] { AdminConstants.AdministratorRoleName })
                        .WithData(GetOrder()))
                 .Calling(c => c.Delete(1, new OrderDeleteFormModel
                 {
                     ConfirmDeletion = false

                 }))
              .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(OrdersController.UnAccomplished));

        [Fact]
        public void PostDeleteShouldReturnNotFoundWhenOrderDoesNotExist()
          => MyController<OrdersController>
                .Instance(controller => controller
                      .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .Calling(c => c.Delete(1, new OrderDeleteFormModel
               {
                   ConfirmDeletion = true

               }))
            .ShouldHave()
                 .ActionAttributes(attributes => attributes
                                .RestrictingForHttpMethod(HttpMethod.Post))
                 .ValidModelState()
              .AndAlso()
              .ShouldReturn()
              .NotFound();

    }
}
