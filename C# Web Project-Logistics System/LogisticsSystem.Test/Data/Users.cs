﻿using LogisticsSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    public class Users
    {
        public const string TelephoneNumber = "0888888888";

        public static IEnumerable<User> GetUsers(int count=5)
        => Enumerable.Range(0, count).Select(i => new User()
        {
           
        });

    }
}
