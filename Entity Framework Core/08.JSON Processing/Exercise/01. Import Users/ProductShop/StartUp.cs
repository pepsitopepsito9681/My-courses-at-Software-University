﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Dtos.Input;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var contex = new ProductShopContext();
            contex.Database.EnsureDeleted();
            contex.Database.EnsureCreated();

            string usersJasonString = File.ReadAllText("../../../Datasets/users.json");
            Console.WriteLine(ImportUsers(contex, usersJasonString));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(mapperConfiguration);

            IEnumerable<User> mappedUsers = mapper.Map<IEnumerable<User>>(users);
            //IEnumerable<User> mappedUsers = users
            //    .Select(x => x.MapToDomainUser())
            //    .ToList();
            context.Users.AddRange(mappedUsers);
            context.SaveChanges();

            return $"Successfully imported {mappedUsers.Count()}";
        }
    }

    public static class UserMappings
    {
        //public static User MapToDomainUser(this UserInputDto userDto)
        //{
        //    return new User
        //    {
        //        Age = userDto.Age,
        //        FirstName = userDto.FirstName,
        //        LastName = userDto.LastName
        //    };
        //}
    }
}