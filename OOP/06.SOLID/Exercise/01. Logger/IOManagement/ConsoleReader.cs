using _01._Logger.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.IOManagement
{
    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
