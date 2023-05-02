using System;
using System.Collections.Generic;


namespace _06.Supermarket
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            var queue = new Queue<string>();
            int counter = 0;
            string[] array = { " " };
            while (true)
            {

                queue.Enqueue(input);
                input = Console.ReadLine();
                counter++;

                if (input == "Paid")
                {
                    array = queue.ToArray();
                    queue.Clear();
                }

                else if (input == "End")
                {
                    break;
                }

            }
            for (int i = 0; i < queue.Count; i++)
            {
                Console.WriteLine(array[i]);

            }

            Console.WriteLine($"{counter} people remaining");
        }
    }
}
