using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    

   public static class Questions
    {
        public static string GetInformation(int condition=2,string content="Quest" )
            => String.Concat(condition + "-" + new DateTime(1, 1, 1).ToString("dd MMM yyy") + "-" + new string(content.Take(5).ToArray()));


        public static List<Question> GetQuestions(         
            int count=5,
            bool isPublic=true,
            bool sameUser=true)

        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };

            var questions = Enumerable
               .Range(1, count)
               .Select(i => new Question
               {
                   Id = i,
                  
                   Content = $"Question Content {i}",
                   IsPublic = isPublic,
                   PublishedOn = new DateTime(1, 1, 1),
                   User = sameUser ? user : new User
                   {
                       Id = $"Question Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Load = new Load
                   {
                       LoadCondition = LogisticsSystem.Data.Models.Enums.LoadCondition.New
                   },
                   
               })
               .ToList();

            return questions;

        }




        public static List<Question> GetQuestionsByLoad(string loadId = "TestId", int count = 5)
          => Enumerable.Range(0, count).Select(i => new Question()
          {
              Content = "Question Content",
              IsPublic = true,
              PublishedOn = new DateTime(1, 1, 1),
              LoadId = loadId,
              User =  new User
               {
                     Id = $"Question Author Id {i}",
                     UserName = $"Author {i}"
               },
          })
            .ToList();
    }
}
