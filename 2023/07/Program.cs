// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var hands = File.ReadAllLines("input.txt").Select(x => x.Split(" ")).Select(x => new Hand(x[0], int.Parse(x[1]))).OrderBy(x => x.Value);

int index = 1;
var result = 0;
foreach (var hand in hands)
{
    Console.WriteLine($"Hand: {hand.Cards} - Value: {hand.Value}");
    result += hand.Bid * index;
    index++;
}

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

    private long _value = 0;
    public long Value
    {
        get
        {
            if (_value == 0)
            {
                _value = CalculateValue();
            }
            return _value;
        }
    }

    private long _jokerCount = 0;
    public long JokerCount
    {
        get
        {
            if (_jokerCount == 0)
            {
                _jokerCount = Cards.Where(x => x == 'J').Count();
            }
            return _jokerCount;
        }
    }

    private Dictionary<char, int>? _groups = null;
    public Dictionary<char, int> Groups
    {
        get
        {
            if (_groups is null)
            {
                _groups = Cards.GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
            }
            return _groups;
        }
    }

    long CalculateValue()
    {
        var value = IsFive() ?? IsFour() ?? IsHouse() ?? IsThree() ?? IsTwoPair() ?? IsOnePair() ?? 0;
        return value * 10000000000
            + CardValues[Cards[0]] * 100000000
            + CardValues[Cards[1]] * 1000000
            + CardValues[Cards[2]] * 10000
            + CardValues[Cards[3]] * 100
            + CardValues[Cards[4]];
    }

    int? IsFive()
    {
        return Groups.Count == 1 ? 6 : null;
    }

    int? IsFour()
    {
        return Groups.Max(x => x.Value) == 4 ? 5 : null;
    }

    int? IsHouse()
    {
        return Groups.Count == 2 && Groups.Max(x => x.Value) == 3 ? 4 : null;
    }

    int? IsThree()
    {
        return Groups.Max(x => x.Value) == 3 ? 3 : null;
    }

    int? IsTwoPair()
    {
        return Groups.Count == 3 && Groups.Max(x => x.Value) == 2 ? 2 : null;
    }

    int? IsOnePair()
    {
        return Groups.Count == 4 ? 1 : null;
    }
}
