// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Console.Clear();
var file = File.ReadLines("input.txt");
var rows = file.Count();
var splitters = file.SelectMany((row, rowIndex) => row.Select((col, colIndex) => (col, new Coordinate(rowIndex, colIndex))))
    .Where(x => x.col == '^')
    .Select(x => x.Item2)
    .ToHashSet();

// WriteMany(splitters, '^');

var start = new Coordinate(0, file.First().IndexOf('S'));

// Write(start, 'S');

HashSet<Coordinate> beams = [start];

HashSet<Coordinate> splittersHit = [];

for (int row = 0; row < rows; row++)
{
    HashSet<Coordinate> newBeams = [];
    foreach (var beam in beams)
    {
        var newLoc = beam.Down;
        if (!splitters.Contains(newLoc))
        {
            newBeams.Add(newLoc);
            continue;
        }
        splittersHit.Add(newLoc);
        newBeams.Add(newLoc.Left);
        newBeams.Add(newLoc.Right);
    }
    beams = newBeams;
    // WriteMany(newBeams, '|');
}

// Console.CursorLeft = 0;
// Console.CursorTop = rows + 1;

Console.WriteLine($"Splitters hit: {splittersHit.Count}");

void WriteMany(IEnumerable<Coordinate> coordinates, char c)
{
    foreach (var coordinate in coordinates)
    {
        Write(coordinate, c);
    }
}

void Write(Coordinate coordinate, char c)
{
    Console.CursorLeft = coordinate.Column;
    Console.CursorTop = coordinate.Row;
    Console.Write(c);
}

record Coordinate(int Row, int Column)
{
    public static Coordinate operator +(Coordinate left, Coordinate right) => new(left.Row + right.Row, left.Column + right.Column);

    public Coordinate Down => this + new Coordinate(1, 0);
    public Coordinate Left => this + new Coordinate(0, -1);
    public Coordinate Right => this + new Coordinate(0, 1);
}
