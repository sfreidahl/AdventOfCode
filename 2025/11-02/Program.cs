// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var connections = File.ReadAllLines("input.txt").Select(x => x.Split(" ")).ToDictionary(x => x[0].Replace(":", ""), x => x[1..].ToHashSet());

Dictionary<CacheKey, long> cache = [];

var result = GetPaths("svr", [], false, false);

Console.WriteLine(result);

long GetPaths(string location, HashSet<string> visited, bool fft, bool dac)
{
    var key = new CacheKey(location, fft, dac);
    HashSet<string> currentPath = [.. visited, location];
    if (cache.TryGetValue(key, out var value))
    {
        return value;
    }
    if (visited.Contains(location))
    {
        cache[key] = 0;
        return 0;
    }
    if (location == "out")
    {
        var r = fft && dac ? 1 : 0;
        cache[key] = r;
        return r;
    }

    var result = connections[location].Sum(x => GetPaths(x, currentPath, currentPath.Contains("fft"), currentPath.Contains("dac")));

    cache[key] = result;
    return result;
}

record CacheKey(string Location, bool Fft, bool Dac);
