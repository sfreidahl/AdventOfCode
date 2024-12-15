// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

const char Up = '^';
const char Down = 'v';
const char Left = '<';
const char Right = '>';

var map = File.ReadAllLines("input-map.txt")
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
wareHouse.MoveRobot(instructions);

wareHouse.Print();

Console.WriteLine(wareHouse.Result);

class WareHouse(Dictionary<Coordinate, char> Map)
{
    const char Wall = '#';
    const char Robot = '@';
    const char Crate = 'O';

    private Dictionary<Coordinate, char> _map = Map;
    private Coordinate _robot => _map.Where(x => x.Value == Robot).First().Key;

    public int Result => _map.Where(x => x.Value == Crate).Select(x => x.Key).Select(x => x.Row * 100 + x.Column).Sum();

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

    public void MoveRobot(List<Direction> moves)
    {
        foreach (var move in moves)
        {
            MoveRobot(move);
        }
    }

    private void MoveRobot(Direction direction)
    {
        TryMove(_robot, direction);
    }

    private bool TryMove(Coordinate from, Direction direction)
    {
        var move = d[direction];
        var newLoc = from.GetRelativeCoordinate(move);
        if (!_map.TryGetValue(newLoc, out var n))
        {
            MoveItem(from, newLoc);
            return true;
        }
        if (n == Wall)
        {
            return false;
        }
        if (!TryMove(newLoc, direction))
        {
            return false;
        }

        MoveItem(from, newLoc);
        return true;
    }

    private void MoveItem(Coordinate from, Coordinate to)
    {
        var c = _map[from];
        _map[to] = c;
        _map.Remove(from);
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
