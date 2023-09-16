using System;
using System.Collections.Generic;
using Xamarin.Forms;

using Xamarin.Essentials;
using Newtonsoft.Json;
using System.IO;

namespace CaloriesTracker
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage()
        {
            InitializeComponent();

            // Create an instance of ProductsViewModel to load products
            var viewModel = new ProductsViewModel();
            var products = viewModel.LoadProducts();

            // Set the ListView's ItemsSource to the loaded products
            productListView.ItemsSource = products;
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            // Retrieve the product associated with the button clicked
            var button = (Button)sender;
            var product = (Product)button.BindingContext;

            // Display a confirmation dialog to ask the user if they want to add the product to the stats file
            var result = await DisplayAlert("Add Product", $"Add {product.Name} to stats file?", "Yes", "No");
            if (result)
            {
                try
                {
                    // Use an Entry to get the weight input from the user
                    var weightEntry = new Entry
                    {
                        Placeholder = "Enter weight in grams",
                        Keyboard = Keyboard.Numeric // Ensure numeric keyboard for weight input
                    };

                    // Create a dialog box to get weight input from the user
                    var weightInput = await DisplayPromptAsync("Enter Weight", $"Enter weight for {product.Name} in grams:", "OK", "Cancel", null, -1, null, null);


                    if (!string.IsNullOrEmpty(weightInput))
                    {
                        if (double.TryParse(weightInput, out double weightInGrams))
                        {
                            // Create a new product entry with the current date and weight
                            Product newProduct = new Product
                            {
                                Name = product.Name,
                                Calories = product.Calories,
                                Carbs = product.Carbs,
                                Fats = product.Fats,
                                Proteins = product.Proteins,
                                Img = product.Img,
                                Description = product.Description,
                                Category = product.Category,
                                Date = DateTime.Now.Date, // Set the date to today's date
                                WeightInGrams = weightInGrams // Set the weight
                            };

                            StatsService statsInstance = new StatsService();

                            // Load existing stats from the file (if any)
                            List<Product> stats = statsInstance.LoadStats();

                            // Add the new product entry to the stats list
                            stats.Add(newProduct);

                            // Serialize the stats list to JSON
                            string statsJson = JsonConvert.SerializeObject(stats);

                            // Save the JSON data to a file
                            statsInstance.SaveStats(stats);

                            // Inform the user that the product has been added to the stats file
                            await DisplayAlert("Success", $"{product.Name} (Weight: {weightInGrams} grams) has been added to the stats file.", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Please enter a valid numeric weight in grams.", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }






    }
}
