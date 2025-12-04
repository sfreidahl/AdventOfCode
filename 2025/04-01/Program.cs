// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var paperRows = File.ReadLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((col, colIndex) => (col, new Coordinate(rowIndex, colIndex))))
    .Where(x => x.col == '@')
    .Select(x => x.Item2)
    .ToHashSet();

var result = paperRows.RemoveWhere(x => x.CheckAdjacent(paperRows));

Console.WriteLine(result);

record Coordinate(int Row, int Column)
{
    static readonly List<Coordinate> dirs = [new(1, 1), new(1, 0), new(0, 1), new(1, -1), new(-1, 1), new(-1, -1), new(-1, 0), new(0, -1)];

    public bool CheckAdjacent(HashSet<Coordinate> paperRolls)
    {
        var count = 0;
        foreach (var dir in dirs)
        {
            count += paperRolls.Contains(this + dir) ? 1 : 0;
        }
        return count < 4;
    }

    public static Coordinate operator +(Coordinate left, Coordinate right) => new(left.Row + right.Row, left.Column + right.Column);
}
