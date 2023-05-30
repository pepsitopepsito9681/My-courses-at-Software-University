﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05.CountSymbols
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            Dictionary<char, int> symbols = new Dictionary<char, int>();

            foreach (var symbol in input)
            {
                if (!symbols.ContainsKey(symbol))
                {
                    symbols.Add(symbol, 0);
                }

                symbols[symbol]++;
            }

            foreach (var symbol in symbols.OrderBy(s => s.Key))
            {
                Console.WriteLine($"{symbol.Key}: {symbol.Value} time/s");
            }
        }
    }
}
