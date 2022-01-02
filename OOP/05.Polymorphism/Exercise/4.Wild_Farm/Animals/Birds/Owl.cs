using System.Collections.Generic;
using WildFarm.Foods;

namespace WildFarm.Animals.Birds
{
    public class Owl:Bird
    {
        private const double BaseWeightModifier = 0.25;
        private static HashSet<string> allowedFoods = new HashSet<string>()
        {
            nameof (Meat)
        };

        public Owl(
            string name, 
            double weight,
            double wingSize) 
            : base(name, weight, allowedFoods, BaseWeightModifier, wingSize)
        {
        }

        public override string ProduceSound()
        {
            return $"Hoot Hoot";
        }
    }
}
