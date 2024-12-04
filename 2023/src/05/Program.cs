// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var seeds = File.ReadAllText("seeds.txt").Split(" ").Select(x => long.Parse(x));

var seedToSoil = new Map(File.ReadAllLines("seed-to-soil.txt").Select(x => new MapItem(x)));
var soilToFertilizer = new Map(File.ReadAllLines("soil-to-fertilizer.txt").Select(x => new MapItem(x)));
var fertilizerToWater = new Map(File.ReadAllLines("fertilizer-to-water.txt").Select(x => new MapItem(x)));
var waterToLight = new Map(File.ReadAllLines("water-to-light.txt").Select(x => new MapItem(x)));
var lightToTemperature = new Map(File.ReadAllLines("light-to-temperature.txt").Select(x => new MapItem(x)));
var temperatureToHumidity = new Map(File.ReadAllLines("temperature-to-humidity.txt").Select(x => new MapItem(x)));
var humidityToLocation = new Map(File.ReadAllLines("humidity-to-location.txt").Select(x => new MapItem(x)));

var minLocation = long.MaxValue;

foreach (var seed in seeds)
{
    var location = humidityToLocation.GetDestination(
        temperatureToHumidity.GetDestination(
            lightToTemperature.GetDestination(
                waterToLight.GetDestination(fertilizerToWater.GetDestination(soilToFertilizer.GetDestination(seedToSoil.GetDestination(seed))))
            )
        )
    );
    minLocation = Math.Min(minLocation, location);
}

Console.WriteLine(minLocation);

record struct Map(IEnumerable<MapItem> Items)
{
    public long GetDestination(long source)
    {
        foreach (var map in Items)
        {
            var destination = map.GetDestination(source);
            if (destination != -1)
                return destination;
        }
        return source;
    }
};

record struct MapItem(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
{
    public MapItem(string input) : this(input.Split(" ")) { }

    public MapItem(string[] input) : this(long.Parse(input[0]), long.Parse(input[1]), long.Parse(input[2])) { }

    public long GetDestination(long source)
    {
        if (source > SourceRangeStart && source < SourceRangeStart + (RangeLength - 1))
        {
            return DestinationRangeStart + source - SourceRangeStart;
        }
        return -1;
    }
};
