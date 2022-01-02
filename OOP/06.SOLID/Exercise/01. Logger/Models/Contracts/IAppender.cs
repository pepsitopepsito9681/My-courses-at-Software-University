using _01._Logger.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.Models.Contracts
{
    public interface IAppender
    {
        ILayout Layout { get; }
        Level Level { get; }
        void Append(IError error);

    }
}
