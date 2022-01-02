using System;


namespace _11.Multiplication_Table_2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int m = int.Parse(Console.ReadLine());

            if (n > 10 || m > 10)
            {
                Console.WriteLine($"{0} X {1} = {2}",n,m,n*m);
            }
            else
            {
                for (int i = m; i <= 10; i++)
                {
                    Console.WriteLine($"{0} X {1} = {2}",n,i,n*i);
                }
            }
        }
    }
}
