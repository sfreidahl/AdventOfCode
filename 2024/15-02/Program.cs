// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

const char Up = '^';
const char Down = 'v';
const char Left = '<';
const char Right = '>';

var map = File.ReadAllLines("input-map.txt")
    .Select(x => x.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@."))
    .SelectMany((row, rowIndex) => row.Select((c, columnIndex) => (c, new Coordinate(rowIndex, columnIndex))))
    .Where(x => x.c != '.')
    .ToDictionary(x => x.Item2, x => x.c);

var instructions = File.ReadAllLines("input-movement.txt")
    .SelectMany(x => x.Select(x => x))
    .Select(x =>
        x switch
        {
            Up => Direction.Up,
            Down => Direction.Down,
            Left => Direction.Left,
            Right => Direction.Right,
            _ => throw new UnreachableException(),
        }
    )
    .ToList();

var wareHouse = new WareHouse(map);

// wareHouse.Print();

await wareHouse.MoveRobot(instructions);

wareHouse.Print();

Console.WriteLine(wareHouse.Result);

class WareHouse(Dictionary<Coordinate, char> Map)
{
    const char Wall = '#';
    const char Robot = '@';
    const char CrateLeft = '[';
    const char CrateRight = ']';

    private Dictionary<Coordinate, char> _map = Map;
    private Coordinate _robot => _map.Where(x => x.Value == Robot).First().Key;

    public int Result => _map.Where(x => x.Value == CrateLeft).Select(x => x.Key).Select(x => x.Row * 100 + x.Column).Sum();

    public void Print()
    {
        Console.Clear();
        foreach (var s in _map)
        {
            Console.SetCursorPosition(s.Key.Column, s.Key.Row);
            Console.Write(s.Value);
        }
        Console.SetCursorPosition(0, _map.Max(x => x.Key.Row) + 1);
    }

    public async Task MoveRobot(List<Direction> moves)
    {
        Print();
        foreach (var move in moves)
        {
            await Task.Delay(1);
            MoveRobot(move);
            // Print();
        }
    }

    private void MoveRobot(Direction direction)
    {
        if (!TryMove(_robot, direction))
        {
            return;
        }
        Move(_robot, direction);
    }

    private bool TryMove(Coordinate from, Direction direction)
    {
        var move = d[direction];
        var newLoc = from.GetRelativeCoordinate(move);
        if (!_map.TryGetValue(newLoc, out var n))
        {
            return true;
        }
        if (n == Wall)
        {
            return false;
        }
        if (direction == Direction.Left || direction == Direction.Right)
        {
            return TryMove(newLoc, direction);
        }

        if (_map[newLoc] == CrateLeft)
        {
            if (!(TryMove(newLoc, direction) && TryMove(newLoc.GetRelativeCoordinate(d[Direction.Right]), direction)))
            {
                return false;
            }
        }

        if (_map[newLoc] == CrateRight)
        {
            if (!(TryMove(newLoc, direction) && TryMove(newLoc.GetRelativeCoordinate(d[Direction.Left]), direction)))
            {
                return false;
            }
        }

        return true;
    }

    private void Move(Coordinate from, Direction direction)
    {
        var move = d[direction];
        var newLoc = from.GetRelativeCoordinate(move);
        if (!_map.TryGetValue(newLoc, out var n))
        {
            MoveItem(from, newLoc);
            return;
        }
        if (n == Wall)
        {
            return;
        }
        if (direction == Direction.Left || direction == Direction.Right)
        {
            Move(newLoc, direction);
            MoveItem(from, newLoc);
            return;
        }
        if (_map.ContainsKey(newLoc))
        {
            if (_map[newLoc] == CrateLeft)
            {
                Move(newLoc, direction);
                Move(newLoc.GetRelativeCoordinate(d[Direction.Right]), direction);
            }
        }
        if (_map.ContainsKey(newLoc))
        {
            if (_map[newLoc] == CrateRight)
            {
                Move(newLoc, direction);
                Move(newLoc.GetRelativeCoordinate(d[Direction.Left]), direction);
            }
        }

        MoveItem(from, newLoc);
    }

    private void MoveItem(Coordinate from, Coordinate to)
    {
        var c = _map[from];
        _map[to] = c;
        _map.Remove(from);

        Console.SetCursorPosition(from.Column, from.Row);
        Console.Write(" ");
        Console.SetCursorPosition(to.Column, to.Row);
        Console.Write(c);
    }

    private readonly Dictionary<Direction, Coordinate> d = new()
    {
        { Direction.Up, new Coordinate(-1, 0) },
        { Direction.Down, new Coordinate(1, 0) },
        { Direction.Left, new Coordinate(0, -1) },
        { Direction.Right, new Coordinate(0, 1) },
    };
}

record Coordinate(int Row, int Column)
{
    public Coordinate GetRelativeCoordinate(Coordinate c) => new(Row + c.Row, Column + c.Column);
}

enum Direction
{
    Up,
    Down,
    Left,
    Right,
}
