/* 
    LoadProducts: Implements method to load list of products
    SearchProducts: Filters based on name and category
 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Reflection;
using System.Text.Json;

namespace CaloriesTracker
{
    public class ProductsViewModel
    {
        private List<Product> allProducts; // Store all products

        public ProductsViewModel()
        {
            // Initialize the list of all products
            allProducts = LoadProducts();
        }

        public List<Product> LoadProducts()
        {
            List<Product> products = new List<Product>();

            try
            {
                // Get the path to the JSON file
                string jsonFileName = "CaloriesTracker.db.Products.products.json"; // Adjust the path as needed
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ProductsViewModel)).Assembly;
                Stream stream = assembly.GetManifestResourceStream(jsonFileName);

                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        products = JsonSerializer.Deserialize<List<Product>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file access or JSON parsing
                Console.WriteLine($"Error loading products: {ex.Message}");
            }

            return products;
        }

        public List<Product> SearchProducts(string query)
        {
            // Filter the products based on the search query
            if (string.IsNullOrWhiteSpace(query))
            {
                // If the query is empty, return all products
                return allProducts;
            }
            else
            {
                // Filter products based on the search query (e.g., by Name or Category)
                return allProducts
                    .Where(product => product.Name.ToLower().Contains(query.ToLower()) || product.Category.ToLower().Contains(query.ToLower()))
                    .ToList();
            }
        }
    }
}
