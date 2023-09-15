using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace CaloriesTracker
{
    public class ProductsViewModel
    {
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
    }
}


