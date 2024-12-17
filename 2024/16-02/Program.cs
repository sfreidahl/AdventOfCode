// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var locations = File.ReadLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((c, columnIndex) => (c, new Coordinate(rowIndex, columnIndex))))
    .Where(x => x.c != '#');

var maze = new Maze(locations.ToDictionary(x => x.Item2, x => x.c));

Console.WriteLine(maze.GetLocationsALongPath());

;

class Maze(Dictionary<Coordinate, char> Maze)
{
    private Dictionary<Coordinate, char> _maze = Maze;
    private Position _start = new(Maze.Where(x => x.Value == 'S').First().Key, Direction.Right);
    private Coordinate _end = Maze.Where(x => x.Value == 'E').First().Key;
    private Dictionary<Position, int> _visited = [];

    public int GetLocationsALongPath()
    {
        Dictionary<Position, int> costs = [];
        List<(Position Position, int Cost, List<Coordinate> CurrentPath)> currentPositions = [(_start, 0, [])];
        _visited.Add(_start, 0);
        List<(List<Coordinate> Path, int Cost)> paths = [];
        while (currentPositions.Count > 0)
        {
            Console.WriteLine(currentPositions.Count);
            currentPositions = currentPositions.SelectMany(x => GetPositionsAndPrice(x.Position, x.Cost, x.CurrentPath)).ToList();

            foreach (var endPoint in currentPositions.Where(x => x.Position.Coordinate == _end).ToList())
            {
                paths.Add((endPoint.CurrentPath, endPoint.Cost));
                currentPositions.Remove(endPoint);
            }
        }
        return paths.Where(x => x.Cost == paths.MinBy(x => x.Cost).Cost).SelectMany(x => x.Path).ToHashSet().Count + 1;
    }

    private List<(Position Position, int Cost, List<Coordinate> CurrentPath)> GetPositionsAndPrice(
        Position CurrentPosition,
        int CurrentPrice,
        List<Coordinate> CurrentPath
    )
    {
        if (_visited.TryGetValue(CurrentPosition, out var v))
        {
            if (v < CurrentPrice)
            {
                return [];
            }
        }
        _visited[CurrentPosition] = CurrentPrice;
        List<(Position Position, int Cost, List<Coordinate> CurrentPath)> newPositions = [];
        foreach (var dir in GetNewDirections(CurrentPosition.Direction))
        {
            var newPos = CurrentPosition.Coordinate.GetAdjacentCoordinate(dir.Direction);
            if (!_maze.ContainsKey(newPos))
            {
                continue;
            }
            newPositions.Add((new(newPos, dir.Direction), CurrentPrice + dir.Cost, [.. CurrentPath, newPos]));
        }

        return newPositions;
    }

    private List<(Direction Direction, int Cost)> GetNewDirections(Direction direction)
    {
        var right = direction == Direction.Left ? Direction.Up : direction + 1;
        var left = direction == Direction.Up ? Direction.Left : direction - 1;
        return [(left, 1001), (direction, 1), (right, 1001)];
    }
}

record Coordinate(int Row, int Column)
{
    public Coordinate GetAdjacentCoordinate(Direction direction)
    {
        return direction switch
        {
            Direction.Up => new(Row - 1, Column),
            Direction.Down => new(Row + 1, Column),
            Direction.Left => new(Row, Column - 1),
            Direction.Right => new(Row, Column + 1),
            _ => throw new UnreachableException(""),
        };
    }
}

record Position(Coordinate Coordinate, Direction Direction);

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}
