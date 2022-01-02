using _01._Logger.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.Models 
{
    public class SimpleLayout : ILayout
    {
        public string Format => "{0} - {1} - {2}"; // date time format
    }
}
