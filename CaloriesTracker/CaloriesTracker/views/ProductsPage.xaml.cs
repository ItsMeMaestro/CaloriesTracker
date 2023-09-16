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
                    await DisplayAlert("Success", $"{product.Name} has been added to the stats file.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }

       

        

    }
}
