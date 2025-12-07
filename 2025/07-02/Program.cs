// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var file = File.ReadLines("input.txt");
var rows = file.Count();
var splitters = file.SelectMany((row, rowIndex) => row.Select((col, colIndex) => (col, new Coordinate(rowIndex, colIndex))))
    .Where(x => x.col == '^')
    .Select(x => x.Item2)
    .ToHashSet();

var start = new Coordinate(0, file.First().IndexOf('S'));

Dictionary<Coordinate, long> beams = new() { { start, 1 } };

for (int row = 0; row < rows; row++)
{
    Dictionary<Coordinate, long> newBeams = [];
    foreach (var beam in beams)
    {
        var newLoc = beam.Key.Down();
        if (!splitters.Contains(newLoc))
        {
            newBeams.AddOrIncrement(newLoc, beam.Value);
            continue;
        }
        newBeams.AddOrIncrement(newLoc.Left(), beam.Value);
        newBeams.AddOrIncrement(newLoc.Right(), beam.Value);
    }
    beams = newBeams;
}

Console.WriteLine($"Total beams: {beams.Values.Sum()}");

public record Coordinate(int Row, int Column)
{
    public static Coordinate operator +(Coordinate left, Coordinate right) => new(left.Row + right.Row, left.Column + right.Column);

    public Coordinate Down() => this + new Coordinate(1, 0);

    public Coordinate Left() => this + new Coordinate(0, -1);

    public Coordinate Right() => this + new Coordinate(0, 1);
}

public static class DictionaryExtensions
{
    public static void AddOrIncrement(this Dictionary<Coordinate, long> coordinates, Coordinate coordinate, long count)
    {
        if (coordinates.ContainsKey(coordinate))
        {
            coordinates[coordinate] += count;
            return;
        }
        coordinates.Add(coordinate, count);
    }
}
