using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.IO;
namespace CaloriesTracker
{
    public partial class StatsPage : ContentPage
    {
        StatsService statsInstance = new StatsService();
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load and display the statistics whenever the page appears
            LoadAndDisplayStatistics();
        }
        private void LoadAndDisplayStatistics()
        {
            try
            {
                
                CalculateStatistics();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file access or JSON deserialization
                Console.WriteLine($"Error loading stats: {ex.Message}");
            }
        }

        public StatsPage()
        {
            InitializeComponent();

            // Load and calculate the statistics
            CalculateStatistics();
        }

        // Method to load stats from the JSON file and calculate the sum
        private void CalculateStatistics()
        {
            try
            {
                // Load the stats from the file
                List<Product> stats = statsInstance.LoadStats();

                // Initialize variables to store the total values
                double totalCalories = 0;
                double totalProteins = 0;
                double totalCarbs = 0;
                double totalFats = 0;

                foreach (var product in stats)
                {
                    // Calculate the nutrient values based on weight
                    double weightedCalories = product.Calories * (product.WeightInGrams / 100.0);
                    double weightedProteins = product.Proteins * (product.WeightInGrams / 100.0);
                    double weightedCarbs = product.Carbs * (product.WeightInGrams / 100.0);
                    double weightedFats = product.Fats * (product.WeightInGrams / 100.0);

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
                LoadAndDisplayStatistics(); // Update the displayed statistics

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



    }
}
