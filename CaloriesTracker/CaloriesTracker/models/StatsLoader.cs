using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace CaloriesTracker
{
    public class StatsService
    {
        public List<Product> LoadStats()
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

        public void SaveStats(List<Product> stats)
        {
            try
            {
                // Get the file path for your JSON stats file
                string filePath = Path.Combine(FileSystem.AppDataDirectory, "CaloriesTracker.db.Stats.stats.json");

                // Serialize the stats list to JSON
                string statsJson = JsonConvert.SerializeObject(stats);

                // Write the JSON data to the file using StreamWriter
                File.WriteAllText(filePath, statsJson);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file access or JSON serialization
                Console.WriteLine($"Error saving stats: {ex.Message}");
            }
        }
    }
}
