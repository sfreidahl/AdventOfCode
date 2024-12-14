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

record Button(long X, long Y)
{
    public Button((long X, long Y) ints)
        : this(ints.X, ints.Y) { }

    public Button(string s)
        : this(ParseString(s)) { }

    public static (long X, long Y) ParseString(string s)
    {
        var ints = s.Split(":")[1].Split(",").Select(x => long.Parse(x.Split("+")[1])).ToArray();
        return (ints[0], ints[1]);
    }
}

record Price(long X, long Y)
{
    public Price((long X, long Y) ints)
        : this(ints.X, ints.Y) { }

    public Price(string s)
        : this(ParseString(s)) { }

    public static (long X, long Y) ParseString(string s)
    {
        var ints = s.Split(":")[1].Split(",").Select(x => long.Parse(x.Split("=")[1])).ToArray();
        return (ints[0] + 10000000000000, ints[1] + 10000000000000);
    }
}

record Game(Button A, Button B, Price Price)
{
    public long Cheapest()
    {
        var e1 = new Equation(A.X, B.X, Price.X);
        var e2 = new Equation(A.Y, B.Y, Price.Y);

        var a = TwoUnknowns(e1, e2);

        if (a == null)
        {
            return 0;
        }
        return a.Value.X * 3 + a.Value.Y;
    }

    public static (long X, long Y)? TwoUnknowns(Equation e1, Equation e2)
    {
        long[,] eliminator = new long[2, 2];

        eliminator[0, 0] = e2.Y * e1.X;
        eliminator[0, 1] = e2.Y * e1.Answer;
        eliminator[1, 0] = e1.Y * e2.X;
        eliminator[1, 1] = e1.Y * e2.Answer;

        var xRemainder = (eliminator[0, 1] - eliminator[1, 1]) % (eliminator[0, 0] - eliminator[1, 0]);
        var x = (eliminator[0, 1] - eliminator[1, 1]) / (eliminator[0, 0] - eliminator[1, 0]);
        var yRemainder = (e1.Answer - e1.X * x) % e1.Y;
        var y = (e1.Answer - e1.X * x) / e1.Y;

        if (xRemainder != 0 || yRemainder != 0)
        {
            return null;
        }

        return (x, y);
    }
}

record Equation(long X, long Y, long Answer);
