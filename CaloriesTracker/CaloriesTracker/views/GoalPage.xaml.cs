/*
  This C# file contains the code-behind logic for the DailyGoalsPage in the CaloriesTracker application.
  The DailyGoalsPage allows users to set and track their daily nutritional goals for calories, proteins, carbs, and fats.
*/
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
        // Constants for keys used to store and retrieve daily goals in Preferences
        private const string CaloriesGoalKey = "CaloriesGoal";
        private const string ProteinsGoalKey = "ProteinsGoal";
        // Constructor for the DailyGoalsPage
        public DailyGoalsPage()
        {
            InitializeComponent();
            LoadGoals(); 
        }
        // Event handler for the "Set Goals" button click
        private void SetGoals_Clicked(object sender, EventArgs e)
        {
            // Toggle the visibility of entry fields and labels
            caloriesEntry.IsVisible = !caloriesEntry.IsVisible;
            caloriesGoalLabel.IsVisible = !caloriesGoalLabel.IsVisible;

            proteinsEntry.IsVisible = !proteinsEntry.IsVisible;
            proteinsGoalLabel.IsVisible = !proteinsGoalLabel.IsVisible;

            carbsEntry.IsVisible = !carbsEntry.IsVisible;
            carbsGoalLabel.IsVisible = !carbsGoalLabel.IsVisible;

            fatsEntry.IsVisible = !fatsEntry.IsVisible;
            fatsGoalLabel.IsVisible = !fatsGoalLabel.IsVisible;

            if (!caloriesEntry.IsVisible)
            {
                // Entry fields are hidden, save goals and update progress
                SaveGoals();
                UpdateProgress();
            }
        }

        // Method to update progress bars and labels based on user-defined goals and consumed values
        public void UpdateProgress()
        {
            // Update progress for calories
            double caloriesProgress = GetConsumedMacroValue("Calories");
            double caloriesGoal = GetDailyMacroGoal("Calories");
            progressLabel.Text = $"Calories Progress: {caloriesProgress} / {caloriesGoal}";
            progressBar.Progress = caloriesProgress / caloriesGoal;

            // Update progress for proteins
            double proteinsProgress = GetConsumedMacroValue("Proteins");
            double proteinsGoal = GetDailyMacroGoal("Proteins");
            proteinsLabel.Text = $"Proteins Progress: {proteinsProgress} / {proteinsGoal}";
            proteinsProgressBar.Progress = proteinsProgress / proteinsGoal;

            // Update progress for fats
            double fatsProgress = GetConsumedMacroValue("Fats");
            double fatsGoal = GetDailyMacroGoal("Fats");
            fatsLabel.Text = $"Fats Progress: {fatsProgress} / {fatsGoal}";
            fatsProgressBar.Progress = fatsProgress / fatsGoal;

            // Update progress for carbs
            double carbsProgress = GetConsumedMacroValue("Carbs");
            double carbsGoal = GetDailyMacroGoal("Carbs");
            carbsLabel.Text = $"Carbs Progress: {carbsProgress} / {carbsGoal}";
            carbsProgressBar.Progress = carbsProgress / carbsGoal;
        }
        // Event handler for text entry change
        private void OnGoalEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Xamarin.Forms.Entry)sender;
            var label = GetGoalLabelForEntry(entry);

            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                label.Text = $"Goal: {entry.Text}";
                label.IsVisible = true;
                entry.IsVisible = false;
            }
            else
            {
                label.IsVisible = false;
                entry.IsVisible = true;
            }
        }
        // Method to get the corresponding label for a text entry
        private Xamarin.Forms.Label GetGoalLabelForEntry(Xamarin.Forms.Entry entry)
        {
            if (entry == caloriesEntry)
                return caloriesGoalLabel;
            else if (entry == proteinsEntry)
                return proteinsGoalLabel;
            else if (entry == carbsEntry)
                return carbsGoalLabel;
            else if (entry == fatsEntry)
                return fatsGoalLabel;

            return null;
        }


       
        // Method called when the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateProgress();
        }

        // Method to save user-defined goals to Preferences
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

        // Method to load user-defined goals from Preferences
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


        private double GetConsumedMacroValue(string macroName)
        {
            try
            {
                StatsService statsInstance = new StatsService();
                // Load existing stats from the file (if any)
                List<Product> stats = statsInstance.LoadStats();

                // Get today's date without the time component
                DateTime today = DateTime.Now.Date;

                // Calculate the sum of the specific macro for entries with today's date
                double consumedValue = 0;

                foreach (var product in stats)
                {
                    if (product.Date == today)
                    {
                        // Calculate the specific macro value based on weight
                        double weightedMacroValue = GetMacroValueByName(product, macroName) * (product.WeightInGrams / 100.0);

                        // Add the weighted macro value to the consumedValue
                        consumedValue += weightedMacroValue;
                    }
                }

                return consumedValue;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                return 0; // Return 0 in case of an error
            }
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
