using System;
using System.Linq;


namespace _07.BinarySearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int[] arr = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            int key = int.Parse(Console.ReadLine());

            int index = BinarySearch.IndexOf(arr, key);
            Console.WriteLine(index);
        }
    }
}
