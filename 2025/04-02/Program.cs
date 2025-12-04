var paperRows = File.ReadLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((col, colIndex) => (col, new Coordinate(rowIndex, colIndex))))
    .Where(x => x.col == '@')
    .Select(x => x.Item2)
    .ToHashSet();

var count = 0;
int removedRolls;
do
{
    removedRolls = paperRows.RemoveWhere(x => x.CheckAdjacent(paperRows));
    count += removedRolls;
} while (removedRolls > 0);

Console.WriteLine(count);

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
