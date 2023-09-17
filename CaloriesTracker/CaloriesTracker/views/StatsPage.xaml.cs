using System;
using System.Collections.Generic;
using System.Linq;
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
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load and calculate the statistics when the page first appears
            LoadAndDisplayStatistics();
        }

        private void LoadAndDisplayStatistics()
        {
            // Get the currently selected month (or the default if nothing is selected)
            string selectedMonth = monthPicker.SelectedItem?.ToString() ?? DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);

            // Parse the selected month into a DateTime object for comparison
            DateTime selectedDate = DateTime.ParseExact(selectedMonth, "MMMM", CultureInfo.InvariantCulture);

            // Filter the statistics based on the selected month
            List<Product> filteredStats = statsInstance.LoadStats()
                .Where(product => product.Date.Month == selectedDate.Month && product.Date.Year == selectedDate.Year)
                .ToList();

            // Calculate the number of days in the selected month
            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);

            // Calculate and display average daily statistics for the filtered data
            CalculateAverageDailyStatistics(filteredStats, daysInMonth);
        }

        private void CalculateAverageDailyStatistics(List<Product> stats, int daysInMonth)
        {
            try
            {
                // Create a dictionary to store daily totals for each nutrient
                Dictionary<DateTime, (double Calories, double Proteins, double Carbs, double Fats)> dailyTotals =
                    new Dictionary<DateTime, (double, double, double, double)>();

                foreach (var product in stats)
                {
                    // Calculate the nutrient values based on weight
                    double weightedCalories = (product.Calories / 100.0) * product.WeightInGrams;
                    double weightedProteins = (product.Proteins / 100.0) * product.WeightInGrams;
                    double weightedCarbs = (product.Carbs / 100.0) * product.WeightInGrams;
                    double weightedFats = (product.Fats / 100.0) * product.WeightInGrams;

                    // Create a date with the time component set to midnight (removing the time component)
                    DateTime productDate = product.Date.Date;

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
                }

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
                    double averageDailyCalories = totalCalories / dailyTotals.Count;
                    double averageDailyProteins = totalProteins / dailyTotals.Count;
                    double averageDailyCarbs = totalCarbs / dailyTotals.Count;
                    double averageDailyFats = totalFats / dailyTotals.Count;

                    // Set the bindings for the labels in the XAML
                    BindingContext = new
                    {
                        TotalCalories = totalCalories,
                        TotalProteins = totalProteins,
                        TotalCarbs = totalCarbs,
                        TotalFats = totalFats,
                        AverageDailyCalories = averageDailyCalories,
                        AverageDailyProteins = averageDailyProteins,
                        AverageDailyCarbs = averageDailyCarbs,
                        AverageDailyFats = averageDailyFats
                    };
                }
                else
                {
                    // Handle the case where there are no non-zero calorie days
                    DisplayAlert("No Data", "There is no data for days with non-zero calories.", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


       
        private void SelectMonthButton_Clicked(object sender, EventArgs e)
        {
            monthPicker.IsVisible = !monthPicker.IsVisible;
        }

        // Handle the selection change in the Picker
        private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAndDisplayStatistics();
        }
    }
}
