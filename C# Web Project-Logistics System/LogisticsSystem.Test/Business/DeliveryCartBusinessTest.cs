using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Models.Loads;
using LogisticsSystem.Services.DeliveryCarts.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Loads;
using static LogisticsSystem.Test.Data.DeliveryCartItems;

namespace LogisticsSystem.Test.Business
{


    public class DeliveryCartBusinessTest
    {
        [Fact]
        public void MyDeliveryCartShouldBeForAuthorizedUsersAndReturnViewWithCorrectDataAndModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request.WithLocation("/DeliveryCart/MyDeliveryCart")
                 .WithUser())
                 .To<DeliveryCartController>(c => c.MyDeliveryCart())
                 .Which(controller => controller.WithData(GetDeliveryCartItems()))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                             .RestrictingForAuthorizedRequests())
                  .AndAlso()
                  .ShouldReturn()
                   .View(view => view.WithModelOfType<List<DeliveryCartItemServiceModel>>()
                     .Passing(model => model.Should().HaveCount(5)));


        [Fact]
        public void DeleteShouldBeForAuthorizedUsersAndDeleteCartItemAndReturnRedirectToMyDeliveryCart()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request.WithLocation($"/DeliveryCart/Delete/{1}")
                  .WithUser())
                  .To<DeliveryCartController>(c => c.Delete(1))
                  .Which(controller => controller.WithData(GetDeliveryCartItems(1)))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                             .RestrictingForAuthorizedRequests())
                  .Data(data => data.WithSet<DeliveryCartItem>(set =>
                  {
                      set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                      set.Should().BeEmpty();
                  }))
                  .AndAlso()
                  .ShouldReturn()
                  .Redirect(redirect => redirect
                    .To<DeliveryCartController>(c => c.MyDeliveryCart()));

        [Fact]
        public void DeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenCartItemIsNotOfUser()
          => MyPipeline
               .Configuration()
               .ShouldMap(request => request.WithLocation($"/DeliveryCart/Delete/{1}")
                .WithUser())
                .To<DeliveryCartController>(c => c.Delete(1))
                .Which(controller => controller.WithData(GetDeliveryCartItems(1, sameUser: false)))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                           .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void DeleteShouldBeForAuthorizedUsersAndReturnNotFoundUserIsAdminAndCartItemDoesNotExist()
         => MyPipeline
              .Configuration()
              .ShouldMap(request => request.WithLocation($"/DeliveryCart/Delete/{1}")
               .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .To<DeliveryCartController>(c => c.Delete(1))
               .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .NotFound();

        [Fact]
        public void AddShouldBeForAuthorizedUsersAndReturnRedirectToMyDeliveryCartWIthCorrectDataAndModel()
            => MyPipeline
                .Configuration()
                 .ShouldMap(request => request.WithLocation($"/DeliveryCart/Add/{LoadTestId}")
                .WithUser())
                .To<DeliveryCartController>(c => c.Add(LoadTestId))
                 .Which(controller => controller.WithData(GetLoad(userSame: false)))
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                 .Data(data => data.WithSet<DeliveryCartItem>(set =>
                 {
                     var cartItem = set.FirstOrDefault();

                     cartItem.LoadId.Should().Be(LoadTestId);
                     cartItem.UserId.Should().Be(TestUser.Identifier);
                     cartItem.Quantity.Should().Be(1);

                 }))
                 .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                       .AndAlso()
                       .ShouldReturn()
                       .Redirect(redirect => redirect
                          .To<LoadsController>(c => c
                          .Details(LoadTestId, With.Any<LoadsDetailsQueryModel>())));


        [Fact]
        public void AddShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsTraderOfLoad()
           => MyPipeline
               .Configuration()
                .ShouldMap(request => request.WithLocation($"/DeliveryCart/Add/{LoadTestId}")
               .WithUser())
               .To<DeliveryCartController>(c => c.Add(LoadTestId))
                .Which(controller => controller.WithData(GetLoad()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .BadRequest();


        [Fact]
        public void AddShouldBeForAuthorizedUsersAndReturnNotFoundWhenLoadDoesNotExist()
           => MyPipeline
               .Configuration()
                .ShouldMap(request => request.WithLocation($"/DeliveryCart/Add/{LoadTestId}")
               .WithUser())
               .To<DeliveryCartController>(c => c.Add(LoadTestId))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                  .AndAlso()
                  .ShouldReturn()
                     .NotFound();


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request.WithLocation($"/DeliveryCart/Edit/{1}")
               .WithUser())
               .To<DeliveryCartController>(c => c.Edit(1))
                .Which(controller => controller.WithData(GetDeliveryCartItems(1)))
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view
                     .WithModelOfType<CartItemServiceModel>()
                   .Passing(model =>
                   {
                       model.Quantity.Should().Be(1);
                       model.LoadQuantity.Should().Be(5);

                   }));


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenCartItemIsNotOfUser()
          => MyPipeline
              .Configuration()
              .ShouldMap(request => request.WithLocation($"/DeliveryCart/Edit/{1}")
             .WithUser())
             .To<DeliveryCartController>(c => c.Edit(1))
              .Which(controller => controller.WithData(GetDeliveryCartItems(1, sameUser: false)))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .BadRequest();


    }
}
