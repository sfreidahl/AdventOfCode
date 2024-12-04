// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");
var directions = input.First().ToArray();
var directionCount = directions.Length;

var mapInput = input[2..]
    .Select(
        x =>
            x.Replace("=", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", "")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    )
    .ToDictionary(x => x[0], x => new Location(x[1], x[2]));

var startLocations = mapInput.Keys.Where(x => x[2] == 'A').ToArray();
var denominators = new List<long>();

foreach (var location in startLocations)
{
    int steps = 1;
    int index = 0;
    var nextLocation = location;
    while (true)
    {
        var dir = directions[index];

        var currentLocation = mapInput[nextLocation];
        nextLocation = dir switch
        {
            'L' => currentLocation.Left,
            'R' => currentLocation.Right,
        };

        if (nextLocation[2] == 'Z')
        {
            denominators.Add(steps);
            break;
        }

        steps++;

        index = (steps - 1) % directionCount;
    }
    ;
}

Console.WriteLine(CalculateLcmList(denominators));

long CalculateLcmList(List<long> values)
{
    long lcm = values[0];
    foreach (var val in values.ToArray()[1..])
    {
        lcm = CalculateLcm(lcm, val);
        Console.WriteLine(lcm);
    }
    return lcm;
}

long CalculateLcm(long a, long b)
{
    for (long tempNum = 1; ; tempNum++)
    {
        if (a * tempNum % b == 0)
        {
            return a * tempNum;
        }
    }
}

record struct Location(string Left, string Right);
