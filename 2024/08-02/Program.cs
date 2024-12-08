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

    public HashSet<Location> GetOverlaps(Distance distance, int multiplier)
    {
        var index = 1;
        HashSet<Location> overlaps = new();
        while (true)
        {
            var overlap = new Location(Row + distance.Row * multiplier * index, Column + distance.Column * multiplier * index);
            if (overlap.Row < 0 || overlap.Column < 0 || overlap.Row > 49 || overlap.Column > 49)
            {
                return overlaps;
            }
            overlaps.Add(overlap);
            index++;
        }
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
            overlaps.Add(location);
            foreach (var otherLocation in locations[index..])
            {
                var distance = location.GetDistance(otherLocation);
                foreach (var overlap in location.GetOverlaps(distance, -1))
                {
                    overlaps.Add(overlap);
                }
                foreach (var overlap in otherLocation.GetOverlaps(distance, +1))
                {
                    overlaps.Add(overlap);
                }
            }
            index++;
        }
        return overlaps;
    }
}
