Console.WriteLine("Hello, World!");

var result = File.ReadAllLines("input.txt")
    .Select(x => x.Split(" "))
    .Select(
        x =>
            new Record(
                $"{x[0]}?{x[0]}?{x[0]}?{x[0]}?{x[0]}" + ".",
                $"{x[1]},{x[1]},{x[1]},{x[1]},{x[1]}".Split(",").Select(y => int.Parse(y)).ToList()
            )
    )
    .Select(x => x.ArrangementCount())
    .Sum();

Console.WriteLine(result);

record struct Data(int GroupIndex, int StringIndex, bool isWildCard, bool isKindaWildCard = false);

record struct Record(string Springs, List<int> GroupSizes)
{
    public long ArrangementCount()
    {
        var count = GetArrangement(new Data(0, 0, false));
        Console.WriteLine(count);
        return count; //arrangements.Count;
    }

    public Dictionary<Data, long> dic = new Dictionary<Data, long>();

    long GetArrangement(Data d)
    {
        if (dic.ContainsKey(d))
        {
            return dic[d];
        }
        var groupIndex = d.GroupIndex;
        var stringIndex = d.StringIndex;
        var isWildCard = d.isWildCard;

        if (groupIndex == GroupSizes.Count && !Springs[stringIndex..].Contains('#'))
        {
            dic.Add(d, 1);
            return 1;
        }
        if (stringIndex >= Springs.Length || groupIndex == GroupSizes.Count)
        {
            dic.Add(d, 0);
            return 0;
        }

        while (Springs[stringIndex] == '.')
        {
            stringIndex++;
            if (stringIndex == Springs.Length)
            {
                dic.Add(d, 0);
                return 0;
            }
        }

        var endIndex = Springs.IndexOf('.', stringIndex);
        if (endIndex == -1)
        {
            endIndex = Springs.Length;
        }

        var groupSize = GroupSizes[groupIndex];
        var nextGrouping = Springs[stringIndex..endIndex];

        if (nextGrouping.Length < groupSize)
        {
            if (nextGrouping.Contains('#'))
            {
                dic.Add(d, 0);
                return 0;
            }
            var r = GetArrangement(new Data(groupIndex, stringIndex + 1, false));

            dic.Add(d, r);
            return r;
        }
        long count = 0;
        if (nextGrouping.All(x => x == '?') && !isWildCard)
        {
            count += GetArrangement(new Data(groupIndex, stringIndex + nextGrouping.Length + 1, false));
        }

        if (nextGrouping.Length == groupSize)
        {
            var r = count + GetArrangement(new Data(groupIndex + 1, stringIndex + nextGrouping.Length + 1, false));
            dic.Add(d, r);
            return r;
        }

        if ((nextGrouping.StartsWith('#') || isWildCard) && Springs[stringIndex + groupSize] == '?')
        {
            var r = count + GetArrangement(new Data(groupIndex + 1, stringIndex + groupSize + 1, false));
            dic.Add(d, r);
            return r;
        }

        var extraIndex = 0;

        while (Springs[stringIndex + extraIndex] == '?' && !isWildCard && extraIndex <= (nextGrouping.Length - groupSize))
        {
            count += GetArrangement(new Data(groupIndex, stringIndex + extraIndex, true));
            extraIndex++;
        }
        if (Springs[stringIndex + extraIndex] == '#' && extraIndex > 0)
        {
            count += GetArrangement(new Data(groupIndex, stringIndex + extraIndex, false, true));
        }

        dic.Add(d, count);
        return count;
    }
}
