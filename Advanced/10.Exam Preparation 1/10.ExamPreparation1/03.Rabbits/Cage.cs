using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _03.Rabbits
{
    public class Cage
    {
        private List<Rabbit> data;

        public Cage(string name, int capacity)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.data = new List<Rabbit>();
        }

        public string Name { get; private set; }

        public int Capacity { get; private set; }

        public int Count => this.data.Count;

        public void Add(Rabbit rabbit)
        {
            if (this.data.Count < this.Capacity)
            {
                this.data.Add(rabbit);
            }
        }

        public bool RemoveRabbit(string name)
        {
            return this.data.Remove(this.data.FirstOrDefault(x => x.Name == name));
        }

        public void RemoveSpecies(string species)
        {
            this.data.RemoveAll(x => x.Species == species);
        }

        public Rabbit SellRabbit(string name)
        {
            var soldRabbit = this.data.FirstOrDefault(x => x.Name == name);
            if (soldRabbit != null)
            {
                soldRabbit.Available = false;
            }
            return soldRabbit;
        }

        public Rabbit[] SellRabbitsBySpecies(string species)
        {
            var soldRabbits = this.data.Where(x => x.Species == species).ToList();
            for (int i = 0; i < soldRabbits.Count; i++)
            {
                soldRabbits[i].Available = false;
            }
            return soldRabbits.ToArray();
        }

        public string Report()
        {
            var notSoldRabbits = this.data.Where(x => x.Available == true);
            var sb = new StringBuilder($"Rabbits available at {this.Name}:{Environment.NewLine}");
            foreach (var rabbit in notSoldRabbits)
            {
                sb.AppendLine(rabbit.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
