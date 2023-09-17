using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.IO;
using System.Globalization;

namespace CaloriesTracker
{
    public partial class StatsPage : ContentPage
    {
        // List of month names for the Picker
        List<string> months = new List<string>
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        // Initialize the StatsService instance
        StatsService statsInstance = new StatsService();

      

        public StatsPage()
        {
            InitializeComponent();

            // Populate the Picker with month options
            foreach (var month in months)
            {
                monthPicker.Items.Add(month);
            }
        }

        // Method to load stats from the JSON file and calculate the sum
        private void CalculateStatistics(List<Product> stats)
        {
            try
            {
                // Initialize variables to store the total values
                double totalCalories = 0;
                double totalProteins = 0;
                double totalCarbs = 0;
                double totalFats = 0;

                foreach (var product in stats)
                {
                    // Calculate the nutrient values based on weight
                    double weightedCalories = (product.Calories / 100.0) * product.WeightInGrams;
                    double weightedProteins = (product.Proteins / 100.0) * product.WeightInGrams;
                    double weightedCarbs = (product.Carbs / 100.0) * product.WeightInGrams;
                    double weightedFats = (product.Fats / 100.0) * product.WeightInGrams;

                    // Add the weighted values to the totals
                    totalCalories += weightedCalories;
                    totalProteins += weightedProteins;
                    totalCarbs += weightedCarbs;
                    totalFats += weightedFats;
                }

                // Set the bindings for the labels in the XAML
                BindingContext = new
                {
                    TotalCalories = totalCalories,
                    TotalProteins = totalProteins,
                    TotalCarbs = totalCarbs,
                    TotalFats = totalFats
                };
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void ClearStatistics_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Clear Statistics", "Are you sure you want to clear the statistics?", "Yes", "No");

            if (result)
            {
                ClearStatistics();
                

                await DisplayAlert("Success", "Statistics have been cleared.", "OK");
            }
        }

        private void ClearStatistics()
        {
            try
            {
                // Create an empty list of statistics
                List<Product> emptyStats = new List<Product>();

                // Serialize the empty list to JSON
                string emptyStatsJson = JsonConvert.SerializeObject(emptyStats);

                // Get the file path for your JSON stats file
                string filePath = Path.Combine(FileSystem.AppDataDirectory, "CaloriesTracker.db.Stats.stats.json");

                // Save the JSON data to the file
                File.WriteAllText(filePath, emptyStatsJson);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file access or JSON serialization
                Console.WriteLine($"Error clearing stats: {ex.Message}");
            }
        }

        private void SelectMonthButton_Clicked(object sender, EventArgs e)
        {
            monthPicker.IsVisible = !monthPicker.IsVisible;
        }

        // Handle the selection change in the Picker
        private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected month
            string selectedMonth = monthPicker.SelectedItem.ToString();

            // Parse the selected month into a DateTime object for comparison
            DateTime selectedDate = DateTime.ParseExact(selectedMonth, "MMMM", CultureInfo.InvariantCulture);

            // Filter the statistics based on the selected month
            List<Product> filteredStats = statsInstance.LoadStats()
                .Where(product => product.Date.Month == selectedDate.Month && product.Date.Year == selectedDate.Year)
                .ToList();

            // Calculate and display statistics for the filtered data
            CalculateStatistics(filteredStats);
        }
    }
}
