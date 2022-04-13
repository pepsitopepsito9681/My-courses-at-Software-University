using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Models.Loads;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.WebConstants;

namespace LogisticsSystem.Test.Business
{

    public class TradersBusinessTest
    {

        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request.WithLocation("/Traders/Become")
            .WithUser())
            .To<TradersController>(c => c.Become())
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        [Theory]
        [InlineData("Dealer", "0888888888")]
        public void PostBecomeShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
            string traderName,
            string phoneNumber
            )
          => MyPipeline
            .Configuration()
             .ShouldMap(request => request
              .WithLocation("/Traders/Become")
              .WithMethod(HttpMethod.Post)
               .WithFormFields(new
               {
                   Name = traderName,
                   TelephoneNumber = phoneNumber
               })
              .WithUser()
                  .WithAntiForgeryToken())
                .To<TradersController>(c => c.Become(new Models.Traders.BecomeTraderFormModel
                {
                    Name = traderName,
                    TelephoneNumber = phoneNumber
                }))
              .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Trader>(set => set
                             .Any(x =>
                             x.Name == traderName &&
                             x.TelephoneNumber == phoneNumber &&
                             x.UserId == TestUser.Identifier)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                      .To<LoadsController>(c => c
                      .All(With.Any<LoadsQueryModel>())));

    }
}
