// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var seedsData = File.ReadAllText("seeds.txt").Split(" ").Select(x => long.Parse(x)).ToArray();
var seeds = new List<Range>();
var remainingSeeds = seedsData;
while (true)
{
    seeds.Add(new Range(remainingSeeds[0], remainingSeeds[1]));
    if (remainingSeeds.Length == 2)
        break;
    remainingSeeds = remainingSeeds[2..];
}

var seedToSoil = new Map(File.ReadAllLines("seed-to-soil.txt").Select(x => new MapItem(x)));
var soilToFertilizer = new Map(File.ReadAllLines("soil-to-fertilizer.txt").Select(x => new MapItem(x)));
var fertilizerToWater = new Map(File.ReadAllLines("fertilizer-to-water.txt").Select(x => new MapItem(x)));
var waterToLight = new Map(File.ReadAllLines("water-to-light.txt").Select(x => new MapItem(x)));
var lightToTemperature = new Map(File.ReadAllLines("light-to-temperature.txt").Select(x => new MapItem(x)));
var temperatureToHumidity = new Map(File.ReadAllLines("temperature-to-humidity.txt").Select(x => new MapItem(x)));
var humidityToLocation = new Map(File.ReadAllLines("humidity-to-location.txt").Select(x => new MapItem(x)));

var location = humidityToLocation.GetDestination(
    temperatureToHumidity.GetDestination(
        lightToTemperature.GetDestination(
            waterToLight.GetDestination(fertilizerToWater.GetDestination(soilToFertilizer.GetDestination(seedToSoil.GetDestination(seeds))))
        )
    )
);

Console.WriteLine(location.Min(x => x.Start));

record struct Map(IEnumerable<MapItem> Items)
{
    public List<Range> GetDestination(List<Range> source)
    {
        var remaining = source;
        var result = new List<Range>();
        foreach (var item in Items)
        {
            var newRemaining = new List<Range>();
            foreach (var rem in remaining)
            {
                var res = item.GetDestination(rem);
                newRemaining.AddRange(res.Item2);
                if (res.Item1 != null)
                {
                    result.Add((Range)res.Item1);
                }
            }
            remaining = newRemaining;
        }

        remaining.AddRange(remaining);
        return result;
    }
};

record struct MapItem(long DestinationStart, long SourceStart, long Length)
{
    public long MaxSource => SourceStart + (Length - 1);
    public long MaxDestination => DestinationStart + (Length - 1);

    public MapItem(string input) : this(input.Split(" ")) { }

    public MapItem(string[] input) : this(long.Parse(input[0]), long.Parse(input[1]), long.Parse(input[2])) { }

    public (Range?, List<Range>) GetDestination(Range source)
    {
        if (!(source.Start <= this.MaxSource && source.End >= this.SourceStart))
        {
            return (null, new List<Range>() { source });
        }

        if (source.Start >= this.SourceStart && source.End <= this.MaxSource)
        {
            var r = new Range(this.DestinationStart + source.Start - this.SourceStart, source.Length);
            return (r, new List<Range>() { });
        }

        List<Range> remaining = new();

        if (source.Start < this.SourceStart)
        {
            remaining.Add(new Range(source.Start, this.SourceStart - source.Start));
        }

        if (source.End > this.MaxSource)
        {
            remaining.Add(new Range(this.MaxSource + 1, source.End - this.MaxSource));
        }

        if (source.End >= this.SourceStart && source.Start <= this.MaxSource)
        {
            var end = Math.Min(source.End, this.MaxSource);
            var start = Math.Max(source.Start, this.SourceStart);
            var r = new Range(this.DestinationStart + start - this.SourceStart, (end - start) + 1);
            return (r, remaining);
        }

        return (null, remaining);
    }
};

record struct Range(long Start, long Length)
{
    public long End => Start + (Length - 1);
};
