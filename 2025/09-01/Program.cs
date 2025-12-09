// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var redSquares = File.ReadAllLines("input.txt")
    .Select(x => x.Split(",").Select(int.Parse).ToArray())
    .Select((x, index) => new Coordinate(x[0], x[1]))
    .ToList();

long result = 0;

for (int i = 0; i < redSquares.Count; i++)
{
    for (int j = i + 1; j < redSquares.Count; j++)
    {
        result = Math.Max(result, redSquares[i].CalculateRectangle(redSquares[j]));
    }
}

Console.WriteLine(result);

record Coordinate(long Row, long Column)
{
    public long CalculateRectangle(Coordinate coordinate) => (Math.Abs(Row - coordinate.Row) + 1) * (Math.Abs(Column - coordinate.Column) + 1);
}
