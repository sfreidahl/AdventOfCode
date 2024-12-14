// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var robots = File.ReadAllLines("input.txt")
    .Select(x =>
        x.Split(" ").Select(x => x.Replace("p=", "").Replace("v=", "").Split(",").Select(int.Parse).ToList()).Select(x => (X: x[0], Y: x[1])).ToList()
    )
    .Select(x => (Position: x[0], Velocity: x[1]))
    .Select(x => new Robot(new(x.Position), new(x.Velocity)));

var movedRobots = robots.Select(x => x.Move(100)).ToList();

var q1 = movedRobots.Where(x => x.IsInQuadrant(Quadrant.TopLeft)).Count();
var q2 = movedRobots.Where(x => x.IsInQuadrant(Quadrant.TopRight)).Count();
var q3 = movedRobots.Where(x => x.IsInQuadrant(Quadrant.BottomLeft)).Count();
var q4 = movedRobots.Where(x => x.IsInQuadrant(Quadrant.BottomRight)).Count();

Console.WriteLine($"{q1} * {q2} * {q3} * {q4}");
Console.WriteLine(q1 * q2 * q3 * q4);

record Robot(Position Position, Velocity Velocity)
{
    public Robot Move(int moveCount)
    {
        var x = (Position.X + Velocity.X * moveCount) % Width;
        var y = (Position.Y + Velocity.Y * moveCount) % Height;
        if (x < 0)
        {
            x = Width + x;
        }
        if (y < 0)
        {
            y = Height + y;
        }

        Console.WriteLine($"{Position} - {Velocity} - {Position.X + Velocity.X * moveCount},{Position.Y + Velocity.Y * moveCount} - {x},{y}");

        return this with
        {
            Position = new(x, y),
        };
    }

    public bool IsInQuadrant(Quadrant quadrant)
    {
        return quadrant switch
        {
            Quadrant.TopLeft => Position.X < Left && Position.Y < Top,
            Quadrant.TopRight => Position.X >= Right && Position.Y < Top,
            Quadrant.BottomLeft => Position.X < Left && Position.Y >= Bottom,
            Quadrant.BottomRight => Position.X >= Right && Position.Y >= Bottom,
            _ => false,
        };
    }

    static int Left => Width / 2;
    static int Right => Width - Width / 2;
    static int Top => Height / 2;
    static int Bottom => Height - Height / 2;

    // const int Width = 11;
    // const int Height = 7;
    const int Width = 101;
    const int Height = 103;
}

record Position(int X, int Y)
{
    public Position((int X, int Y) p)
        : this(p.X, p.Y) { }
}

record Velocity(int X, int Y)
{
    public Velocity((int X, int Y) p)
        : this(p.X, p.Y) { }
}

enum Quadrant
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}
