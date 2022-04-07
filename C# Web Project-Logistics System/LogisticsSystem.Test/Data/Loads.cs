using LogisticsSystem.Data.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    
    public static class Loads
    {
        public const string LoadTestId = "TestId";

        public const string FirstImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.nike.com%2Fbg%2Ft%2Fair-max-270-older-shoes-9KTdjGPz&psig=AOvVaw1BLwISSNYoflfMiovKE7bp&ust=1649433318375000&source=images&cd=vfe&ved=0CAoQjRxqFwoTCLCXoo2ogvcCFQAAAAAdAAAAABAD";

        public const string SecondImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fwww.obuvki.bg%2Fmedia%2Fcatalog%2Fproduct%2Fcache%2Fimage%2F650x650%2F0%2F0%2F0000208588713_01_rz.jpg&imgrefurl=https%3A%2F%2Fwww.obuvki.bg%2Fobuvki-nike-air-max-95-recraft-gs-cj3906-001-black-black-black-white.html&tbnid=NeZATe9NihwR-M&vet=12ahUKEwjmv7mJqIL3AhVuiP0HHeJRB8sQMygRegUIARDsAQ..i&docid=XDoQmvsXnsYocM&w=650&h=650&q=nike%20air%20max&ved=2ahUKEwjmv7mJqIL3AhVuiP0HHeJRB8sQMygRegUIARDsAQ";

        public const string ThirdImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fstatic.nike.com%2Fa%2Fimages%2Ft_PDP_1280_v1%2Ff_auto%2Cq_auto%3Aeco%2Fb99930d3-563a-49ee-8460-54b618dd52ea%2Fair-max-plus-shoes-2cd0sW.png&imgrefurl=https%3A%2F%2Fwww.nike.com%2Fbg%2Ft%2Fair-max-plus-shoes-2cd0sW&tbnid=brXfaiM8OeKqAM&vet=12ahUKEwjmv7mJqIL3AhVuiP0HHeJRB8sQMygFegUIARDOAQ..i&docid=TAWkEYbIt93YAM&w=1280&h=1600&q=nike%20air%20max&ved=2ahUKEwjmv7mJqIL3AhVuiP0HHeJRB8sQMygFegUIARDOAQ";


        public static Load GetLoad(
            string id=LoadTestId,
            bool userSame=true,
            bool IsDeleted = false,
            bool IsPublic=true)
        {

            var user = new User 
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };


            var trader = new Trader
            {
                
                Name = TestUser.Username,               
                User= userSame ? user : new User
                {
                    Id="DifferentId",
                    UserName="DifferentName"
                },
                UserId= userSame? TestUser.Identifier : "DifferentId"
            };

            return new Load
            {
                Id = id,
                Title = "Title",
                IsPublic = IsPublic,
                IsDeleted = IsDeleted,
                Quantity=5,
                Trader = trader,
                Kind = new Kind
                {
                    Id = 1,
                    Name = "TestKind",



                },
                SubKind = new SubKind
                {
                    Id=1,
                    Name = "TestSubKind",
                    Kind=new Kind
                    {
                        Id=2,
                        Name = "TestKind"
                    },
                    KindId=2
                },
                Images=new List<LoadImage>()
                {
                    new LoadImage()
                    {
                        ImageUrl="TestUrl"
                    }

                }

            };
        }
 

        public static List<Load> GetLoads(int count=5,bool IsDeleted=false,bool sameUser=true)     
        {
              var dealer = new Trader
              {
                Name = TestUser.Username,
                UserId = TestUser.Identifier,
              };

            var loads = Enumerable
             .Range(1, count)
             .Select(i => new Load
             {
                 IsPublic = !IsDeleted ,
                 IsDeleted = IsDeleted ,
                 DeletedOn = IsDeleted ? new System.DateTime(1, 1, 1) : null,
                 Trader = sameUser ? dealer : new Trader
                  {
                      Id = $"Author Id {i}",
                      Name = $"Author {i}"
                  },

             })
             .ToList();


            return loads;

         }

        
    } 
}
