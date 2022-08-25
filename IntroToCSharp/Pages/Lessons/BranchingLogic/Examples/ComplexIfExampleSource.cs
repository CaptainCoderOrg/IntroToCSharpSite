public static class ComplexIfExampleSource {
    public static readonly string Code = @"
string userInput = Console.ReadLine()!;
if (userInput == ""1"")
{
    Console.WriteLine(""You enter the cave and prepare to face the dragon!"");
}
else if (userInput == ""2"")
{
    Console.WriteLine(""You return town to buy supplies."");
}
else if (userInput == ""3"")
{
    Console.WriteLine(""You run home to the safety of your warm bed."");
}
else
{
    Console.WriteLine(""Invalid Choice!"");
}
    ".Trim();
}