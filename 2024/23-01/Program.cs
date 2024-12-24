// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<HashSet<string>> originalGroups = File.ReadAllLines("input.txt")
    .Select(x => string.Join("-", x.Split("-").Order()))
    .Order()
    .Select(x => x.Split("-").ToHashSet())
    .DistinctBy(x => string.Join("-", x))
    .ToList();

var result = 0;
while (originalGroups.Count > 0)
{
    var group = originalGroups.First();
    originalGroups.Remove(group);
    string first = group.First();
    string last = group.Last();
    var firstGroups = originalGroups.Where(x => x.Contains(first));
    foreach (var f in firstGroups)
    {
        var test = f.Where(x => x != first).First();
        var results = originalGroups.Where(x => x.Contains(test) && x.Contains(last));
        foreach (var s in results)
        {
            HashSet<string> strings = [first, test, .. s];
            if (strings.Any(x => x.StartsWith('t')))
            {
                result++;
            }
        }
    }
}

Console.WriteLine(result);
