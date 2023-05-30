using System;
using System.Linq;


namespace _03.Rabbits
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ////Initialize the repository (Cage)
            //Cage cage = new Cage("Wildness", 20);
            ////Initialize entity
            //Rabbit rabbit = new Rabbit("Fluffy", "Blanc de Hotot");
            ////Print Rabbit
            //Console.WriteLine(rabbit); //Rabbit (Blanc de Hotot): Fluffy

            ////Add Rabbit
            //cage.Add(rabbit);
            //Console.WriteLine(cage.Count); //1
            //                               //Remove Rabbit
            //cage.RemoveRabbit("Rabbit Name"); //false
            //Console.WriteLine(cage.RemoveRabbit("Rabbit Name"));
            //Rabbit secondRabbit = new Rabbit("Bunny", "Brazilian");
            //Rabbit thirdRabbit = new Rabbit("Jumpy", "Cashmere Lop");
            //Rabbit fourthRabbit = new Rabbit("Puffy", "Cashmere Lop");
            //Rabbit fifthRabbit = new Rabbit("Marlin", "Brazilian");

            ////Add Rabbits
            //cage.Add(secondRabbit);
            //cage.Add(thirdRabbit);
            //cage.Add(fourthRabbit);
            //cage.Add(fifthRabbit);

            ////Sell Rabbit by name
            //Console.WriteLine(cage.SellRabbit("Bunny")); //Rabbit (Brazilian): Bunny
            //                                             //Sell Rabbit by species
            //Rabbit[] soldSpecies = cage.SellRabbitsBySpecies("Cashmere Lop");
            //Console.WriteLine(string.Join(", ", soldSpecies.Select(f => f.Name))); //Jumpy, Puffy

            //Console.WriteLine(cage.Report());
            ////Rabbits available at Wildness:
            ////Rabbit (Blanc de Hotot): Fluffy
            ////Rabbit (Brazilian): Marlin

            var cage = new Cage("FirstCage", 5);
            //Console.WriteLine(cage.SellRabbit("null"));
            var soldrabb = cage.SellRabbitsBySpecies("null");
            //Console.WriteLine(string.Join(", ", soldrabb.ToList()));
            //Console.WriteLine(cage.RemoveRabbit("nu;;"));
            //cage.RemoveSpecies("alabala");
            var rabbit1 = new Rabbit("Gosho", "Bql");
            var rabbit2 = new Rabbit("Gosho", "Bql");
            var rabbit3 = new Rabbit("Gosho", "Cheren");
            var rabbit4 = new Rabbit("Gosho", "Cheren");
            var rabbit5 = new Rabbit("Gosho", "Bql");
            var rabbit6 = new Rabbit("Gosho", "Bql");
            cage.Add(rabbit1);
            Console.WriteLine(cage.SellRabbit("Gosho"));
            Console.WriteLine(cage.RemoveRabbit("Gosho"));
            cage.Add(rabbit2);
            cage.Add(rabbit3);
            cage.Add(rabbit4);
            cage.Add(rabbit5);
            cage.Add(rabbit6);
            Console.WriteLine(cage.Report());
            Console.WriteLine();
            Console.WriteLine(cage.RemoveRabbit("Tanq"));
            Console.WriteLine(cage.RemoveRabbit("Mimi"));
            Console.WriteLine(cage.Report());
            Console.WriteLine();
            cage.RemoveSpecies("cherven");
            Console.WriteLine(cage.Report());
            Console.WriteLine();
            cage.RemoveSpecies("Cheren");
            Console.WriteLine(cage.Report());
            Console.WriteLine();
            cage.Add(rabbit6);
            Console.WriteLine(cage.SellRabbit("Tania"));
            Console.WriteLine(cage.SellRabbit("Tanq"));
            Console.WriteLine(cage.SellRabbit("Gosho"));
            Console.WriteLine(rabbit6.Available);
            Console.WriteLine();

            var soldRabbitsFalse = cage.SellRabbitsBySpecies("Bel");
            var soldRabbits = cage.SellRabbitsBySpecies("Bql");
            foreach (var sold in soldRabbits)
            {
                Console.WriteLine(sold.Available);
            }
            Console.WriteLine(string.Join("; ", soldRabbits.Select(x => x.Name)));
            Console.WriteLine(cage.SellRabbit("Tanq"));

            Console.WriteLine(cage.Count);
            Console.WriteLine(cage.Report());


        }
    }

}
