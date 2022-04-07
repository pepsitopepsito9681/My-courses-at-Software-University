using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    public static class Responses
    {
        public static IEnumerable<Response> GetResponses(int count = 5, int questionId = 1, bool IsPublic = true)
       => Enumerable.Range(0, count).Select(i => new Response()
       {
           User = new User
           {
               FullName = TestUser.Username
           },
           QuestionId = questionId,
           IsPublic = IsPublic

       });


        public static Response GetResponse(int id = 1, bool IsPublic = true)
            => new()
            {
                Id = id,
                IsPublic = IsPublic
            };
    }
}
