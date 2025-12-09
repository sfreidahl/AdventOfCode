Console.WriteLine("Hello, World!");

var redSquares = File.ReadAllLines("input.txt")
    .Select(x => x.Split(",").Select(int.Parse).ToArray())
    .Select((x, index) => new Coordinate(x[0], x[1]))
    .ToList();

List<Rectangle> rectangles = [];

for (int i = 0; i < redSquares.Count; i++)
{
    for (int j = i + 1; j < redSquares.Count; j++)
    {
        rectangles.Add(new(redSquares[i], redSquares[j]));
    }
}

List<Line> verticalLines = [];
List<Line> horizontalLines = [];

for (int i = 0; i < redSquares.Count; i++)
{
    var second = i == redSquares.Count - 1 ? 0 : i + 1;
    var line = new Line(redSquares[i], redSquares[second]);
    if (line.IsVertical)
    {
        verticalLines.Add(line);
    }
    else
    {
        horizontalLines.Add(line);
    }
}

foreach (var rectangle in rectangles.OrderByDescending(x => x.Size))
{
    if (CheckRectangle(rectangle))
    {
        Console.WriteLine(rectangle.Size);
        break;
    }
}

bool CheckRectangle(Rectangle rectangle)
{
    foreach (var line in rectangle.Lines)
    {
        if (!CheckCorner(line.One, line.IsVertical) || !CheckCorner(line.Two, line.IsVertical))
        {
            return false;
        }
        if (!CheckLine(line))
        {
            return false;
        }
    }
    return true;
}

bool CheckCorner(Coordinate corner, bool IsVertical)
{
    var lines = IsVertical
        ? verticalLines.Where(x => x.Top <= corner.Row && x.Bottom > corner.Row && x.LinePosition < corner.Column)
        : horizontalLines.Where(x => x.Left <= corner.Column && x.Right > corner.Column && x.LinePosition < corner.Row);

    return lines.Count() % 2 == 1;
}

bool CheckLine(Line side)
{
    var lines = side.IsVertical ? horizontalLines.Where(x => x.Intersects(side)) : verticalLines.Where(x => x.Intersects(side));

    return !lines.Any();
}

record Corder(Coordinate coordinate);

record Coordinate(long Column, long Row);

record Line(Coordinate One, Coordinate Two)
{
    public bool IsVertical = One.Column == Two.Column;

    public long Left = Math.Min(One.Column, Two.Column);
    public long Right = Math.Max(One.Column, Two.Column);
    public long Top = Math.Min(One.Row, Two.Row);
    public long Bottom = Math.Max(One.Row, Two.Row);
    public long LinePosition => IsVertical ? One.Column : Two.Row;

    public bool Intersects(Line line) =>
        IsVertical
            ? line.Bottom > Top && line.Bottom < Bottom && Left > line.Left && Left < line.Right
            : Bottom > line.Top && Bottom < line.Bottom && line.Left > Left && line.Left < Right;
}

record Rectangle(Coordinate One, Coordinate Two)
{
    public long Size => (Math.Abs(One.Row - Two.Row) + 1) * (Math.Abs(One.Column - Two.Column) + 1);

    public List<Line> Lines =>
        [
            new(new(LeftMinus, TopMinus), new(RightMinus, TopMinus)),
            new(new(LeftMinus, BottomMinus), new(RightMinus, BottomMinus)),
            new(new(LeftMinus, BottomMinus), new(LeftMinus, TopMinus)),
            new(new(RightMinus, BottomMinus), new(RightMinus, TopMinus)),
        ];

    public long LeftMinus => Math.Min(One.Column, Two.Column) + 1;
    public long RightMinus => Math.Max(One.Column, Two.Column) - 1;
    public long TopMinus => Math.Min(One.Row, Two.Row) + 1;
    public long BottomMinus => Math.Max(One.Row, Two.Row) - 1;
}
