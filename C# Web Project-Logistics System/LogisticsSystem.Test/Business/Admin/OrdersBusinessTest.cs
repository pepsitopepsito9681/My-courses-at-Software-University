using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Areas.Admin.Models.Orders;
using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Orders;
using OrdersController = LogisticsSystem.Areas.Admin.Controllers.OrdersController;

namespace LogisticsSystem.Test.Business.Admin
{


    public class OrdersBusinessTest
    {
        [Fact]
        public void UnAccomplishedShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                   .WithLocation("/Admin/Orders/UnAccomplished")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c => c
                       .UnAccomplished(With.Default<OrdersQueryModel>()))
                 .Which(controller => controller.WithData(GetOrders(5)))
                 .ShouldReturn()
                 .View(view => view
                    .WithModelOfType<OrdersQueryModel>()
                    .Passing(model =>
                    {
                        model.Orders.Should().HaveCount(5);
                        model.TotalOrders.Should().Be(5);
                    }));


        [Fact]
        public void AccomplishedShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                   .WithLocation("/Admin/Orders/Accomplished")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c => c
                       .Accomplished(With.Default<OrdersQueryModel>()))
                 .Which(controller => controller.WithData(GetOrders(5, accomplished: true)))
                 .ShouldReturn()
                 .View(view => view
                    .WithModelOfType<OrdersQueryModel>()
                    .Passing(model =>
                    {
                        model.Orders.Should().HaveCount(5);
                        model.TotalOrders.Should().Be(5);
                    }));


        [Theory]
        [InlineData(1, 1, 5)]
        public void AccomplishShouldChangeOrderIfIsUnAccomplishedAndReturnRedirectToAccomplished(
            int cartItemsCount,
            byte quantityPerCartItem,
            byte quantityPerLoad)

          => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                   .WithLocation($"/Admin/Orders/Accomplish/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c => c
                       .Accomplish(1))
                 .Which(controller => controller
                         .WithData(GetOrder(
                          cartItemsCount: cartItemsCount,
                          quantityPerItem: quantityPerCartItem,
                          quantityPerLoad: quantityPerLoad)))
                 .ShouldHave()
                 .TempData(tempData => tempData
                        .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                  .Data(data => data.WithSet<Order>(set =>
                  {
                      var order = set.FirstOrDefault();
                      order.IsAccomplished.Should().BeTrue();

                  }))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                      .To<OrdersController>(c => c
                      .Accomplished(With.Any<OrdersQueryModel>())));


        [Fact]
        public void AccomplishShouldReturnNotFoundWhenOrderDoesNotExist()
        => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                 .WithLocation($"/Admin/Orders/Accomplish/{1}")
                 .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .To<OrdersController>(c => c
                     .Accomplish(1))
               .Which()
               .ShouldReturn()
               .NotFound();


        [Theory]
        [InlineData(1, 1, 5)]
        public void CancelShouldRetunRedirectToUnAccomplished(
            int cartItemsCount,
            byte quantityPerCartItem,
            byte quantityPerLoad)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"Admin/Orders/Cancel/{1}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c => c.Cancel(1))
                  .Which(controller => controller
                         .WithData(GetOrder(
                          accomplished: true,
                          cartItemsCount: cartItemsCount,
                          quantityPerItem: quantityPerCartItem,
                          quantityPerLoad: quantityPerLoad)))
                 .ShouldHave()
                 .TempData(tempData => tempData
                        .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                  .Data(data => data.WithSet<Order>(set =>
                  {
                      var order = set.FirstOrDefault();
                      order.IsAccomplished.Should().BeFalse();

                  }))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                      .To<OrdersController>(c => c
                      .UnAccomplished(With.Any<OrdersQueryModel>())));

        [Fact]
        public void CancelShouldReturnNotFoundWhenOrderDoesNotExist()
       => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                .WithLocation($"/Admin/Orders/Cancel/{1}")
                .WithUser(new[] { AdminConstants.AdministratorRoleName }))
              .To<OrdersController>(c => c
                    .Cancel(1))
              .Which()
              .ShouldReturn()
              .NotFound();


        [Fact]
        public void GetDeleteShouldReturnViewWithCorrectModel()
            => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                .WithLocation($"/Admin/Orders/Delete/{1}")
                .WithUser(new[] { AdminConstants.AdministratorRoleName }))
              .To<OrdersController>(c => c
                    .Delete(1))
              .Which(controller => controller
                  .WithData(GetOrder())
              .ShouldReturn()
              .View());


        [Fact]
        public void GetDeleteShouldReturnNotFoundWhenOrderDoesNotExist()
           => MyPipeline
             .Configuration()
             .ShouldMap(request => request
               .WithLocation($"/Admin/Orders/Delete/{1}")
               .WithUser(new[] { AdminConstants.AdministratorRoleName }))
             .To<OrdersController>(c => c
                   .Delete(1))
             .Which()
             .ShouldReturn()
             .NotFound();

        [Fact]
        public void DetailsShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
             .Configuration()
             .ShouldMap(request => request
               .WithLocation($"/Admin/Orders/Details/{1}")
               .WithUser(new[] { AdminConstants.AdministratorRoleName }))
             .To<OrdersController>(c => c
                   .Details(1))
             .Which(controller => controller.WithData(GetOrder(cartItemsCount: 3)))
             .ShouldReturn()
             .View(view => view
             .WithModelOfType<OrderDetailsModel>()
             .Passing(model =>
             {
                 model.Order.Should().NotBeNull();
                 model.OrderItems.Should().HaveCount(3);
             }));

    }
}
