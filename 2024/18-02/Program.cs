// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var locations = File.ReadLines("input.txt").Select(x => x.Split(",").Select(int.Parse).ToList()).Select(x => new Coordinate(x[0], x[1])).ToList();

for (int i = 1025; i < locations.Count; i++)
{
    var maze = new Maze(locations.Take(i).ToHashSet());
    var quickest = maze.GetQuickestPath();
    if (quickest == int.MaxValue)
    {
        Console.WriteLine(i);
        Console.WriteLine(locations[i - 1]);
        break;
    }
}

class Maze(HashSet<Coordinate> Maze)
{
    private HashSet<Coordinate> _maze = Maze;
    private Coordinate _start = new(0, 0);
    private Coordinate _end = new(70, 70);
    private Dictionary<Coordinate, int> _visited = [];

    public int GetQuickestPath()
    {
        Dictionary<Coordinate, int> costs = [];
        List<(Coordinate Position, int Cost)> currentPositions = [(_start, 0)];
        _visited.Add(_start, 0);
        var cheapest = int.MaxValue;
        while (currentPositions.Count > 0)
        {
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

record Coordinate(int X, int Y)
{
    public List<Coordinate> GetAdjacentCoordinates()
    {
        List<Coordinate> coordinates = [new(X - 1, Y), new(X + 1, Y), new(X, Y - 1), new(X, Y + 1)];
        return coordinates.Where(x => x.Y >= 0 && x.Y <= 70 && x.X >= 0 && x.X <= 70).ToList();
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
