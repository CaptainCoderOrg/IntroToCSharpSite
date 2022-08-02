using System;
public class ReadValidInputExample
{

    /// <summary>
    /// Prompts the user to enter an integer that is greater than 1000. Then,
    /// returns the users input as an int. If the user enters an invalid valud,
    /// this method will continue to prompt the user.
    /// </summary>
    /// <returns>The users input as an integer</returns>
    public static int GetInputGreaterThan1000()
    {
        // Create a variable to store the value the user enters
        int userChoice;

        // Start a loop
        do
        {
            // Asks the user to enter a number
            Console.Write("Enter a number that is greater than 1000:");

            // Read the user input into a string
            string input = Console.ReadLine();

            // Try to conver the user input into an integer.
            bool isANumber = int.TryParse(input, out userChoice);

            if (isANumber == false)
            {
                // If the user didn't enter an integer, display an error.
                Console.Error.WriteLine("You did not enter a number.");
            }
            else if (userChoice <= 1000)
            {
                // If the user entered an integer that was not greater than 1000,
                // display an error
                Console.WriteLine("That number is not greater than 1000.");
            }
        }
        while (userChoice <= 1000); // If the userChoice is less than 1000, we continue the loop

        // Finally, we return the users choice
        return userChoice;
    }

    public static void Main()
    {
        int value = GetInputGreaterThan1000();
        Console.WriteLine($"You entered {value}.");
    }
}
