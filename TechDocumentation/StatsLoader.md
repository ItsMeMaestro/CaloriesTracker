# StatsService Class

The `StatsService` class is responsible for managing the loading and saving of statistical data related to the CaloriesTracker application. This includes loading stored statistics from a JSON file and saving updated statistics to the same file.

## Dependencies

This class relies on the following dependencies:
- `System`
- `System.Collections.Generic`
- `System.IO`
- `Newtonsoft.Json`
- `Xamarin.Essentials`


## Methods

### `public List<Product> LoadStats()`

This method loads statistical data from a JSON file stored in the application's data directory.

**Returns:** 
- `List<Product>`: A list of `Product` objects containing statistical data.

**Exceptions:** This method may throw exceptions related to file access or JSON deserialization.

### `public void SaveStats(List<Product> stats)`

This method saves the provided statistical data to a JSON file in the application's data directory, overwriting any existing data.

**Parameters:** 
- `stats` (`List<Product>`): A list of `Product` objects containing statistical data to be saved.



**Exceptions:** This method may throw exceptions related to file access or JSON serialization.

## Usage

Here's an example of how to use the `StatsService` class:

```csharp
// Create an instance of StatsService
StatsService statsService = new StatsService();

// Load statistical data
List<Product> loadedStats = statsService.LoadStats();

// Modify the loaded data as needed

// Save the updated statistical data
statsService.SaveStats(loadedStats);
