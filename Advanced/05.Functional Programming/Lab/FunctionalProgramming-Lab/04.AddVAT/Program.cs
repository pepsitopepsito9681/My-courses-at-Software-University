﻿using System;
using System.Linq;


namespace _04.AddVAT
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<double, double> unaryOperator = n => n * 1.2;

            double[] nums = Console.ReadLine()
                .Split(new[] { ", " },StringSplitOptions.RemoveEmptyEntries)
                .Select(double.Parse)
                .Select(unaryOperator)
                .ToArray();

            foreach (var num in nums)
            {
                Console.WriteLine($"{num:F2}");
            }
        }
    }
}
