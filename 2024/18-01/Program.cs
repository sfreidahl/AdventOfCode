// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var locations = File.ReadLines("input.txt").Select(x => x.Split(",").Select(int.Parse).ToList()).Select(x => new Coordinate(x[0], x[1])).ToList();

var maze = new Maze(locations.Take(2941).ToHashSet());

Console.WriteLine(maze.GetQuickestPath());

class Maze(HashSet<Coordinate> Maze)
{
    private HashSet<Coordinate> _maze = Maze;
    private Coordinate _start = new(0, 0);
    private Coordinate _end = new(70, 70);
    private Dictionary<Coordinate, int> _visited = [];

    private void Print()
    {
        Console.Clear();
        foreach (var wall in _maze)
        {
            Console.SetCursorPosition(wall.Column, wall.Row);
            Console.Write("#");
        }
    }

    private void Print(List<Coordinate> coords)
    {
        foreach (var visited in coords)
        {
            Console.SetCursorPosition(visited.Column, visited.Row);
            Console.Write("O");
            Console.SetCursorPosition(0, 71);
        }
    }

    public int GetQuickestPath()
    {
        Print();
        Console.ReadLine();
        Dictionary<Coordinate, int> costs = [];
        List<(Coordinate Position, int Cost)> currentPositions = [(_start, 0)];
        _visited.Add(_start, 0);
        var cheapest = int.MaxValue;
        while (currentPositions.Count > 0)
        {
            Print(currentPositions.Select(x => x.Position).ToList());
            currentPositions = currentPositions
                .SelectMany(x => GetPositionsAndPrice(x.Position, x.Cost))
                .GroupBy(x => x.Position)
                .Select(x => x.MinBy(x => x.Cost))
                .ToList();

            var endPoint = currentPositions.Where(x => x.Position == _end);
            if (endPoint.Count() > 0)
            {
                cheapest = int.Min(cheapest, endPoint.First().Cost);
                currentPositions.Remove(endPoint.First());
            }
        }
        return cheapest;
    }

    private List<(Coordinate Position, int Cost)> GetPositionsAndPrice(Coordinate CurrentPosition, int CurrentPrice)
    {
        if (_visited.TryGetValue(CurrentPosition, out var v))
        {
            if (v < CurrentPrice)
            {
                return [];
            }
        }
        _visited[CurrentPosition] = CurrentPrice;
        List<(Coordinate Position, int Cost)> newPositions = [];
        foreach (var newPos in CurrentPosition.GetAdjacentCoordinates())
        {
            if (_maze.Contains(newPos))
            {
                continue;
            }
            newPositions.Add((newPos, CurrentPrice + 1));
        }

        return newPositions;
    }
}

record Coordinate(int Row, int Column)
{
    public List<Coordinate> GetAdjacentCoordinates()
    {
        List<Coordinate> coordinates = [new(Row - 1, Column), new(Row + 1, Column), new(Row, Column - 1), new(Row, Column + 1)];
        return coordinates.Where(x => x.Column >= 0 && x.Column <= 70 && x.Row >= 0 && x.Row <= 70).ToList();
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
