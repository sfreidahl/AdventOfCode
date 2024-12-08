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

    public HashSet<Location> GetAntinodes(Location otherLocation)
    {
        var index = 1;
        HashSet<Location> antinodes = [];
        var distance = GetDistance(otherLocation);
        while (true)
        {
            var antinode = new Location(Row - distance.Row * index, Column - distance.Column * index);
            if (IsOutOfBounds(antinode))
            {
                return antinodes;
            }
            antinodes.Add(antinode);
            index++;
        }
    }

    private static bool IsOutOfBounds(Location location) => location.Row < 0 || location.Column < 0 || location.Row > 49 || location.Column > 49;
}

public record struct Antenna(char Name, Location Location);

public static class ListExtensions
{
    public static HashSet<Location> GetFrequencyOverlaps(this List<Location> locations)
    {
        HashSet<Location> antinodes = [];
        var index = 1;
        foreach (var location in locations)
        {
            antinodes.Add(location);
            foreach (var otherLocation in locations[index..])
            {
                antinodes.AddRange(location.GetAntinodes(otherLocation));
                antinodes.AddRange(otherLocation.GetAntinodes(location));
            }
            index++;
        }
        return antinodes;
    }
}

public static class HasSetExtensions
{
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
    {
        foreach (var item in values)
        {
            set.Add(item);
        }
    }
}
