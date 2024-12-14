// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<Game> games = File.ReadAllText("input.txt")
    .Split($"\n\n")
    .Select(x =>
    {
        var lines = x.Split("\n");
        return new Game(new Button(lines[0]), new Button(lines[1]), new Price(lines[2]));
    })
    .ToList();

Console.WriteLine(games.Select(x => x.Cheapest()).Sum());

record Distance(int X, int Y);

record Button(Distance Distance)
{
    public Distance GetDistance(int presses)
    {
        return new(presses * Distance.X, presses * Distance.Y);
    }

    public Button((int X, int Y) ints)
        : this(new Distance(ints.X, ints.Y)) { }

    public Button(string s)
        : this(ParseString(s)) { }

    public static (int X, int Y) ParseString(string s)
    {
        var ints = s.Split(":")[1].Split(",").Select(x => int.Parse(x.Split("+")[1])).ToArray();
        return (ints[0], ints[1]);
    }
}

record Price(Distance Distance)
{
    public Price((int X, int Y) ints)
        : this(new Distance(ints.X, ints.Y)) { }

    public Price(string s)
        : this(ParseString(s)) { }

    public static (int X, int Y) ParseString(string s)
    {
        var ints = s.Split(":")[1].Split(",").Select(x => int.Parse(x.Split("=")[1])).ToArray();
        return (ints[0], ints[1]);
    }
}

record Game(Button A, Button B, Price Price)
{
    public int Cheapest()
    {
        var maxA = Math.Min(Price.Distance.X / A.Distance.X, Price.Distance.Y - A.Distance.Y);

        var result = int.MaxValue;

        for (int i = 0; i <= maxA; i++)
        {
            var aLoc = A.GetDistance(i);

            var bRemainder = (Price.Distance.X - aLoc.X) % B.Distance.X;
            if (bRemainder != 0)
            {
                continue;
            }

            var bTimes = (Price.Distance.X - aLoc.X) / B.Distance.X;

            if (!(i * A.Distance.Y + bTimes * B.Distance.Y == Price.Distance.Y))
            {
                continue;
            }

            var innerResult = i * 3 + bTimes;
            result = Math.Min(innerResult, result);
        }
        return result == int.MaxValue ? 0 : result;
    }
}
