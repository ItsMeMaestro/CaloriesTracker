/* 
 This is a class instance for products.
 Each product has a name and picture visible, the rest is used for filtering searching and collecting data
*/
using System;

namespace CaloriesTracker
{
    public class Product
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        // Additional properties for nutritional information
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public double WeightInGrams { get; set; }
      

        public override string ToString()
        {
            return Name;
        }
    }
}
