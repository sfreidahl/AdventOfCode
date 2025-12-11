// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var connections = File.ReadAllLines("input.txt").Select(x => x.Split(" ")).ToDictionary(x => x[0].Replace(":", ""), x => x[1..].ToHashSet());

var result = GetPaths("you");

Console.WriteLine(result);

long GetPaths(string location)
{
    if (location == "out")
    {
        return 1;
    }

    return connections[location].Sum(GetPaths);
}
