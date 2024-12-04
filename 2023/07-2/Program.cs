// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var result = File.ReadAllLines("input.txt")
    .Select(x => x.Split(" "))
    .Select(x => new Hand(x[0], int.Parse(x[1])))
    .OrderBy(x => x.CalculateValue())
    .Aggregate((TotalScore: 0, Rank: 1), (acc, src) => (acc.TotalScore + (src.Bid * acc.Rank), acc.Rank + 1))
    .Item1;

Console.WriteLine(result);

record struct Hand(string Cards, int Bid)
{
    static Dictionary<char, int> CardValues =
        new()
        {
            { '2', 1 },
            { '3', 2 },
            { '4', 3 },
            { '5', 4 },
            { '6', 5 },
            { '7', 6 },
            { '8', 7 },
            { '9', 8 },
            { 'T', 9 },
            { 'J', 0 },
            { 'Q', 11 },
            { 'K', 12 },
            { 'A', 13 }
        };

    public int CalculateValue()
    {
        return GetHandRank() * 1000000
            + Cards.Reverse().Aggregate((0, 1), (acc, src) => (acc.Item1 + CardValues[src] * acc.Item2, acc.Item2 * 14)).Item1;
    }

    int GetHandRank()
    {
        var jokerCount = Cards.Where(x => x == 'J').Count();
        var groups = Cards.Where(x => x != 'J').GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
        if (groups.Count <= 1)
            return 6;
        if (groups.Max(x => x.Value) + jokerCount == 4)
            return 5;
        if (groups.Count == 2)
            return 4;
        if (groups.Max(x => x.Value + jokerCount) == 3)
            return 3;
        if (groups.Count == 3)
            return 2;
        if (groups.Count == 4)
            return 1;
        return 0;
    }
}
