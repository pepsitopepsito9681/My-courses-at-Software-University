﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EfCoreDemo.Models
{
    public class EmployeeInClub
    {
       // public int Id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int ClubId { get; set; }

        public Club Club { get; set; }

       // public DateTime JoinDate { get; set; }
    }
}
