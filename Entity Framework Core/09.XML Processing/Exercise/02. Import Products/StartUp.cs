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

            //var userXml = File.ReadAllText("../../../Datasets/users.xml");
            var productXml = File.ReadAllText("../../../Datasets/products.xml");

            //ImportUsers(context, userXml);
            var result = ImportProducts(context, productXml);

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

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Products";
            var productsDto = XMLConverter.Deserializer<ImportProductDto>(inputXml, rootElement);

            var products = productsDto.Select(p => new Product
            {
                Name = p.Name,
                Price = p.Price,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId
            })
                .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
    }
}