// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var connections = File.ReadAllLines("input.txt")
    .Select(x => x.Split("-"))
    .SelectMany(x => new List<(string PC, string ConnectTo)> { (x[0], x[1]), (x[1], x[0]) })
    .GroupBy(x => x.PC, x => x.ConnectTo)
    .ToDictionary(x => x.Key, x => x.Select(x => x).ToHashSet());

var groups = connections.Select(x => new HashSet<string>() { x.Key }).ToList();

while (groups.Count > 1)
{
    Console.WriteLine(groups.Count);
    List<HashSet<string>> newGroups = [];

    foreach (var group in groups)
    {
        foreach (var pc in connections)
        {
            if (group.Contains(pc.Key))
            {
                continue;
            }
            if (group.All(x => pc.Value.Contains(x)))
            {
                newGroups.Add([.. group, pc.Key]);
            }
        }
    }

    groups = newGroups.DistinctBy(x => string.Join("-", x.Order())).ToList();
}

Console.WriteLine(string.Join(",", groups.First().Order()));
