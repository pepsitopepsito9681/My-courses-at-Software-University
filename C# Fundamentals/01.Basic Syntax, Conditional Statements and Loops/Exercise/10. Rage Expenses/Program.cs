using System;


namespace _10.Rage_Expenses
{
    class Program
    {
        static void Main(string[] args)
        {
            int lostGame = int.Parse(Console.ReadLine());
            double headsetPrice = double.Parse(Console.ReadLine());
            double mousePrice = double.Parse(Console.ReadLine());
            double keyboardPrice = double.Parse(Console.ReadLine());
            double displayPrice = double.Parse(Console.ReadLine());

            int trashedHeadSet = 0;
            int trashedMouse = 0;
            int trashedKeyboard = 0;
            int trashedDisplay = 0;

            for (int i = 1; i <= lostGame; i++)
            {
                if (i % 2 == 0)
                {
                    trashedHeadSet += 1;
                }
                if (i % 3 == 0)
                {
                    trashedMouse += 1;
                }
                if (i % 6 == 0)
                {
                    trashedKeyboard += 1;
                }
                if (i % 12 == 0)
                {
                    trashedDisplay += 1;
                }
            }
            double rageExpenses = trashedDisplay * displayPrice +
                                  trashedMouse * mousePrice +
                                  trashedHeadSet * headsetPrice +
                                  trashedKeyboard * keyboardPrice;
            Console.WriteLine("Rage expenses: {0:f2} lv.",rageExpenses);
        }
    }
}
