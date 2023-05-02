using System;
using System.Linq;


namespace _05.MergeSort
{
    class Program
    {
        public static void Main(string[] args)
        {
            var numbers = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            MergeSort<int>.Sort(numbers);

            Console.WriteLine(string.Join(" ", numbers));
        }
    }
}
