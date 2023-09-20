# DailyGoalsPage Class

The `DailyGoalsPage` class is a Xamarin.Forms content page within the CaloriesTracker application. It allows users to set and track their daily nutritional goals for calories, proteins, carbs, and fats.

## Constructor

### `public DailyGoalsPage()`

This constructor initializes the `DailyGoalsPage` by setting up the user interface and loading user-defined nutritional goals.



## Methods

### `public void SetGoals_Clicked(object sender, EventArgs e)`

This method is an event handler for the "Set Goals" button click. It toggles the visibility of entry fields and labels to allow users to set their nutritional goals for calories, proteins, carbs, and fats. When the goals are saved, progress bars and labels are updated.

**Parameters:**
- `sender` (`object`): The event sender (a button).
- `e` (`EventArgs`): Event arguments.



**Exceptions:** This method may throw exceptions related to user input.

### `public void UpdateProgress()`

This method updates the progress bars and labels based on user-defined nutritional goals and consumed values for calories, proteins, carbs, and fats.



### `public void OnGoalEntryTextChanged(object sender, TextChangedEventArgs e)`

This method is an event handler for text entry changes. It updates the corresponding label when the user enters their nutritional goals for calories, proteins, carbs, and fats.

**Parameters:**
- `sender` (`object`): The event sender (a text entry).
- `e` (`TextChangedEventArgs`): Event arguments containing the old and new text values.



### `public Xamarin.Forms.Label GetGoalLabelForEntry(Xamarin.Forms.Entry entry)`

This method returns the corresponding label for a given text entry. It is used to associate text entries with their goal labels.

**Parameters:**
- `entry` (`Xamarin.Forms.Entry`): The text entry for which to retrieve the label.

**Returns:**
- `Xamarin.Forms.Label`: The label associated with the text entry.



### `protected override void OnAppearing()`

This method is overridden to perform actions when the page appears. It updates the progress bars and labels based on user-defined goals and consumed values.



### `public void SaveGoals()`

This method saves user-defined nutritional goals to Xamarin.Essentials Preferences for calories, proteins, carbs, and fats. It also displays an alert message to inform the user of the successful goal-saving operation.



**Exceptions:** This method may throw exceptions related to user input.

### `public void LoadGoals()`

This method loads user-defined nutritional goals from Xamarin.Essentials Preferences for calories and proteins. It also displays an alert message in case of an error.



**Exceptions:** This method may throw exceptions related to preferences access.

### `public double GetConsumedMacroValue(string macroName)`

This method calculates the total consumed value of a specific macro category (e.g., calories, proteins, carbs, or fats) based on user input. It retrieves data from the `StatsService` class.

**Parameters:**
- `macroName` (`string`): The name of the macro category to calculate (e.g., "Calories", "Proteins").

**Returns:**
- `double`: The total consumed value of the specified macro category.

**Exceptions:** This method may throw exceptions related to data access.

### `public double GetMacroValueByName(Product product, string macroCategory)`

This method calculates the value of a specific macro category (e.g., calories, proteins, carbs, or fats) for a given product. It is used within the `GetConsumedMacroValue` method.

**Parameters:**
- `product` (`Product`): The product for which to calculate the macro category value.
- `macroCategory` (`string`): The name of the macro category to calculate (e.g., "Calories", "Proteins").

**Returns:**
- `double`: The value of the specified macro category for the given product.



### `public double GetDailyMacroGoal(string macroName)`

This method retrieves the user's daily nutritional goal for a specific macro category (e.g., calories, proteins, carbs, or fats) from Xamarin.Essentials Preferences.

**Parameters:**
- `macroName` (`string`): The name of the macro category to retrieve the goal for (e.g., "Calories", "Proteins").

**Returns:**
- `double`: The user's daily nutritional goal for the specified macro category.



## Usage

Here's an example of how to use the `DailyGoalsPage` class:

```csharp
// Create an instance of DailyGoalsPage
DailyGoalsPage dailyGoalsPage = new DailyGoalsPage();

// Display the DailyGoalsPage in your Xamarin.Forms application
App.Current.MainPage = dailyGoalsPage;
