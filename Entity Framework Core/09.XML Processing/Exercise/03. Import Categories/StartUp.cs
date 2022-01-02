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
            //var productXml = File.ReadAllText("../../../Datasets/products.xml");
            var categoryXml = File.ReadAllText("../../../Datasets/categories.xml");

            //ImportUsers(context, userXml);
            //ImportProducts(context, productXml);
            var result = ImportCategories(context, categoryXml);

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

            //var products = new List<Product>();

            //foreach (var productDto in productsDto)
            //{
            //    if (!context.Users.Any(x => x.Id == productDto.SellerId && x.Id == productDto.BuyerId))
            //    {
            //        continue;
            //    }

            //    var product = new Product
            //    {
            //        Name = productDto.Name,
            //        Price = productDto.Price,
            //        SellerId = productDto.SellerId,
            //        BuyerId = productDto.BuyerId
            //    };
            //    products.Add(product);
            //}
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
           // return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Categories";
            var categoriesDto = XMLConverter.Deserializer<ImportCategoryDto>(inputXml, rootElement);
            List<Category> categories = new List<Category>();

            foreach (var dto in categoriesDto)
            {
                if (dto.Name == null)
                {
                    continue;
                }

                var category = new Category
                {
                    Name = dto.Name
                };
                categories.Add(category);
            }
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
    }
}