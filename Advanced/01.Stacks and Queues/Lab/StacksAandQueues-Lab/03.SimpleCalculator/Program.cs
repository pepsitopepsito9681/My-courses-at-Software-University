using System;
using System.Collections.Generic;
using System.Linq;


namespace _03.SimpleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            string[] expression = input.Split(new[] { " " },StringSplitOptions.RemoveEmptyEntries);
            Stack<string> stack = new Stack<string>(expression.Reverse());

            while (stack.Count > 1)
            {
                int operand1 = Int32.Parse(stack.Pop());
                string opr = stack.Pop();
                int operand2 = Int32.Parse(stack.Pop());

                switch (opr)
                {
                    case "+":
                        stack.Push($"{operand1 + operand2}");
                        break;
                    case "-":
                        stack.Push($"{operand1 - operand2}");
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine(stack.Pop());
        }
    }
}
