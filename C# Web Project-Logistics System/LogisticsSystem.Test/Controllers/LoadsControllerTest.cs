using FluentAssertions;
using LogisticsSystem.Areas.Admin;
using LogisticsSystem.Controllers;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Models.Loads;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static LogisticsSystem.Test.Data.Kinds;
using static LogisticsSystem.Test.Data.Dealers;
using static LogisticsSystem.Test.Data.Loads;

namespace LogisticsSystem.Test.Controllers
{

    public class LoadsControllerTest
    {
        [Fact]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectToAndCorrectData()
            => MyController<LoadsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetDealer())
                         .WithData(GetKinds()))
                 .Calling(c => c.Add(new LoadFormModel
                 {
                     Title = "TitleTest",
                     Description = "DescriptionTest",
                     Price = 12.50M,
                     Quantity = 2,
                     FirstImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.nike.com%2Fbg%2Ft%2Fair-max-270-older-shoes-9KTdjGPz&psig=AOvVaw1BLwISSNYoflfMiovKE7bp&ust=1649433318375000&source=images&cd=vfe&ved=0CAoQjRxqFwoTCLCXoo2ogvcCFQAAAAAdAAAAABAD",
                     KindId = 1,
                     SubKindId = 1,
                     Condition = LoadCondition.New,
                     Delivery = PersonType.Seller,
                     AgreeOnTermsOfPolitics = true

                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Load>(set => set
                             .Any(x =>
                             x.Title == "TitleTest" &&
                             x.Description == "DescriptionTest" &&
                             x.Price == 12.50M &&
                             x.Quantity == 2 &&
                             x.Images.Any() &&
                             x.KindId == 1 &&
                             x.SubKindId == 1 &&
                             x.IsPublic == false &&
                             x.LoadCondition == LoadCondition.New &&
                             x.PersonType == PersonType.Seller)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                       .To<LoadsController>(c => c
                       .Details(With.Any<string>(), With.Any<LoadsDetailsQueryModel>())));


        [Fact]
        public void PostAddShouldReturnViewWhenFormIsWithWrongFields()
            => MyController<LoadsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetDealer()))
                 .Calling(c => c.Add(new LoadFormModel
                 {


                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .Data(data => data
                     .WithSet<Load>(set => set.Should().BeEmpty()))
                 .AndAlso()
                 .ShouldReturn()
                 .View(view => view
                      .WithModelOfType<LoadFormModel>());

        [Fact]
        public void PostAddShouldReturnBadRequestWhenUserIsNotDealer()
           => MyController<LoadsController>
               .Instance(controller => controller
                       .WithUser())
                .Calling(c => c.Add(new LoadFormModel
                {


                }))
               .ShouldHave()
              .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests()
                   .RestrictingForHttpMethod(HttpMethod.Post))
                 .AndAlso()
                .ShouldReturn()
                 .BadRequest();


        [Theory]
        [InlineData(
            "TitleTest",
            "DescriptionTest",
            12.55,
            2,
            FirstImageUrl,
            SecondImageUrl,
            ThirdImageUrl,
            2,
            1,
            LoadCondition.New,
            PersonType.Seller
            )]
        public void PostEditShouldEditLoadAndReturnRedirectToWithCorrectDataAndModel(
            string title,
            string description,
            decimal price,
            byte quantity,
            string firstImageUrl,
            string secondImageUrl,
            string thirdImageUrl,
            int kindId,
            int subKindId,
            LoadCondition condition,
            PersonType personType)
            => MyController<LoadsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetLoad()))
                 .Calling(c => c.Edit(LoadTestId, new LoadFormModel
                 {
                     Title = title,
                     Description = description,
                     Price = price,
                     Quantity = quantity,
                     FirstImageUrl = firstImageUrl,
                     SecondImageUrl = secondImageUrl,
                     ThirdImageUr = thirdImageUrl,
                     KindId = kindId,
                     SubKindId = subKindId,
                     Condition = condition,
                     Delivery = personType,

                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Load>(set => set
                             .Any(x =>
                             x.Title == title &&
                             x.Description == description &&
                             x.Price == price &&
                             x.Quantity == quantity &&
                             x.Images.Any() &&
                             x.KindId == kindId &&
                             x.SubKindId == subKindId &&
                             x.IsPublic == false &&
                             x.LoadCondition == condition &&
                             x.PersonType == personType)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                       .To<LoadsController>(c => c
                       .Details(LoadTestId, With.Any<LoadsDetailsQueryModel>())));




        [Fact]
        public void PostEditShouldReturnBadRequestWhenUserIsNotDealer()
         => MyController<LoadsController>
             .Instance(controller => controller
                     .WithUser())
              .Calling(c => c.Edit(LoadTestId, new LoadFormModel
              {


              }))
             .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
               .AndAlso()
              .ShouldReturn()
               .BadRequest();

        [Fact]
        public void PostEditShouldReturnNotFoundWhenLoadDoesNotExist()
      => MyController<LoadsController>
          .Instance(controller => controller
                  .WithUser(new[] { AdminConstants.AdministratorRoleName }))
           .Calling(c => c.Edit(LoadTestId, With.Any<LoadFormModel>()))
          .ShouldHave()
         .ActionAttributes(attributes => attributes
             .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
           .ShouldReturn()
            .NotFound();


        [Fact]
        public void PostEditShouldReturnBadRequestWhenLoadIsNotOfUser()
       => MyController<LoadsController>
           .Instance(controller => controller
                   .WithUser()
                   .WithData(GetDealer())
                   .WithData(GetLoad(LoadTestId, false)))
            .Calling(c => c.Edit(LoadTestId, new LoadFormModel
            {


            }))
           .ShouldHave()
          .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
               .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
            .ShouldReturn()
             .BadRequest();


        [Fact]
        public void PostEditShouldReturnViewWhenFormFieldsAreWrong()
   => MyController<LoadsController>
       .Instance(controller => controller
               .WithUser()
               .WithData(GetLoad()))
        .Calling(c => c.Edit(LoadTestId, new LoadFormModel
        {


        }))
       .ShouldHave()
      .ActionAttributes(attributes => attributes
          .RestrictingForAuthorizedRequests()
           .RestrictingForHttpMethod(HttpMethod.Post))
        .InvalidModelState()
         .AndAlso()
         .ShouldReturn()
                 .View(view => view
                      .WithModelOfType<LoadFormModel>());




        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnRedirectToWithCorrectData()
            => MyController<LoadsController>
                .Instance(controller => controller
               .WithUser()
               .WithData(GetLoad()))
               .Calling(c => c.Delete(LoadTestId, new LoadDeleteFormModel
               {
                   ConfirmDeletion = true
               }))
             .ShouldHave()
              .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
              .ValidModelState()
              .Data(data => data.WithSet<Load>(set => set.Any(x =>
                       x.Id == LoadTestId &&
                       x.IsDeleted == true &&
                       x.IsPublic == false &&
                       x.DeletedOn.HasValue)))
              .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
              .AndAlso()
              .ShouldReturn()
              .Redirect(redirect => redirect
                    .To<LoadsController>(c => c
                    .All(With.Any<LoadsQueryModel>())));



        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnRedirectToAdminAreaWithCorrectData()
        => MyController<LoadsController>
             .Instance(controller => controller
            .WithUser(new[] { AdminConstants.AdministratorRoleName })
            .WithData(GetLoad(LoadTestId, false)))
            .Calling(c => c.Delete(LoadTestId, new LoadDeleteFormModel
            {
                ConfirmDeletion = true
            }))
          .ShouldHave()
           .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))
           .ValidModelState()
           .Data(data => data.WithSet<Load>(set => set
                    .Should()
                    .BeEmpty()))
           .TempData(tempData => tempData
                    .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
                 .To<Areas.Admin.Controllers.LoadsController>(c => c
                       .Existing(With.Any<LoadsQueryModel>())));



        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsNotDealer()
        => MyController<LoadsController>
             .Instance(controller => controller
            .WithUser())
            .Calling(c => c.Delete(
                   LoadTestId,
                   With.Default<LoadDeleteFormModel>()))
          .ShouldHave()
           .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))
          .AndAlso()
          .ShouldReturn()
          .BadRequest();

        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenLoadDoesNotExist()
         => MyController<LoadsController>
          .Instance(controller => controller
         .WithUser()
         .WithData(GetDealer()))
         .Calling(c => c.Delete(
                LoadTestId,
                With.Default<LoadDeleteFormModel>()))
       .ShouldHave()
        .ActionAttributes(attributes => attributes
           .RestrictingForAuthorizedRequests()
           .RestrictingForHttpMethod(HttpMethod.Post))
       .AndAlso()
       .ShouldReturn()
       .BadRequest();

        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnNotFoundWhenUserIsAdminAndLoadDoesNotExist()
         => MyController<LoadsController>
          .Instance(controller => controller
         .WithUser(new[] { AdminConstants.AdministratorRoleName })
         .WithData())
         .Calling(c => c.Delete(
                LoadTestId,
                new LoadDeleteFormModel
                {
                    ConfirmDeletion = true
                }))
       .ShouldHave()
        .ActionAttributes(attributes => attributes
           .RestrictingForAuthorizedRequests()
           .RestrictingForHttpMethod(HttpMethod.Post))
       .AndAlso()
       .ShouldReturn()
       .NotFound();

        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnRedirectToDetailsWhenLoadWhenNotConfirmDeletion()
        => MyController<LoadsController>
         .Instance(controller => controller
        .WithUser()
        .WithData(GetLoad()))
        .Calling(c => c.Delete(
               LoadTestId,
               With.Default<LoadDeleteFormModel>()))
      .ShouldHave()
       .ActionAttributes(attributes => attributes
          .RestrictingForAuthorizedRequests()
          .RestrictingForHttpMethod(HttpMethod.Post))
      .AndAlso()
      .ShouldReturn()
      .Redirect(redirect => redirect
                 .To<LoadsController>(c => c
                       .Details(LoadTestId, With.Any<LoadsDetailsQueryModel>())));

    }
}
