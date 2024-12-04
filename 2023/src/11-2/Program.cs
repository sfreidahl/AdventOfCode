Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt").Select(x => x.ToList()).ToList();

var emptyColumns = new List<long>();
var emptyRows = new List<long>();

for (int i = input.Count - 1; i >= 0; i--)
{
    if (input[i].All(x => x == '.'))
    {
        emptyRows.Add(i);
    }
}

for (int i = input[0].Count - 1; i >= 0; i--)
{
    if (input.All(x => x[i] == '.'))
    {
        emptyColumns.Add(i);
    }
}
List<Location> locations = new List<Location>();
var rowIndex = 0;
var colIndex = 0;
foreach (var row in input)
{
    foreach (var col in row)
    {
        if (input[rowIndex][colIndex] == '#')
        {
            locations.Add(new Location(rowIndex, colIndex, emptyRows, emptyColumns));
        }
        colIndex++;
    }
    colIndex = 0;
    rowIndex++;
}

var locationsArray = locations.ToArray();
var result = locationsArray.Select((location, index) => locationsArray[(index)..].Select(x => x.CalculateDistance(location)).Sum()).Sum();

// Console.WriteLine(JsonSerializer.Serialize(locations));
Console.WriteLine(result);

record struct Location(long Row, long Column, List<long> EmptyRows, List<long> EmptyColumns)
{
    public long CalculateDistance(Location location)
    {
        var column = Column;
        var row = Row;
        var emptyHor = EmptyColumns.Where(x => x > Math.Min(column, location.Column) && x < Math.Max(column, location.Column)).Count();
        var emptyVert = EmptyRows.Where(x => x > Math.Min(row, location.Row) && x < Math.Max(row, location.Row)).Count();
        var hor = Math.Abs(Row - location.Row);
        var vert = Math.Abs(Column - location.Column);
        return hor + emptyHor * 999999 + vert + emptyVert * 999999;
    }
}
