using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.Models.Contracts
{
    public interface ILayout
    {
        string Format { get; }
    }
}
