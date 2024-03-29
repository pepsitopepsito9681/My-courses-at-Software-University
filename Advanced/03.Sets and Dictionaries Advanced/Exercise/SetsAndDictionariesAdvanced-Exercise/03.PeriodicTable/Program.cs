﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.PeriodicTable
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            HashSet<string> elements = new HashSet<string>();

            for (int i = 0; i < n; i++)
            {
                string[] chemicalCompounds = Console.ReadLine().Split();
                foreach (var compound in chemicalCompounds)
                {
                    elements.Add(compound);
                }
            }

            Console.WriteLine(string.Join(" ", elements.OrderBy(e => e)));
        }
    }
}
