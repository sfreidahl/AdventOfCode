Console.WriteLine("Hello, World!");

var input = File.ReadAllText("input.txt").Split("\n\n");

var workflows = input[0]
    .Split("\n")
    .Select(x => x.Split("{"))
    .Select(
        x =>
            new Workflow(
                x[0],
                x[1][..x[1].LastIndexOf(',')]
                    .Split(",")
                    .Select(y => new Command(y[0], ConditionFromChar(y[1]), long.Parse(y[2..y.IndexOf(':')]), y[(y.IndexOf(':') + 1)..]))
                    .ToList(),
                x[1][(x[1].LastIndexOf(',') + 1)..^1]
            )
    )
    .ToDictionary(x => x.Label, x => x);

var approved = new List<Item>();
var rejected = new List<Item>();

var ranges = new List<RangeWithLabel>()
{
    new(
        "in",
        new Item(
            new Dictionary<char, Range>()
            {
                { 'x', new Range(1, 4000) },
                { 'm', new Range(1, 4000) },
                { 'a', new Range(1, 4000) },
                { 's', new Range(1, 4000) }
            }
        )
    )
};

while (ranges.Count > 0)
{
    var newRanges = new List<RangeWithLabel>();
    foreach (var range in ranges)
    {
        if (range.Label == "R")
        {
            rejected.Add(range.Item);
            continue;
        }
        if (range.Label == "A")
        {
            approved.Add(range.Item);
            continue;
        }
        newRanges.AddRange(workflows[range.Label].GetNext(range.Item));
    }
    ranges = newRanges;
}

Console.WriteLine(approved.Sum(x => x.GetCombinations()));

Condition ConditionFromChar(char c)
{
    return c switch
    {
        '>' => Condition.GreaterThan,
        '<' => Condition.SmallerThan
    };
}

record struct Item(Dictionary<char, Range> Values)
{
    public long GetCombinations()
    {
        return (Values['x'].Max - Values['x'].Min + 1)
            * (Values['m'].Max - Values['m'].Min + 1)
            * (Values['a'].Max - Values['a'].Min + 1)
            * (Values['s'].Max - Values['s'].Min + 1);
    }
};

record struct Range(long Min, long Max);

record struct Workflow(string Label, List<Command> Commands, string DefaultDestination)
{
    public List<RangeWithLabel> GetNext(Item item)
    {
        List<RangeWithLabel> newRanges = new List<RangeWithLabel>();
        Dictionary<char, Range> defaultDic = item.Values.ToDictionary(x => x.Key, x => x.Value);
        foreach (var command in Commands)
        {
            Dictionary<char, Range> newDic = defaultDic.ToDictionary(x => x.Key, x => x.Value);
            var value = defaultDic[command.Category];

            if (command.Condition == Condition.GreaterThan && value.Max > command.Value)
            {
                newDic[command.Category] = new Range(command.Value + 1, value.Max);
                defaultDic[command.Category] = new Range(value.Min, command.Value);
            }
            if (command.Condition == Condition.SmallerThan && value.Min < command.Value)
            {
                newDic[command.Category] = new Range(value.Min, command.Value - 1);
                defaultDic[command.Category] = new Range(command.Value, value.Max);
            }

            newRanges.Add(new(command.Destination, new Item(newDic.ToDictionary(x => x.Key, x => x.Value))));
        }

        newRanges.Add(new(DefaultDestination, new Item(defaultDic)));

        return newRanges;
    }
}

record struct RangeWithLabel(string Label, Item Item);

record struct Command(char Category, Condition Condition, long Value, string Destination);

enum Condition
{
    GreaterThan,
    SmallerThan
}
