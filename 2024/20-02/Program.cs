// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var locations = File.ReadAllLines("input.txt")
    .SelectMany((row, rowIndex) => row.Select((c, columnIndex) => (c, new Coordinate(rowIndex, columnIndex))));
var start = locations.Where(x => x.c == 'S').First().Item2;
var end = locations.Where(x => x.c == 'E').First().Item2;

var walls = locations.Where(x => x.c == '#').Select(x => x.Item2).ToHashSet();

var maze = new Maze(walls, start, end);

var basePath = maze.GetPath();

var result = 0;

foreach (var coord in basePath)
{
    foreach (var cheat in Maze.GetPossibleCheats(coord.Key))
    {
        if (basePath.TryGetValue(cheat.Key, out var baseCost))
        {
            if (baseCost - (coord.Value + cheat.Value) >= 100)
            {
                result++;
            }
        }
    }
}

Console.WriteLine(result);

class Maze(HashSet<Coordinate> maze, Coordinate start, Coordinate end)
{
    private HashSet<Coordinate> _maze = maze;
    private Coordinate _start = start;
    private Coordinate _end = end;
    private Dictionary<Coordinate, int> _visited = [];

    public static Dictionary<Coordinate, int> GetPossibleCheats(Coordinate coord)
    {
        Dictionary<Coordinate, int> newCoords = [];

        for (int wallHackLength = 2; wallHackLength <= 20; wallHackLength++)
        {
            for (int i = 0; i <= wallHackLength; i++)
            {
                newCoords[new Coordinate(coord.Row + i, coord.Column + (wallHackLength - i))] = wallHackLength;
                newCoords[new Coordinate(coord.Row - i, coord.Column + (wallHackLength - i))] = wallHackLength;
                newCoords[new Coordinate(coord.Row - i, coord.Column - (wallHackLength - i))] = wallHackLength;
                newCoords[new Coordinate(coord.Row + i, coord.Column - (wallHackLength - i))] = wallHackLength;
            }
        }

        return newCoords;
    }

    public Dictionary<Coordinate, int> GetPath()
    {
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

        _visited.Add(_end, cheapest);
        return _visited;
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
