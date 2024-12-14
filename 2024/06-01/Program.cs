// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

Maze maze = new Maze(File.ReadAllLines("input.txt"));

Console.WriteLine(maze.MoveThroughMaze().Count);

// var startPosition = GetStartPosition(map);

class Maze
{
    private const char _startIcon = '^';
    private string[] _map;

    public Maze(string[] map)
    {
        _map = map;
    }

    public HashSet<Position> MoveThroughMaze()
    {
        var currentPosition = (MoveResult: MoveResult.Start, Position: GetStartPosition(), Direction: Direction.Up);
        HashSet<Position> visited = [currentPosition.Position];
        do
        {
            currentPosition = Move(currentPosition.Position, currentPosition.Direction);
            if (currentPosition.MoveResult == MoveResult.Exit)
            {
                return visited;
            }
            visited.Add(currentPosition.Position);
        } while (true);
    }

    private Position GetStartPosition()
    {
        for (int rowIndex = 0; rowIndex < _map.Count(); rowIndex++)
        {
            var row = _map[rowIndex];
            var startIndex = row.IndexOf(_startIcon);
            if (startIndex == -1)
            {
                continue;
            }

            return new(rowIndex, startIndex);
        }
        throw new UnreachableException("");
    }

    private (MoveResult MoveResult, Position Position, Direction Direction) Move(Position position, Direction direction)
    {
        var nextPos = position.GetNextPosition(direction);
        if (IsExit(nextPos))
        {
            return (MoveResult.Exit, nextPos, direction);
        }
        if (IsWall(nextPos))
        {
            var newDirection = Turn(direction);
            return Move(position, newDirection);
        }
        return (MoveResult.Moved, nextPos, direction);
    }

    private bool IsWall(Position position)
    {
        return _map[position.Row][position.Column] == '#';
    }

    private bool IsExit(Position position)
    {
        if (position.Row == -1)
        {
            return true;
        }
        if (position.Column == -1)
        {
            return true;
        }
        if (position.Row == _map.Count())
        {
            return true;
        }
        if (position.Column == _map[0].Count())
        {
            return true;
        }
        return false;
    }

    private Direction Turn(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new UnreachableException("invalid direction"),
        };
    }
}

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}

enum MoveResult
{
    Start,
    Moved,
    Wall,
    Exit,
}

record struct Position(int Row, int Column)
{
    public Position GetNextPosition(Direction direction)
    {
        return direction switch
        {
            Direction.Up => this with { Row = Row - 1 },
            Direction.Right => this with { Column = Column + 1 },
            Direction.Down => this with { Row = Row + 1 },
            Direction.Left => this with { Column = Column - 1 },
            _ => throw new UnreachableException("invalid direction"),
        };
    }
}
