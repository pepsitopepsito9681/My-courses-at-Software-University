using System;

namespace VehiclesExtension
{
    public class Bus : Vehicle
    {
        private const double BusAirConditionModifier = 1.4;

        public Bus(double fuel, double fuelConsumption, double tankCapacity)
            : base(fuel, fuelConsumption, tankCapacity, BusAirConditionModifier)
        {
        }
        public void DriveEmpty(double distance)
        {
            double requiredFuel = this.FuelConsumption * distance;

            if (requiredFuel > this.Fuel)
            {
                throw new InvalidOperationException($"{this.GetType().Name} needs refueling");
            }

            this.Fuel -= requiredFuel;
        }
    }
}
