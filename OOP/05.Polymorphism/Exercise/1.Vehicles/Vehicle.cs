using System;

namespace Vehicles
{
    public abstract class Vehicle
    {
        protected Vehicle(double fuel, double fuelConsumption, double airConditionerModifier)
        {
            this.Fuel = fuel;
            this.FuelConsumption = fuelConsumption;
            this.AirConditionerModifier = airConditionerModifier;
        }

        private double AirConditionerModifier { get; set; }

        public double Fuel { get; private set; }

        public double FuelConsumption { get; private set; }

        public void Drive(double distance)
        {
            double requiredFuel = (this.FuelConsumption + AirConditionerModifier) * distance;

            if (requiredFuel > this.Fuel)
            {
                throw new InvalidOperationException($"{this.GetType().Name} needs refueling");
            }

            this.Fuel -= requiredFuel;
        }

        public virtual void Refuel(double amount)
        {
            this.Fuel += amount;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: {this.Fuel:F2}";
        }
    }
}
