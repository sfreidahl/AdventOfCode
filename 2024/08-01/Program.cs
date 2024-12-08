// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var antinodes = File.ReadAllLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((name, columnIndex) => new Antenna(name, new Location(rowIndex, columnIndex))))
    .Where(x => x.Name != '.')
    .GroupBy(x => x.Name, x => x)
    .SelectMany(x => x.Select(x => x.Location).ToList().GetFrequencyOverlaps())
    .ToHashSet();

Console.WriteLine(antinodes.Count);

public record struct Distance(int Row, int Column);

public record struct Location(int Row, int Column)
{
    public Distance GetDistance(Location location)
    {
        return new(location.Row - Row, location.Column - Column);
    }

    public Location GetAntinode(Location otherLocation)
    {
        var distance = GetDistance(otherLocation);
        return new(Row - distance.Row, Column - distance.Column);
    }
}

public record struct Antenna(char Name, Location Location);

public static class ListExtensions
{
    public static HashSet<Location> GetFrequencyOverlaps(this List<Location> locations)
    {
        HashSet<Location> antinodes = new HashSet<Location>();
        var index = 1;
        foreach (var location in locations)
        {
            foreach (var otherLocation in locations[index..])
            {
                antinodes.Add(location.GetAntinode(otherLocation));
                antinodes.Add(otherLocation.GetAntinode(location));
            }
            index++;
        }
        return antinodes.Where(x => x.Row >= 0 && x.Column >= 0 && x.Row <= 49 && x.Column <= 49).ToHashSet();
    }
}
