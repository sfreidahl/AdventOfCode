// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var robots = File.ReadAllLines("input.txt")
    .Select(x =>
        x.Split(" ").Select(x => x.Replace("p=", "").Replace("v=", "").Split(",").Select(int.Parse).ToList()).Select(x => (X: x[0], Y: x[1])).ToList()
    )
    .Select(x => (Position: x[0], Velocity: x[1]))
    .Select(x => new Robot(new(x.Position), new(x.Velocity)));

var index = 0;
robots = robots.Select(x => x.Move(86 + 101 * 69));
PrintRobots(robots);
while (Console.ReadLine() != "X")
{
    robots = robots.Select(x => x.Move(101));
    PrintRobots(robots);
    index++;
    Console.SetCursorPosition(index * 5, 103 + 1);
    Console.Write(index);
}

Console.WriteLine(index);

void PrintRobots(IEnumerable<Robot> robots)
{
    Console.Clear();
    foreach (var robot in robots)
    {
        Console.SetCursorPosition(robot.Position.X, robot.Position.Y);
        Console.Write("X");
    }
}

record Robot(Position Position, Velocity Velocity)
{
    public Robot Move(int moves)
    {
        var x = (Position.X + Velocity.X * moves) % Width;
        var y = (Position.Y + Velocity.Y * moves) % Height;
        if (x < 0)
        {
            x = Width + x;
        }
        if (y < 0)
        {
            y = Height + y;
        }

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
    const int patternX = 86;
    const int Height = 103;
    const int patternY = 51;
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
