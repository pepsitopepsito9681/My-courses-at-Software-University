using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    public static class Reviews
    {
        public const string TestContent = nameof(TestContent);

        public const string Title = "TestReview";
        public const int Rating = 5;


        public static string GetInformation(
            string title=Title,        
            int rating=Rating)
         => String.Concat(title + "-" + rating + "-" + new DateTime(1, 1, 1).ToString("dd MMM yyy"));

       public static List<Review> GetReviews(int count = 5,
            bool isPublic = true,
            bool sameUser = true)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };


            var reviews = Enumerable
               .Range(1, count)
               .Select(i => new Review
               {
                   Id = i,
                   Title = Title,
                   Rating = LogisticsSystem.Data.Models.Enums.ReviewKind.Excellent,
                   Content = TestContent,
                   IsPublic = isPublic,
                   PublishedOn = new DateTime(1, 1, 1),
                   User = sameUser ? user : new User
                   {
                       Id = $"Review Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Load = new Load
                   {
                       Title = "Test",
                       LoadCondition = LogisticsSystem.Data.Models.Enums.LoadCondition.New
                   },
                   LoadId = "TestId"
               })
               .ToList();


            return reviews;
        }


        public static List<Review> GetReviewsByLoad(string loadId="TestId",int count=5)
          => Enumerable.Range(0, count).Select(i => new Review()
          {
              Title = Title,
              Rating = LogisticsSystem.Data.Models.Enums.ReviewKind.Excellent,
              Content = TestContent,
              IsPublic = true,
              PublishedOn = new DateTime(1, 1, 1),
              LoadId =loadId,
              User = new User
              {
                  Id = $"Review Author Id {i}",
                  UserName = $"Author {i}"
              },


          })
            .ToList();

    }
}
