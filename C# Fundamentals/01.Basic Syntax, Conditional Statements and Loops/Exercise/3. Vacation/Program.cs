using System;


namespace _3.Vacation
{
    class Program
    {
        static void Main(string[] args)
        {
            int groupOfPeople = int.Parse(Console.ReadLine());
            string groupType = Console.ReadLine();
            string day = Console.ReadLine();
            double price = 0;

            switch (groupType)
            {
                case "Students":
                    switch (day)
                    {
                        case "Friday":
                            price = groupOfPeople * 8.45;
                            break;
                        case "Saturday":
                            price = groupOfPeople * 9.80;
                            break;
                        case "Sunday":
                            price = groupOfPeople * 10.46;
                            break;
                    }
                    break;
                case "Business":
                    switch (day)
                    {
                        case "Friday":
                            price = groupOfPeople * 10.90;
                            break;
                        case "Saturday":
                            price = groupOfPeople * 15.60;
                            break;
                        case "Sunday":
                            price = groupOfPeople * 16;
                            break;
                    }
                    break;
                case "Regular":
                    switch (day)
                    {
                        case "Friday":
                            price = groupOfPeople * 15;
                            break;
                        case "Saturday":
                            price = groupOfPeople * 20;
                            break;
                        case "Sunday":
                            price = groupOfPeople * 22.50;
                            break;
                    }
                    break;
            }

            if (groupType == "Students" && groupOfPeople >= 30)
            {
                price *= 0.85;
            }
            else if (groupType == "Business" && groupOfPeople >= 100)
            {
                price = price - (price / groupOfPeople) * 10;
            }
            else if (groupType == "Regular" && (groupOfPeople >= 10 && groupOfPeople <= 20))

            {
                price *= 0.95;
            }
            Console.WriteLine("Total price: {0:f2}",price);
        }
    }
}
