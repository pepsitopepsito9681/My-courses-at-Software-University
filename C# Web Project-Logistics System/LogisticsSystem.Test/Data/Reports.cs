using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    

  public static  class Reports
    {
        public const string TestContent = nameof(TestContent);


        public static List<Report> GetReports(
            int count = 5,
            bool sameUser = true)    
       {

            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };

            var reports = Enumerable
               .Range(1, count)
               .Select(i => new Report
               {
                   Id = i,
                   ReportKind = LogisticsSystem.Data.Models.Enums.ReportKind.Sexual,
                   Content = TestContent,
                   PublishedOn = new DateTime(1, 1, 1),
                   User = sameUser ? user : new User
                   {
                       Id = $"Review Author Id {i}",
                       UserName = $"Author {i}"
                   },                 
                   LoadId = "TestId"
               })
               .ToList();

            return reports;
        }
    }
}
