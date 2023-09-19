# StatsPage Class

The `StatsPage` class is part of the CaloriesTracker application and is responsible for displaying statistics related to the consumption of nutritional values (e.g., calories, proteins, carbs, and fats) for a selected month.

## Dependencies

This class relies on the following dependencies:
- `Xamarin.Forms`
- `Newtonsoft.Json`
- `Xamarin.Essentials`
- `System`
- `System.Collections.Generic`
- `System.Linq`
- `System.IO`
- `System.Globalization`

## Constructor

### `public StatsPage()`

This constructor initializes the `StatsPage` by setting up the user interface, populating the month picker with month options, and loading and displaying statistics for the selected month.

**Parameters:** None

**Returns:** None

**Exceptions:** None

## Properties

None

## Methods

### `protected override void OnAppearing()`

This method is overridden to perform actions when the page first appears. It calls the `LoadAndDisplayStatistics` method to load and display statistics.

**Parameters:** None

**Returns:** None

**Exceptions:** None

### `private void LoadAndDisplayStatistics()`

This method loads and displays statistics based on the selected month. It retrieves statistics data from the `StatsService` class, filters the data for the selected month, and calculates average daily nutritional values. The results are displayed on the page.

**Parameters:** None

**Returns:** None

**Exceptions:** This method may throw exceptions related to data access or calculations.

### `private void CalculateAverageDailyStatistics(List<Product> stats, int daysInMonth)`

This method calculates average daily nutritional statistics based on the provided statistics data and the number of days in the selected month. It filters out days with zero calorie sums and displays the calculated values on the page.

**Parameters:**
- `stats` (`List<Product>`): The list of statistics data to calculate from.
- `daysInMonth` (`int`): The number of days in the selected month.

**Returns:** None

**Exceptions:** This method may throw exceptions related to calculations.

### `private void SelectMonthButton_Clicked(object sender, EventArgs e)`

This method is an event handler for the "Select Month" button click. It toggles the visibility of the month picker.

**Parameters:**
- `sender` (`object`): The event sender (a button).
- `e` (`EventArgs`): Event arguments.

**Returns:** None

**Exceptions:** None

### `private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)`

This method is an event handler for the selection change in the month picker. It triggers the `LoadAndDisplayStatistics` method to load and display statistics for the selected month.

**Parameters:**
- `sender` (`object`): The event sender (the month picker).
- `e` (`EventArgs`): Event arguments.

**Returns:** None

**Exceptions:** None

## Usage

Here's an example of how to use the `StatsPage` class:

```csharp
// Create an instance of StatsPage
StatsPage statsPage = new StatsPage();

// Display the StatsPage in your Xamarin.Forms application
App.Current.MainPage = statsPage;
