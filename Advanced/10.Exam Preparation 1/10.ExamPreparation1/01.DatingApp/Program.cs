using System;
using System.Collections.Generic;
using System.Linq;


namespace _01.DatingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var males = new Stack<int>(Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            var females = new Queue<int>(Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            int matches = 0;
            while (males.Count > 0 && females.Count > 0)
            {
                var currentMale = males.Peek();
                var currentFemale = females.Peek();
                if (currentFemale <= 0)
                {
                    females.Dequeue();
                    continue;
                }
                if (currentMale <= 0)
                {
                    males.Pop();
                    continue;
                }
                if (currentFemale % 25 == 0)
                {
                    females.Dequeue();
                    if (females.Count > 0)
                    {
                        females.Dequeue();
                    }
                    continue;
                }
                if (currentMale % 25 == 0)
                {
                    males.Pop();
                    if (males.Count > 0)
                    {
                        males.Pop();
                    }
                    continue;
                }
                if (currentMale == currentFemale)
                {
                    matches++;
                    males.Pop();
                    females.Dequeue();
                }
                else
                {
                    males.Push(males.Pop() - 2);
                    females.Dequeue();
                }
            }

            Console.WriteLine($"Matches: {matches}");
            if (males.Count == 0)
            {
                Console.WriteLine("Males left: none");
            }
            else
            {
                Console.WriteLine($"Males left: {string.Join(", ", males)}");
            }

            if (females.Count == 0)
            {
                Console.WriteLine("Females left: none");
            }
            else
            {
                Console.WriteLine($"Females left: {string.Join(", ", females)}");
            }
        }
    }
}
