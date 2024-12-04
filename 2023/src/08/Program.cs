// See https://aka.ms/new-console-template for more information
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

var nextLocation = "AAA";
int steps = 0;
int index = 0;

while (nextLocation != "ZZZ")
{
    var dir = directions[index];

    var currentLocation = mapInput[nextLocation];
    nextLocation = dir switch
    {
        'L' => currentLocation.Left,
        'R' => currentLocation.Right,
    };

    steps++;
    index = steps % directionCount;
}

Console.WriteLine(steps);

record struct Location(string Left, string Right);
