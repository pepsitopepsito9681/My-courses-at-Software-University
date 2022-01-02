using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.Models.IOManagement.Contracts
{
   public interface IWriter
    {
        void Write(string text);

        void WriteLine(string text);
    }
}
