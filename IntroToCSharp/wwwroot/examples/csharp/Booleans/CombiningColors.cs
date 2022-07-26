using System;

string red = "red";
string blue = "blue";

CombiningColors(red, blue);


void CombiningColors(string ColorOne, string ColorTwo)
{
    if(ColorOne == "red" && ColorTwo == "blue")
    {
        Console.WriteLine("These two colors make purple!");
    }
    else
    {
        Console.WriteLine("These two colors don't make purple!");
    }
}