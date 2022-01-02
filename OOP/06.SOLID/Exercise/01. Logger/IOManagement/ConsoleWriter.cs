using _01._Logger.Models.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.Models.IOManagement
{
    public class ConsoleWriter : IWriter
    {
        public void Write(string text)
        {
            Console.Write(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
