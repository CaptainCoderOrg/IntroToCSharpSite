using System;
using System.Collections.Generic;

// First, we create a list of dog types
List<string> dogs = new List<string>();

dogs.Add("Pomeranian");
dogs.Add("Bulldog");
dogs.Add("Husky");
dogs.Add("Poodle");
dogs.Add("Golden Retriever");
dogs.Add("Shiba Inu");

// Next, we generate a random number between 0 and the number of elements in the list:

Random generator = new Random();
int index = generator.Next(0, dogs.Count);

// Finally, we must select the dog from the list that is at he randomly generated index
string randomDog = dogs[index];
Console.WriteLine($"You found a {randomDog}");