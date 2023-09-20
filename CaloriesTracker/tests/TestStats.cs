using CaloriesTracker;

class Test
{
    static public void Main()
    {
        
        List<Product> products = new List<Product>();
        List<double> results = new List<double>();
        
        Product product1 = new Product();
        Product product2 = new Product();
        product1.Calories = 200;
        product1.WeightInGrams = 100;
        product1.Fats = 10;
        product1.Carbs = 20;
        product1.Proteins = 10;
        products.Add(product1);
        product2.Calories = 200;
        product2.WeightInGrams = 100;
        product2.Fats = 300;
        product2.Carbs = 20;
        product2.Proteins = 10;
        products.Add(product2);

        results = StatsMath(products);
        for(int i=0;i<4;i++)
        {
            Console.WriteLine(results[i]);
        }
        
    }
    static public List<double> StatsMath(List<Product> products)
    {
        List<double> results = new List<double>();
        double averageDailyCalories=0;
        double averageDailyProteins=0;
        double averageDailyCarbs=0;
        double averageDailyFats=0;
        results.Add(averageDailyCalories);
        results.Add(averageDailyProteins);
        results.Add(averageDailyCarbs);
        results.Add(averageDailyFats);
        Dictionary<DateTime, (double Calories, double Proteins, double Carbs, double Fats)> dailyTotals =
                new Dictionary<DateTime, (double, double, double, double)>();

        
        foreach (var product in products)
        {
            // Calculate the nutrient values based on weight
            double weightedCalories = (product.Calories / 100.0) * product.WeightInGrams;
            double weightedProteins = (product.Proteins / 100.0) * product.WeightInGrams;
            double weightedCarbs = (product.Carbs / 100.0) * product.WeightInGrams;
            double weightedFats = (product.Fats / 100.0) * product.WeightInGrams;

            // Create a date with the time component set to midnight (removing the time component)
            //DateTime productDate = new DateTime(2023, 9, 20, 14, 30, 0);
            DateTime productDate = DateTime.Now;

            // Update the daily totals dictionary
            if (!dailyTotals.ContainsKey(productDate))
            {
                dailyTotals[productDate] = (0, 0, 0, 0);
            }

            var (dailyCalories, dailyProteins, dailyCarbs, dailyFats) = dailyTotals[productDate];

            dailyTotals[productDate] = (
                dailyCalories + weightedCalories,
                dailyProteins + weightedProteins,
                dailyCarbs + weightedCarbs,
                dailyFats + weightedFats
            );

            // Filter out days with zero calorie sum
            dailyTotals = dailyTotals
                .Where(kv => kv.Value.Calories > 0)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            // Calculate and display average daily statistics for the filtered data
            if (dailyTotals.Count > 0)
            {
                double totalCalories = dailyTotals.Values.Sum(v => v.Calories);
                double totalProteins = dailyTotals.Values.Sum(v => v.Proteins);
                double totalCarbs = dailyTotals.Values.Sum(v => v.Carbs);
                double totalFats = dailyTotals.Values.Sum(v => v.Fats);

                // Calculate the average daily values
                averageDailyCalories = totalCalories / dailyTotals.Count;
                averageDailyProteins = totalProteins / dailyTotals.Count;
                averageDailyCarbs = totalCarbs / dailyTotals.Count;
                averageDailyFats = totalFats / dailyTotals.Count;

                // Set the bindings for the labels in the XAML
                results[0] = averageDailyCalories;
                results[1] = averageDailyProteins;
                results[2] = averageDailyCarbs;
                results[3] = averageDailyFats;

            }

        }
        
        return results;
    }


}
