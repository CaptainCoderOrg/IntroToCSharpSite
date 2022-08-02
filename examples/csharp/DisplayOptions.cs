using System;
using System.Collections.Generic;
public class LoopThroughListExample
{
    
    /// <summary>
    /// Given a non-empty list of options, display the options as a list
    /// starting with the number 1.
    /// </summary>
    /// <param name="options">a non-empty list of options to be displayed.</param>
    public static void DisplayOptions(List<string> options)
    {
        // Start by validating the input
        if (options == null) throw new ArgumentNullException("List of options may not be null.");
        if (options.Count == 0) throw new ArgumentException("The List of options must contain at least 1 option.");

        // If the input is valid, we continue:
        
        int ix = 1; // Create a variable to track the index
        foreach (string option in options)
        {
            Console.WriteLine($"{ix}. {option}"); // Display the current index and option
            ix = ix + 1; // Increment the index
        }
    }

    public static void Main()
    {
        List<string> options = new List<string>();
        options.Add("First Choice");
        options.Add("Second Choice");
        options.Add("Third Choice");
        DisplayOptions(options);
    }
}
