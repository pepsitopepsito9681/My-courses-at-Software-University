using FluentAssertions;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Orders.Models;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.DeliveryCartItems;
using static LogisticsSystem.Test.Data.Dealers;

namespace LogisticsSystem.Test.Pipeline
{


    public class OrdersPipelineTest
    {
        [Fact]
        public void GetAddOrderShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                  .WithLocation("/Orders/Add")
                  .WithUser())

               .To<OrdersController>(c => c.Add())
            .Which(controller => controller
                 .WithData(GetDeliveryCartItems())
                 .AndAlso()
                 .WithData(GetDealer())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<OrderFormServiceModel>()
                 .Passing(model =>
                 {
                     model.FullName.Should().BeNull();
                     model.TelephoneNumber.Should().Be(TelephoneNumber);

                 })));


        [Fact]
        public void GetAddOrderShouldBeForAuthorizedUsersAndReturnRedirectToDeliveryCartMyDeliveryCartWhenCartContainsInvalidItems()
    => MyPipeline
        .Configuration()
        .ShouldMap(request => request
          .WithLocation("/Orders/Add")
          .WithUser())

          .To<OrdersController>(c => c.Add())
         .Which(controller => controller
            .WithData(GetDeliveryCartItems(cartQuantity: 0, loadQuantity: 0)))
          .ShouldHave()
          .ActionAttributes(attributes => attributes
            .RestrictingForAuthorizedRequests())
           .TempData(tempData => tempData
               .ContainingEntryWithKey(WebConstants.GlobalErrorMessageKey))
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
            .To<DeliveryCartController>(c => c
            .MyDeliveryCart()));



        [Theory]
        [InlineData("Petar Kostov", TelephoneNumber, "Sofia", "Sofia", "Vazrojdenska 37", "1220", 3)]
        public void PostAddOrderShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel(
            string fullName,
            string telephoneNumber,
            string state,
            string city,
            string address,
            string postCode,
            int cartItemsCount)
          => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                .WithLocation("/Orders/Add")
                .WithMethod(HttpMethod.Post)
                .WithFormFields(new
                {
                    FullName = fullName,
                    TelephoneNumber = telephoneNumber,
                    State = state,
                    City = city,
                    Address = address,
                    PostCode = postCode

                })
                .WithUser()
                .WithAntiForgeryToken())

             .To<OrdersController>(c => c.Add(new OrderFormServiceModel
             {
                 FullName = fullName,
                 TelephoneNumber = TelephoneNumber,
                 State = state,
                 City = city,
                 Address = address,
                 PostCode = postCode

             }))
          .Which(controller => controller
               .WithData(GetDeliveryCartItems(cartItemsCount)))
          .ShouldHave()
          .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests()
               .RestrictingForHttpMethod(HttpMethod.Post))
            .ValidModelState()
           .Data(data => data.WithSet<Order>(set => set.Any(x =>
                   x.FullName == fullName &&
                   x.PhoneNumber == telephoneNumber &&
                   x.State == state &&
                   x.City == city &&
                   x.Address == address &&
                   x.PostCode == postCode &&
                   x.DeliveryCart.Count == cartItemsCount)))
            .TempData(tempData => tempData
                .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
          .AndAlso()
          .ShouldReturn()
          .Redirect(redirect => redirect
                .To<HomeController>(c =>
                c.Index()));





        [Theory]
        [InlineData("Ivan Ivanov", TelephoneNumber, "Kirdjali", "Momchilgrad", "Bacho Kiro 15", "6800", 3)]
        public void PostAddOrderShouldBeForAuthorizedUsersAndReturnRedirectToDeliveryCartMyDeliveryCartWhenCartHasInvalidItems(
           string fullName,
           string telephoneNumber,
           string state,
           string city,
           string address,
           string postCode,
           int cartItemsCount)
         => MyPipeline
             .Configuration()
             .ShouldMap(request => request
               .WithLocation("/Orders/Add")
               .WithMethod(HttpMethod.Post)
               .WithFormFields(new
               {
                   FullName = fullName,
                   TelephoneNumber = telephoneNumber,
                   State = state,
                   City = city,
                   Address = address,
                   PostCode = postCode

               })
               .WithUser()
               .WithAntiForgeryToken())

            .To<OrdersController>(c => c.Add(new OrderFormServiceModel
            {
                FullName = fullName,
                TelephoneNumber = TelephoneNumber,
                State = state,
                City = city,
                Address = address,
                PostCode = postCode

            }))
         .Which(controller => controller
              .WithData(GetDeliveryCartItems(cartItemsCount, cartQuantity: 0, loadQuantity: 0)))
         .ShouldHave()
         .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))
           .TempData(tempData => tempData
               .ContainingEntryWithKey(WebConstants.GlobalErrorMessageKey))
         .AndAlso()
         .ShouldReturn()
         .Redirect(redirect => redirect
               .To<DeliveryCartController>(c =>
               c.MyDeliveryCart()));


    }
}
