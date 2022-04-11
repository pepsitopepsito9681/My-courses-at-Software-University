using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.DeliveryCarts.Models;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static LogisticsSystem.Test.Data.DeliveryCartItems;

namespace LogisticsSystem.Test.Controllers
{

    public class DeliveryCartControllerTest
    {
        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnRedirectToMyDeliveryCartWithCorrectData()
          => MyController<DeliveryCartController>
             .Instance(instance => instance
             .WithUser()
             .WithData(GetDeliveryCartItems(1)))
             .Calling(c => c.Edit(1, new CartItemServiceModel
             {
                 Quantity = 3,
                 LoadQuantity = 5
             }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests()
                      .RestrictingForHttpMethod(HttpMethod.Post))
             .Data(data => data.WithSet<DeliveryCartItem>(set =>
             {
                 var cartItem = set.Find(1);

                 cartItem.Quantity.Should().Be(3);


             }))
              .TempData(tempData => tempData
                     .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
             .AndAlso()
             .ShouldReturn()
             .Redirect(redirect => redirect
                  .To<DeliveryCartController>(c => c.MyDeliveryCart()));


        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenCartItemIsNotOfUser()
         => MyController<DeliveryCartController>
            .Instance(instance => instance
            .WithUser()
            .WithData(GetDeliveryCartItems(1, sameUser: false)))
            .Calling(c => c.Edit(1, new CartItemServiceModel
            {
                Quantity = 3,
                LoadQuantity = 5
            }))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests()
                     .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .BadRequest();


        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnNotFoundWhenUserIsAdminCartItemDoesNotExist()
        => MyController<DeliveryCartController>
           .Instance(instance => instance
           .WithUser(new[] { AdminConstants.AdministratorRoleName }))
           .Calling(c => c.Edit(1, new CartItemServiceModel
           {
               Quantity = 3,
               LoadQuantity = 5
           }))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
           .AndAlso()
           .ShouldReturn()
           .NotFound();

    }
}
