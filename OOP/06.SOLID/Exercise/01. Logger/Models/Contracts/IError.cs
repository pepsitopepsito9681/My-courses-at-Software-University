using _01._Logger.Models.Enumerations;
using System;

namespace _01._Logger.Models.Contracts
{
    public interface IError
    {
        DateTime DateTime { get; }
        string Message { get; }
        Level Level { get; }
    }
}