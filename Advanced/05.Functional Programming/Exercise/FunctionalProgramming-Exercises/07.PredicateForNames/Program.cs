using System;
using System.Linq;


namespace _07.PredicateForNames
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            string[] originalNames = Console.ReadLine().Split();

            Predicate<string> isLength = s => s.Length <= n;
            Func<string[], string[]> getNames = name => originalNames.Where(s => isLength(s)).ToArray();
            Action<string[]> printNames = names => Console.WriteLine(string.Join(Environment.NewLine, names));

            string[] newNames = getNames(originalNames);
            printNames(newNames);
        }
    }
}
