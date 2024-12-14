// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((c, columnIndex) => (c, new Coordinate(rowIndex, columnIndex))))
    .GroupBy(x => x.c, x => x.Item2);

var result = 0;

foreach (var group in input)
{
    var coordinates = group.ToHashSet();
    while (coordinates.Count > 0)
    {
        HashSet<Coordinate> fields = [];
        var perimeter = coordinates.First().Traverse(coordinates, fields);
        result += perimeter * fields.Count;
        coordinates.RemoveRange(fields);
        Console.WriteLine($"perimeter: {perimeter}, size: {fields.Count}");
    }
}

Console.WriteLine(result);

record Coordinate(int Row, int Column)
{
    public int Traverse(HashSet<Coordinate> coordinates, HashSet<Coordinate> field)
    {
        if (!coordinates.Contains(this))
        {
            return 1;
        }
        if (field.Contains(this))
        {
            return 0;
        }
        field.Add(this);
        int edges = 0;
        foreach (var direction in _directions)
        {
            var newCoord = Next(direction);
            edges += newCoord.Traverse(coordinates, field);
        }
        return edges;
    }

    private Coordinate Next(Coordinate diff) => new Coordinate(Row + diff.Row, Column + diff.Column);

    private static List<Coordinate> _directions = [new Coordinate(0, 1), new Coordinate(0, -1), new Coordinate(1, 0), new Coordinate(-1, 0)];
}

public static class HasSetExtensions
{
    public static void RemoveRange<T>(this HashSet<T> set, IEnumerable<T> values)
    {
        foreach (var item in values)
        {
            set.Remove(item);
        }
    }
}
