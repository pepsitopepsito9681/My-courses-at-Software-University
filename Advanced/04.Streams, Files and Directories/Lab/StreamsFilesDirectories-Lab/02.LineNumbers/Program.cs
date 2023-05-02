using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.LineNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine("Files", "Input.txt");
            var reader = new StreamReader(filePath);

            using (reader)
            {
                var writer = new StreamWriter(Path.Combine("Files", "Output.txt"));

                string line = reader.ReadLine();
                int counter = 1;

                using (writer)
                {
                    while (line != null)
                    {
                        writer.WriteLine($"{counter++}. {line}");
                        line = reader.ReadLine();
                    }
                }
            }
        }
    }
}
