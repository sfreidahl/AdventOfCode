// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, World!");

var input = File.ReadAllText("input.txt")
    .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Split("\n", StringSplitOptions.RemoveEmptyEntries));

var result = 0;
foreach (var group in input)
{
    var hor = (CompareDown(group) + CompareUp(group)) * 100 + CompareHor(group);
    Console.WriteLine(hor);
    result += hor;
}

Console.WriteLine(result);

int CompareHor(string[] group)
{
    foreach (var l in group)
    {
        Console.WriteLine(l);
    }
    Console.WriteLine();
    List<string> newList = new List<string>();
    for (int i = 0; i < group[0].Length; i++)
    {
        var line = "";
        for (int j = 0; j < group.Length; j++)
        {
            line += group[j][i];
        }
        newList.Add(line);
    }
    var swappedGroup = newList.ToArray();
    foreach (var l in swappedGroup)
    {
        Console.WriteLine(l);
    }
    return CompareUp(swappedGroup) + CompareDown(swappedGroup);
}

int CompareDown(string[] group)
{
    for (int i = 1; i <= group.Length / 2; i++)
    {
        var over = group[..i];
        var under = group[i..(i * 2)];
        if (Mirror(over, under))
        {
            return i;
        }
    }
    return 0;
}

int CompareUp(string[] group)
{
    var groupAsList = group.ToList();
    groupAsList.Reverse();
    var swappedGroup = groupAsList.ToArray();
    var r = CompareDown(swappedGroup);
    if (r == 0)
    {
        return 0;
    }
    return group.Length - r;
}

bool Mirror(string[] over, string[] under)
{
    var underList = under.ToList();
    underList.Reverse();
    var underFlipped = underList.ToArray();
    foreach (var line in over)
    {
        // Console.WriteLine(line);
    }
    // Console.WriteLine("----------");
    foreach (var line in under)
    {
        // Console.WriteLine(line);
    }
    // Console.WriteLine();
    for (int i = 0; i < over.Length; i++)
    {
        if (over[i] != underFlipped[i])
        {
            return false;
        }
    }

    return true;
}
