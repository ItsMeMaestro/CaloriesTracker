using System;

namespace CaloriesTracker
{
    public class Product
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        // Additional properties for nutritional information
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }

        // Add other properties as needed

        public override string ToString()
        {
            return Name;
        }
    }
}
