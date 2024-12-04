// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt").Select(x => x.ToList()).ToList();
var extraLine = new List<char>();

for (int i = 0; i < input[0].Count; i++)
{
    extraLine.Add('.');
}

for (int i = input.Count - 1; i >= 0; i--)
{
    if (input[i].All(x => x == '.'))
    {
        input.Insert(i, extraLine.ToList());
    }
}

for (int i = input[0].Count - 1; i >= 0; i--)
{
    if (input.All(x => x[i] == '.'))
    {
        input.ForEach(x => x.Insert(i, '.'));
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
            locations.Add(new Location(rowIndex, colIndex));
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

record struct Location(int Row, int Column)
{
    public int CalculateDistance(Location location)
    {
        var hor = Math.Abs(Row - location.Row);
        var vert = Math.Abs(Column - location.Column);
        return hor + vert;
    }
}
