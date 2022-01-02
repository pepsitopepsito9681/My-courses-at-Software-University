namespace Vehicles
{
    public class Truck: Vehicle 
    {
        private const double TruckAirConditionerModifier = 1.6;

        public Truck(double fuel, double fuelConsumption) 
            : base(fuel, fuelConsumption, TruckAirConditionerModifier)
        {
        }

        public override void Refuel(double amount)
        {
            base.Refuel(amount*0.95);
        }
    }
}
