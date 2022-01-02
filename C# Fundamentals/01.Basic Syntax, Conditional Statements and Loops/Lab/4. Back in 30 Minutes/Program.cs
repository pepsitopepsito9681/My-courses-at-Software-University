using System;


namespace _4.Back_in_30_Minutes
{
    class Program
    {
        static void Main(string[] args)
        {
            int hour = int.Parse(Console.ReadLine());
            int minutes = int.Parse(Console.ReadLine()) + 30;

            if (minutes > 59)
            {
                hour += 1;
                minutes -= 60;
            }

            if (hour > 23)
            {
                hour = 0;
            }
            Console.WriteLine("{0}:{1:D2}",hour,minutes);
        }
    }
}
