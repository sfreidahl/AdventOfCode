// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var locations = File.ReadAllLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((c, columnIndex) => (c, new Coordinate(rowIndex, columnIndex))));
var start = locations.Where(x => x.c == 'S').First().Item2;
var end = locations.Where(x => x.c == 'E').First().Item2;

var walls = locations.Where(x => x.c == '#').Select(x => x.Item2).ToHashSet();

var height = walls.Max(x => x.Row);
var width = walls.Max(x => x.Column);

var maze = new Maze(walls, start, end);

int baseTime = maze.GetQuickestPath();

// Console.SetCursorPosition(0, height+1);

Console.WriteLine($"basetime: {baseTime}");

var wallsToTestRemove = walls
    .Where(x => x.Row > 0 && x.Row < height && x.Column > 0 && x.Column < width)
    .Where(x =>
        !walls.Contains(new(x.Row - 1, x.Column)) && !walls.Contains(new(x.Row + 1, x.Column))
        || !walls.Contains(new(x.Row, x.Column - 1)) && !walls.Contains(new(x.Row, x.Column + 1))
    );

var result = 0;
foreach (var wall in wallsToTestRemove)
{
    HashSet<Coordinate> newWalls = [.. walls];
    newWalls.Remove(wall);
    var newMaze = new Maze(newWalls, start, end);
    var time = newMaze.GetQuickestPath();

    if (baseTime - time >= 100)
    {
        result++;
    }
}

Console.WriteLine(result);

class Maze(HashSet<Coordinate> maze, Coordinate start, Coordinate end)
{
    private HashSet<Coordinate> _maze = maze;
    private Coordinate _start = start;
    private Coordinate _end = end;
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
        }
    }

    public int GetQuickestPath()
    {
        // Print();
        // Console.ReadLine();
        Dictionary<Coordinate, int> costs = [];
        List<(Coordinate Position, int Cost)> currentPositions = [(_start, 0)];
        _visited.Add(_start, 0);
        var cheapest = int.MaxValue;
        while (currentPositions.Count > 0)
        {
            // Print(currentPositions.Select(x => x.Position).ToList());
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
        return coordinates;
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
