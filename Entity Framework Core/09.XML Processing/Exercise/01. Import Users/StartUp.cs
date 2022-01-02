using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using XmlFacade;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using ProductShopContext context = new ProductShopContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var userXml = File.ReadAllText("../../../Datasets/users.xml");

            var result = ImportUsers(context, userXml);

            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Users";
            var usersResult = XMLConverter.Deserializer<ImportUserDto>(inputXml, rootElement);

            //List<User> users = new List<User>();
            //foreach (var importUserDto in usersResult)
            //{
            //    var user = new User()
            //    {
            //        FirstName = importUserDto.FirstName,
            //        LastName = importUserDto.LastName,
            //        Age = importUserDto.Age
            //    };
            //    users.Add(user);
            //}
            var users = usersResult
                .Select(u => new User
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age
                })
                .ToArray();

            context.Users.AddRange(users);
            context.SaveChanges();
            //return $"Successfully imported {users.Count}";
            return $"Successfully imported {users.Length}";
        }
    }
}