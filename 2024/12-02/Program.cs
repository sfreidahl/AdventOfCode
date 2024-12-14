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
        coordinates.RemoveRange(fields);
        var edges = fields.Edges();
        Console.WriteLine(
            $"{group.Key}: perimeter: {perimeter.Count}, size: {fields.Count}, edges: hor: {edges.hor}, vert: {edges.vert}, price: {fields.Count * (edges.hor + edges.vert)}"
        );
        result += fields.Count * (edges.hor + edges.vert);
    }
}

Console.WriteLine(result);

public record Coordinate(int Row, int Column)
{
    public HashSet<Coordinate> Traverse(HashSet<Coordinate> coordinates, HashSet<Coordinate> field)
    {
        if (!coordinates.Contains(this))
        {
            return [this];
        }
        if (field.Contains(this))
        {
            return [];
        }
        field.Add(this);
        HashSet<Coordinate> edges = [];
        foreach (var direction in _directions)
        {
            var newCoord = Next(direction);
            edges.AddRange(newCoord.Traverse(coordinates, field));
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

    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
    {
        foreach (var item in values)
        {
            set.Add(item);
        }
    }

    public static (int hor, int vert) Edges(this HashSet<Coordinate> coordinates)
    {
        var startRow = coordinates.MinBy(x => x.Row)!;
        var endRow = coordinates.MaxBy(x => x.Row)!;
        var startCol = coordinates.MinBy(x => x.Column)!;
        var endCol = coordinates.MaxBy(x => x.Column)!;

        var horEdge = 0;
        for (int row = startRow.Row; row <= endRow.Row; row++)
        {
            bool isEdgeOver = false;
            bool isEdgeUnder = false;
            for (int column = startCol.Column; column <= endCol.Column; column++)
            {
                var exists = coordinates.Contains(new Coordinate(row, column));
                if (!exists)
                {
                    isEdgeOver = false;
                    isEdgeUnder = false;
                    continue;
                }
                var existsOver = coordinates.Contains(new Coordinate(row - 1, column));
                var existsUnder = coordinates.Contains(new Coordinate(row + 1, column));
                if (!existsOver && !isEdgeOver)
                {
                    horEdge++;
                }
                if (!existsUnder && !isEdgeUnder)
                {
                    horEdge++;
                }
                isEdgeOver = !existsOver;
                isEdgeUnder = !existsUnder;
            }
        }

        var vertEdge = 0;
        for (int column = startCol.Column; column <= endCol.Column; column++)
        {
            bool isEdgeLeft = false;
            bool isEdgeRight = false;
            for (int row = startRow.Row; row <= endRow.Row; row++)
            {
                var exists = coordinates.Contains(new Coordinate(row, column));
                if (!exists)
                {
                    isEdgeLeft = false;
                    isEdgeRight = false;
                    continue;
                }

                var existsLeft = coordinates.Contains(new Coordinate(row, column - 1));
                var existsRight = coordinates.Contains(new Coordinate(row, column + 1));
                if (!existsLeft && !isEdgeLeft)
                {
                    vertEdge++;
                }
                if (!existsRight && !isEdgeRight)
                {
                    vertEdge++;
                }
                isEdgeLeft = !existsLeft;
                isEdgeRight = !existsRight;
            }
        }

        return (horEdge, vertEdge);
    }
}
