using System.Diagnostics;

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
var commands = File.ReadAllLines("input.txt")
    .Select(x => x.Split(" "))
    .Select(x => new Command(x[2].Replace("(", "").Replace(")", "").Replace("#", "")));

List<Location> corners = new List<Location>() { new Location(0, 0) };
foreach (var command in commands)
{
    var prevCorner = corners.LastOrDefault();
    corners.Add(GetNewLocation(prevCorner, command.Direction, command.Distance));
}

var r = 0L;

for (int i = 0; i < corners.Count - 1; i++)
{
    var thisLoc = corners[i];
    var nextLoc = corners[i + 1];
    r += thisLoc.Row * nextLoc.Column - thisLoc.Column * nextLoc.Row;
}

var edge = commands.Sum(x => x.Distance);

var result = r / 2 + edge / 2 + 1;
stopWatch.Stop();

Console.WriteLine(r);
Console.WriteLine(r / 2);
Console.WriteLine(edge);
Console.WriteLine($"result: {result} found after {stopWatch.ElapsedMilliseconds}ms");

Location GetNewLocation(Location previousLocation, Direction direction, long distance)
{
    if (direction == Direction.Up)
    {
        return new Location(previousLocation.Row + distance, previousLocation.Column);
    }
    if (direction == Direction.Down)
    {
        return new Location(previousLocation.Row - distance, previousLocation.Column);
    }
    if (direction == Direction.Left)
    {
        return new Location(previousLocation.Row, previousLocation.Column - distance);
    }
    if (direction == Direction.Right)
    {
        return new Location(previousLocation.Row, previousLocation.Column + distance);
    }
    throw new Exception("UNPOSIBLE");
}

record struct Location(long Row, long Column);

record struct Command(Direction Direction, long Distance)
{
    public Command(string hex) : this((Direction)int.Parse($"{hex[5]}"), long.Parse(hex[..5], System.Globalization.NumberStyles.HexNumber)) { }
}

enum Direction
{
    Right,
    Down,
    Left,
    Up
}
