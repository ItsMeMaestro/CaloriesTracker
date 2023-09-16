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
                    // Create a new product entry with the current date
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
                        Date = DateTime.Now.Date // Set the date to today's date
                    };

                    // Load existing stats from the file (if any)
                    List<Product> stats = LoadStatsFromFile();

                    // Add the new product entry to the stats list
                    stats.Add(newProduct);

                    // Serialize the stats list to JSON
                    string statsJson = JsonConvert.SerializeObject(stats);

                    // Save the JSON data to a file
                    SaveStatsToFile(statsJson);

                    // Inform the user that the product has been added to the stats file
                    await DisplayAlert("Success", $"{product.Name} has been added to the stats file.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }

        // Helper method to load existing stats from the file (or create an empty list if the file doesn't exist)
        private List<Product> LoadStatsFromFile()
        {
            try
            {
                // Get the file path for your JSON stats file
                string filePath = Path.Combine(FileSystem.AppDataDirectory, "CaloriesTracker.db.Stats.stats.json");
               
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read the JSON data from the file
                    string statsJson = File.ReadAllText(filePath);

                    // Deserialize the JSON data to a list of Product objects
                    List<Product> stats = JsonConvert.DeserializeObject<List<Product>>(statsJson);

                    return stats;
                }
                else
                {
                    // If the file doesn't exist, return an empty list
                    return new List<Product>();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file access or JSON deserialization
                Console.WriteLine($"Error loading stats: {ex.Message}");
                return new List<Product>();
            }
        }

        // Helper method to save the serialized stats to the file
        private async void SaveStatsToFile(string statsJson)
        {
            try
            {
                // Get the file path for your JSON stats file
                string filePath = Path.Combine(FileSystem.AppDataDirectory, "CaloriesTracker.db.Stats.stats.json");
                
                // Write the JSON data to the file using StreamWriter
                File.WriteAllText(filePath, statsJson);
            }
            catch (Exception ex)
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, "CaloriesTracker.db.Stats.stats.json");
                await DisplayAlert("Success", $"{filePath} has been added to the stats file.", "OK");
                // Handle any exceptions that may occur during file access or JSON serialization
                Console.WriteLine($"Error saving stats: {ex.Message}");
            }
        }

    }
}
