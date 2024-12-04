// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, World!");

var input = File.ReadAllText("input.txt").Split("\n\n");

var items = input[1]
    .Split("\n")
    .Select(x => x.Replace("{", "").Replace("}", "").Split(","))
    .Select(
        y =>
            new Item(
                new Dictionary<char, int>()
                {
                    { 'x', int.Parse(y[0][2..]) },
                    { 'm', int.Parse(y[1][2..]) },
                    { 'a', int.Parse(y[2][2..]) },
                    { 's', int.Parse(y[3][2..]) }
                }
            )
    );

var workflows = input[0]
    .Split("\n")
    .Select(x => x.Split("{"))
    .Select(
        x =>
            new Workflow(
                x[0],
                x[1][..x[1].LastIndexOf(',')]
                    .Split(",")
                    .Select(y => new Command(y[0], ConditionFromChar(y[1]), int.Parse(y[2..y.IndexOf(':')]), y[(y.IndexOf(':') + 1)..]))
                    .ToList(),
                x[1][(x[1].LastIndexOf(',') + 1)..^1]
            )
    )
    .ToDictionary(x => x.Label, x => x);

var approved = new List<Item>();
var rejected = new List<Item>();

foreach (var item in items)
{
    Workflow nextWorkflow = workflows["in"];
    while (true)
    {
        string next = nextWorkflow.GetNext(item);
        if (next == "A")
        {
            approved.Add(item);
            break;
        }
        if (next == "R")
        {
            rejected.Add(item);
            break;
        }
        nextWorkflow = workflows[next];
    }
}

Console.WriteLine(approved.Select(x => x.Values.Values.Sum()).Sum());

Condition ConditionFromChar(char c)
{
    return c switch
    {
        '>' => Condition.GreaterThan,
        '<' => Condition.SmallerThan
    };
}

record struct Item(Dictionary<char, int> Values);

record struct Workflow(string Label, List<Command> Commands, string DefaultDestination)
{
    public string GetNext(Item item)
    {
        foreach (var Command in Commands)
        {
            var value = item.Values[Command.Category];
            if (Command.Condition == Condition.GreaterThan && value > Command.Value)
            {
                return Command.Destination;
            }
            if (Command.Condition == Condition.SmallerThan && value < Command.Value)
            {
                return Command.Destination;
            }
        }
        return DefaultDestination;
    }
}

record struct Command(char Category, Condition Condition, int Value, string Destination);

enum Condition
{
    GreaterThan,
    SmallerThan
}
