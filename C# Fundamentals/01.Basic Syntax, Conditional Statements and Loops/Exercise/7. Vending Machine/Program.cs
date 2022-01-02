using System;


namespace _7.Vending_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            double balance = 0;
            while (input != "Start")
            {
                double currentCoin = double.Parse(input);
                if (currentCoin == 0.1 ||
                    currentCoin == 0.2 ||
                    currentCoin == 0.5 ||
                    currentCoin == 1 ||
                    currentCoin == 2)
                {
                    balance += currentCoin;
                }
                else
                {
                    Console.WriteLine($"Cannot accept {currentCoin}");
                }
                input = Console.ReadLine();
            }
            input = Console.ReadLine();
            while (input != "End")
            {
                double price = 0;
                if (input == "Nuts")
                {
                    price = 2.0;
                }
                else if (input == "Water")
                {
                    price = 0.70;
                }
                else if (input == "Crisps")
                {
                    price = 1.50;
                }
                else if (input == "Soda")
                {
                    price = 0.8;
                }
                else if (input == "Coke")
                {
                    price = 1.0;
                }
                if (price != 0)
                {
                    if (balance >= price)
                    {
                        Console.WriteLine($"Purchased {input.ToLower()}");
                        balance -= price;
                    }
                    else
                    {
                        Console.WriteLine("Sorry, not enough money");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid product");
                }
                input = Console.ReadLine();
            }
            Console.WriteLine("Change: {0:f2}",balance);
        }
    }
}
