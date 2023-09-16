using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using System.Collections.Generic;
using System.Linq;

namespace CaloriesTracker
{
    public partial class DailyGoalsPage : ContentPage
    {
        private const string CaloriesGoalKey = "CaloriesGoal";
        private const string ProteinsGoalKey = "ProteinsGoal";

        public DailyGoalsPage()
        {
            InitializeComponent();
            LoadGoals(); 
        }

        private void SetGoals_Clicked(object sender, EventArgs e)
        {

            SaveGoals();
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            // Update progress for calories
            double caloriesProgressPercentage = CalculateProgressPercentage("Calories");
            progressLabel.Text = $"Calories Progress: {caloriesProgressPercentage}%";
            progressBar.Progress = caloriesProgressPercentage / 100;

            // Update progress for proteins
            double proteinsProgressPercentage = CalculateProgressPercentage("Proteins");
            proteinsLabel.Text = $"Proteins Progress: {proteinsProgressPercentage}%";
            proteinsProgressBar.Progress = proteinsProgressPercentage / 100;

            // Update progress for fats
            double fatsProgressPercentage = CalculateProgressPercentage("Fats");
            fatsLabel.Text = $"Fats Progress: {fatsProgressPercentage}%";
            fatsProgressBar.Progress = fatsProgressPercentage / 100;

            // Update progress for carbs
            double carbsProgressPercentage = CalculateProgressPercentage("Carbs");
            carbsLabel.Text = $"Carbs Progress: {carbsProgressPercentage}%";
            carbsProgressBar.Progress = carbsProgressPercentage / 100;
        }



        private double CalculateProgressPercentage(string macroName)
        {
            // Replace with your calculation logic for the specific macro
            double consumedValue = GetConsumedMacroValue(macroName);
            double dailyGoal = GetDailyMacroGoal(macroName);

            if (dailyGoal > 0)
            {
                return (consumedValue / dailyGoal) * 100;
            }
            else
            {
                return 0;
            }
        }


        private void ResetGoals_Clicked(object sender, EventArgs e)
        {
           
            ResetGoals();

            UpdateProgress();
        }

        private void SaveGoals()
        {
            try
            {
                double caloriesGoal = double.Parse(caloriesEntry.Text);
                double proteinsGoal = double.Parse(proteinsEntry.Text);
                double carbsGoal = double.Parse(carbsEntry.Text);
                double fatsGoal = double.Parse(fatsEntry.Text);


                Preferences.Set("CaloriesGoalKey", caloriesGoal);
                Preferences.Set("ProteinsGoalKey", proteinsGoal);
                Preferences.Set("CarbsGoalKey", carbsGoal);
                Preferences.Set("FatsGoalKey", carbsGoal);
                DisplayAlert("Success", "Goals saved successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error saving goals: {ex.Message}", "OK");
            }
        }

        private void LoadGoals()
        {
            try
            {
                if (Preferences.ContainsKey(CaloriesGoalKey))
                {
                    double caloriesGoal = Preferences.Get(CaloriesGoalKey, 0.0);
                    caloriesEntry.Text = caloriesGoal.ToString();
                }

                if (Preferences.ContainsKey(ProteinsGoalKey))
                {
                    double proteinsGoal = Preferences.Get(ProteinsGoalKey, 0.0);
                    proteinsEntry.Text = proteinsGoal.ToString();
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error loading goals: {ex.Message}", "OK");
            }
        }

        private void ResetGoals()
        {
            try
            {
                Preferences.Remove(CaloriesGoalKey);
                Preferences.Remove(ProteinsGoalKey);

                DisplayAlert("Success", "Goals reset successfully", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error resetting goals: {ex.Message}", "OK");
            }
        }
        private double GetConsumedMacroValue(string macroName)
        {
            StatsService statsInstance = new StatsService();
            // Load existing stats from the file (if any)
            List<Product> stats = statsInstance.LoadStats();

            // Get today's date without the time component
            DateTime today = DateTime.Now.Date;

            // Calculate the sum of the specific macro for entries with today's date
            double consumedValue = stats
                .Where(product => product.Date == today) // Filter by today's date
                .Sum(product => GetMacroValueByName(product, macroName)); // Sum the specific macro for matching entries

            return consumedValue;
        }



        private double GetMacroValueByName(Product product, string macroCategory)
        {
            // Calculate the value of the specified macro category for the given product
            switch (macroCategory)
            {
                case "Calories":
                    return product.Calories;
                case "Proteins":
                    return product.Proteins;
                case "Carbs":
                    return product.Carbs;
                case "Fats":
                    return product.Fats;
                // Add more cases for other macro categories if needed
                default:
                    return 0.0; // Return 0 for unknown macro categories
            }
        }



        private double GetDailyMacroGoal(string macroName)
        {
            try
            {
                // Modify this to retrieve the daily goal for the specific macro from Preferences
                if (Preferences.ContainsKey($"{macroName}GoalKey"))
                {
                    return Preferences.Get($"{macroName}GoalKey", 0.0);
                }
                else
                {
                    return 0.0; // Return a default value if the key is not found
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                Console.WriteLine($"Error getting daily {macroName} goal: {ex.Message}");
                return 0.0; // Return 0 in case of an error
            }
        }



    }
}
