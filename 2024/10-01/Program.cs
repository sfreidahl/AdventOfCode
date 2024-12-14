// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var locations = File.ReadAllLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((height, columnIndex) => new Location(new Coordinate(rowIndex, columnIndex), int.Parse("" + height))))
    .ToDictionary(x => x.Coordinate, x => x);

var result = locations.Where(x => x.Value.Height == 0).Select(x => x.Value.Traverse(locations).Count()).Sum();

Console.WriteLine(result);

record struct Coordinate(int Row, int Column);

record struct Location(Coordinate Coordinate, int Height)
{
    public HashSet<Coordinate> Traverse(Dictionary<Coordinate, Location> map)
    {
        var up = Move(new(-1, 0), map);
        var down = Move(new(1, 0), map);
        var left = Move(new(0, -1), map);
        var right = Move(new(0, 1), map);
        return [.. up, .. down, .. left, .. right];
    }

    private HashSet<Coordinate> Move(Coordinate coordinate, Dictionary<Coordinate, Location> map)
    {
        if (map.TryGetValue(new(Coordinate.Row + coordinate.Row, Coordinate.Column + coordinate.Column), out var location))
        {
            if (location.Height != Height + 1)
            {
                return [];
            }
            if (location.Height == 9)
            {
                return [location.Coordinate];
            }
            return location.Traverse(map);
        }
        return [];
    }
}
