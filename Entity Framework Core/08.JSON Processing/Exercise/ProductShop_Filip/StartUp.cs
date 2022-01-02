using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Dtos.Input;
using ProductShop.Dtos.Output;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string usersJsonAsString = File.ReadAllText("../../../Datasets/users.json");
            //string productsJsonAsString = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJsonAsString = File.ReadAllText("../../../Datasets/categories.json");
            //string categoryProductsJsonAsString = File.ReadAllText("../../../Datasets/categories-products.json");

            //Console.WriteLine(ImportUsers(context, usersJsonAsString));
            //Console.WriteLine(ImportProducts(context, productsJsonAsString));
            //Console.WriteLine(ImportCategories(context, categoriesJsonAsString));
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductsJsonAsString));

            Console.WriteLine(GetProductsInRange(context));
            Console.WriteLine(GetSoldProducts(context));
            Console.WriteLine(GetCategoriesByProductsCount(context));
            Console.WriteLine(GetUsersWithProducts(context));

        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);
            InitializeMapper();

            var mappedUsers = mapper.Map<IEnumerable<User>>(users);

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();

            return $"Successfully imported {mappedUsers.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<ProductInputDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductInputDto>>(inputJson);
            InitializeMapper();

            var mappedProducts = mapper.Map<IEnumerable<Product>>(products);
            context.Products.AddRange(mappedProducts);
            context.SaveChanges();

            return $"Successfully imported {mappedProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryInputDto> categories =
                JsonConvert.DeserializeObject<IEnumerable<CategoryInputDto>>(inputJson)
                .Where(x => !string.IsNullOrEmpty(x.Name));

            InitializeMapper();
            var mappedCategories = mapper.Map<IEnumerable<Category>>(categories);

            context.Categories.AddRange(mappedCategories);
            context.SaveChanges();

            return $"Successfully imported {mappedCategories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryProductInputDto> categoryProducts =
               JsonConvert.DeserializeObject<IEnumerable<CategoryProductInputDto>>(inputJson);

            InitializeMapper();
            var mappedCategoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoryProducts);

            context.CategoryProducts.AddRange(mappedCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {mappedCategoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            InitializeMapper();

            // example 1 --> anonymous type example
            //var queryResult = context
            //    .Products
            //    .Where(x => x.Price >= 500 && x.Price <= 1000)
            //    .OrderBy(x => x.Price)
            //    .Select(x => new  // <--- anonymous type 
            //    {
            //        Name = x.Name,
            //        Price = x.Price,
            //        Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
            //    })
            //    .ToList();
            //var result = mapper.Map<List<ProductOutputDto>>(queryResult);
            // ===================================================================

            // example 2 --> Dto 
            //var result = context
            //   .Products
            //   .Where(x => x.Price >= 500 && x.Price <= 1000)
            //   .OrderBy(x => x.Price)
            //   .Select(x => new ProductOutputDto // <--- Dto ( non anonymous type) 
            //    {
            //       Name = x.Name,
            //       Price = x.Price,
            //       Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
            //   })
            //   .ToList();
            // ===================================================================

            // exampel 3 --> ProjectTo<>
            var result = context
                .Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .ProjectTo<ProductOutputDto>(mapper.ConfigurationProvider) // <--- mapper profile configurations
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string productsAsJson = JsonConvert.SerializeObject(result, jsonSettings);

            return productsAsJson;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {

            InitializeMapper();

            // example 1 --> anonymous type example
            //var queryResult = context
            //   .Users
            //   .Include(p => p.ProductsSold)
            //   .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
            //   .OrderBy(x => x.LastName)
            //   .ThenBy(x => x.FirstName)
            //   .Select(x => new
            //   {
            //       FirstName = x.FirstName,
            //       LastName = x.LastName,
            //       SoldProducts = x.ProductsSold
            //       .Select(p => new
            //       {
            //           Name = p.Name,
            //           Price = p.Price,
            //           BuyerFirstName = p.Buyer.FirstName,
            //           BuyerLastName = p.Buyer.LastName
            //       })
            //       .ToList()
            //   })
            //   .ToList();
            //var result = mapper.Map<List<UserSoldProductsOutputDto>>(queryResult);
            // ===================================================================

            // example 2 --> Dto 
            //var result = context
            //   .Users
            //   .Include(p => p.ProductsSold)
            //   .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
            //   .OrderBy(x => x.LastName)
            //   .ThenBy(x => x.FirstName)
            //   .Select(x => new UserSoldProductsOutputDto
            //   {
            //       FirstName = x.FirstName,
            //       LastName = x.LastName,
            //       SoldProducts = x.ProductsSold
            //       .Select(p => new SoldProdcutOutputDto
            //       {
            //           Name = p.Name,
            //           Price = p.Price,
            //           BuyerFirstName = p.Buyer.FirstName,
            //           BuyerLastName = p.Buyer.LastName
            //       })
            //       .ToList()
            //   })
            //   .ToList();
            // ===================================================================

            // example 3 --> ProjectTo<>
            var result = context
                .Users
                .Include(p => p.ProductsSold)
                .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ProjectTo<UserSoldProductsOutputDto>(mapper.ConfigurationProvider)
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string productsAsJson = JsonConvert.SerializeObject(result, jsonSettings);

            return productsAsJson;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            InitializeMapper();
            // example 1 --> anonymous type example
            //var queryResult = context
            //   .Categories
            //   .OrderByDescending(x => x.CategoryProducts.Count)
            //   .Select(x => new
            //   {
            //       Category = x.Name,
            //       ProductsCount = x.CategoryProducts.Count,
            //       AveragePrice = $"{(x.CategoryProducts.Sum(cp => cp.Product.Price) / x.CategoryProducts.Count):F2}",
            //       TotalRevenue = $"{x.CategoryProducts.Sum(cp => cp.Product.Price):F2}"
            //   })
            //   .ToList();
            //var result = mapper.Map<List<CategoryProductsOutputDto>>(queryResult);

            // ===================================================================
            // example 2 --> Dto 
            //var result = context
            //  .Categories
            //  .OrderByDescending(x => x.CategoryProducts.Count)
            //  .Select(x => new CategoryProductsOutputDto
            //  {
            //      Category = x.Name,
            //      ProductsCount = x.CategoryProducts.Count,
            //      AveragePrice = $"{(x.CategoryProducts.Sum(cp => cp.Product.Price) / x.CategoryProducts.Count):F2}",
            //      TotalRevenue = $"{x.CategoryProducts.Sum(cp => cp.Product.Price):F2}"
            //  })
            //  .ToList();
            // ===================================================================

            // exampel 3 --> ProjectTo<>
            var result = context
              .Categories
              .OrderByDescending(x => x.CategoryProducts.Count)
              .ProjectTo<CategoryProductsOutputDto>(mapper.ConfigurationProvider)
              .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string resultsAsJson = JsonConvert.SerializeObject(result, jsonSettings);

            return resultsAsJson;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            InitializeMapper();
            //example 1-- > anonymous type example
            //var queryResult = context.Users
            //    .Include(x => x.ProductsSold)
            //    .ToList() // --> ONLY BECAUSE OF JUDGE
            //    .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
            //    .Select(x => new
            //    {
            //        FirstName = x.FirstName,
            //        LastName = x.LastName,
            //        Age = x.Age,
            //        SoldProducts = new
            //        {
            //            Count = x.ProductsSold
            //            .Where(p => p.Buyer != null)
            //            .Count(),

            //            Products = x.ProductsSold
            //            .Where(p => p.Buyer != null)
            //            .Select(p => new
            //            {
            //                Name = p.Name,
            //                Price = p.Price,
            //            })
            //            .ToList()
            //        }
            //    })
            //    .OrderByDescending(x => x.SoldProducts.Count)
            //    .ToList();
            //var mappedResult = mapper.Map<List<UserProductsOutputDto>>(queryResult);
            //var result = new UsersWithSoldProductsOutputDto
            //{
            //    UsersCount = mappedResult.Count(),
            //    Users = mappedResult
            //};
            var users = context.Users
                 .Include(x => x.ProductsSold)
                 .ToList()
                 .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                 .Select(x => new UserProductsOutputDto
                 {
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     Age = x.Age,
                     SoldProducts = new ProductsOutputDto
                     {
                         Count = x.ProductsSold
                         .Where(p => p.Buyer != null)
                         .Count(),

                         Products = x.ProductsSold
                         .Where(p => p.Buyer != null)
                         .Select(p => new ProductOutputDto
                         {
                             Name = p.Name,
                             Price = p.Price,
                         })
                         .ToList()
                     }
                 })
                 .OrderByDescending(x => x.SoldProducts.Count)
                 .ToList();

            var result = new UsersWithSoldProductsOutputDto
            {
                UsersCount = users.Count(),
                Users = users
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            };
            string resultAsJson = JsonConvert
               .SerializeObject(result,
               jsonSettings);

            return resultAsJson;
        }

        private static void InitializeMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            mapper = new Mapper(mapperConfiguration);
        }
    }
}