using System;
using System.Collections.Generic;
public class AssociationList
{

    /// <summary>
    /// Given two associated lists, the first containing names and the second
    /// containing the height of the person from the first list, finds the
    /// person who is the shortest and returns their name.
    /// </summary>
    /// <param name="names">A non-empty list of names</param>
    /// <param name="heights">A list of heights for each person in names</param>
    /// <returns>The name of the person who is the shortest</returns>
    public static string FindShortestPerson(List<string> names, List<double> heights)
    {
        // First, we must validate the inputs to this method
        if (names == null || heights == null) throw new Exception("Names List and Heights list must be non-null.");
        if (names.Count == 0) throw new Exception("Cannot process an empty list.");
        if (heights.Count != names.Count) throw new Exception("Names and Heights lists were not the same length.");

        // We don't know who the shortest person but we will set it to the first
        // person until we find someone who is shorter NOTICE: We use index 0
        // for both lists because these are associated lists. This means each
        // index is related across the lists
        string shortestPerson = names[0];
        double shortestHeight = heights[0];

        // Next, we need to search the heights list to see if anyone is shorter
        // than the first person. We do this by iterating over the list

        // We need to keep track of the index we are on so we create an index variable:
        int index = 0;
        foreach (double height in heights)
        {
            // We check to see if the height we are on is less than the current
            // shortestHeight
            if (height < shortestHeight)
            {
                // If it is shorter, we need to update the shortest person:
                shortestPerson = names[index];
                shortestHeight = heights[index];
            }
            // We increase the index to keep it synced with our height
            // association
            index = index + 1;
        }

        // After we have finished iterating through the heights, we have checked
        // each height and shortestPerson contains the shortest person. So, we
        // return shortestPErson

        return shortestPerson;
    }

    public static void Main()
    {
        List<string> people = new List<string>();
        List<double> heights = new List<double>();
        people.Add("Bob the Buidler");
        heights.Add(10);
        people.Add("John Cena");
        heights.Add(72.2);
        people.Add("Mx. Collard");
        heights.Add(73.4);

        string shortest = FindShortestPerson(people, heights);
        Console.WriteLine($"The shortest person is {shortest}.");
    }
}
