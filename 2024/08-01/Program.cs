// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((name, columnIndex) => new Antenna(name, new Location(rowIndex, columnIndex))))
    .Where(x => x.Name != '.')
    .GroupBy(x => x.Name, x => x)
    .ToDictionary(x => x.Key, x => x.Select(x => x).ToList())
    .SelectMany(x => x.Value.Select(x => x.Location).ToList().GetFrequencyOverlaps())
    .ToHashSet();

Console.WriteLine(input.Count);

public record struct Distance(int Row, int Column);

public record struct Location(int Row, int Column)
{
    public Distance GetDistance(Location location)
    {
        return new(location.Row - Row, location.Column - Column);
    }

    public Location GetOverlap(Distance distance, int multiplier)
    {
        return new(Row + distance.Row * multiplier, Column + distance.Column * multiplier);
    }
}

public record struct Antenna(char Name, Location Location);

public static class ListExtensions
{
    public static HashSet<Location> GetFrequencyOverlaps(this List<Location> locations)
    {
        HashSet<Location> overlaps = new HashSet<Location>();
        var index = 1;
        foreach (var location in locations)
        {
            foreach (var otherLocation in locations[index..])
            {
                var distance = location.GetDistance(otherLocation);
                overlaps.Add(location.GetOverlap(distance, -1));
                overlaps.Add(otherLocation.GetOverlap(distance, 1));
            }
            index++;
        }
        return overlaps.Where(x => x.Row >= 0 && x.Column >= 0 && x.Row <= 49 && x.Column <= 49).ToHashSet();
    }
}
