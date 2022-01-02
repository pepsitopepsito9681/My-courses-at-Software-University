namespace Vehicles
{
    public class Car : Vehicle
    {
        private const double CarAirConditionerModifier = 0.9;
        public Car(double fuel, double fuelConsumption)
            : base(fuel, fuelConsumption, CarAirConditionerModifier)
        {
        }
    }
}
