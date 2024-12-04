// See https://aka.ms/new-console-template for more information
using System.Collections.Specialized;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var result = File.ReadAllLines("input.txt")
    .Select(x => x.Split(" "))
    .Select(x => new Record(x[0] + ".", x[1].Split(",").Select(y => int.Parse(y)).ToList()))
    .Select(x => x.ArrangementCount())
    .Sum();

Console.WriteLine(result);

record struct Record(string Springs, List<int> GroupSizes)
{
    public int ArrangementCount()
    {
        Console.WriteLine(Springs);
        var count = GetArrangement(0, 0, Springs, false);
        Console.WriteLine(count);
        return count; //arrangements.Count;
    }

    HashSet<string> values = new HashSet<string>();

    int GetArrangement(int groupIndex, int stringIndex, string group, bool isWildCard)
    {
        if (groupIndex == GroupSizes.Count && !Springs[stringIndex..].Contains('#'))
        {
            Console.WriteLine(group);
            if (values.Contains(group))
            {
                // Console.WriteLine(Springs);
                // Console.WriteLine(group);
            }
            values.Add(group);
            return 1;
        }
        if (stringIndex >= Springs.Length || groupIndex == GroupSizes.Count)
        {
            return 0;
        }

        while (Springs[stringIndex] == '.')
        {
            stringIndex++;
            if (stringIndex == Springs.Length)
            {
                return 0;
            }
        }

        var endIndex = Springs.IndexOf('.', stringIndex);
        if (endIndex == -1)
        {
            endIndex = Springs.Length;
        }

        var groupSize = GroupSizes[groupIndex];

        if (Springs[stringIndex..].Length < groupSize)
        {
            return 0;
        }

        var nextGrouping = Springs[stringIndex..endIndex];

        var count = 0;

        // var superWildCard = nextGrouping.All(x => x == '?');


        if (nextGrouping.Length < groupSize)
        {
            if (nextGrouping.Contains('#'))
            {
                return 0;
            }
            return count + GetArrangement(groupIndex, stringIndex + nextGrouping.Length, group, false);
        }
        if (nextGrouping.All(x => x == '?'))
        {
            // Console.WriteLine($"isWildCard: {isWildCard}, stringIndex: {stringIndex}");
        }
        if (nextGrouping.All(x => x == '?') && !isWildCard)
        {
            Console.WriteLine($"isWildCard: {isWildCard}, stringIndex: {stringIndex}");
            count += GetArrangement(groupIndex, stringIndex + nextGrouping.Length + 1, group, false);
        }

        if (nextGrouping.Length == groupSize)
        {
            return count
                + GetArrangement(
                    groupIndex + 1,
                    stringIndex + nextGrouping.Length + 1,
                    $"{group[..stringIndex]}{group[stringIndex..(stringIndex + groupSize)].Replace("?", "#")}{group[(stringIndex + groupSize)..]}",
                    false
                );
        }

        if ((nextGrouping.StartsWith('#') || isWildCard) && Springs[stringIndex + groupSize] == '?')
        {
            return count
                + GetArrangement(
                    groupIndex + 1,
                    stringIndex + groupSize + 1,
                    $"{group[..stringIndex]}{group[stringIndex..(stringIndex + groupSize)].Replace("?", "#")}{group[(stringIndex + groupSize)..]}",
                    false
                );
        }

        var extraIndex = 0;

        while (Springs[stringIndex + extraIndex] == '?' && !isWildCard && extraIndex <= (nextGrouping.Length - groupSize))
        {
            count += GetArrangement(groupIndex, stringIndex + extraIndex, group, true);
            extraIndex++;
        }
        if (Springs[stringIndex + extraIndex] == '#' && extraIndex > 0)
        {
            count += GetArrangement(groupIndex, stringIndex + extraIndex, group, false);
        }

        return count;
    }
}
