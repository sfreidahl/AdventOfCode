// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Console.Clear();

List<Node> nodes = File.ReadAllLines("input.txt")
    .SelectMany((line, rowIndex) => line.Select((x, columnIndex) => new Node(x, new Location(rowIndex, columnIndex))))
    .ToList();
var nodeDic = nodes.ToDictionary(x => x.Location, x => x);
foreach (var node in nodes)
{
    node.Nodes.Add(Direction.North, nodeDic.GetValueOrDefault(new Location(node.Location.Row - 1, node.Location.Column)));
    node.Nodes.Add(Direction.South, nodeDic.GetValueOrDefault(new Location(node.Location.Row + 1, node.Location.Column)));
    node.Nodes.Add(Direction.East, nodeDic.GetValueOrDefault(new Location(node.Location.Row, node.Location.Column + 1)));
    node.Nodes.Add(Direction.West, nodeDic.GetValueOrDefault(new Location(node.Location.Row, node.Location.Column - 1)));
}

// Console.WriteLine(nodeDic.Count);

// Console.Clear();
// nodes.ForEach(x => x.Draw());
// Thread.Sleep(2000);

var maxRow = nodes.Max(x => x.Location.Row);
var maxColumn = nodes.Max(x => x.Location.Column);

var start = new Location(0, 0);
var end = new Location(maxRow, maxColumn);
nodeDic[start].HeatLoss = 0;
nodeDic[end].IsEnd = true;

// nodeDic[new Location(0,0)].Visit(Direction.West, 0, 0);

// Console.WriteLine(nodeDic[start].Visit(Direction.West, 0,0));

HashSet<BigState> currentLocations = new() { { new(start, Direction.Any, 0, 0) } };

var distance = 0;
while (true)
{
    HashSet<BigState> nextLocations = new();
    foreach (var location in currentLocations)
    {
        foreach (var toAdd in nodeDic[location.Location].Visit(location.From, location.CurrentHeatLoss, location.CurrentStraightCount))
        {
            nextLocations.Add(toAdd);
        }
    }
    currentLocations = nextLocations;
    if (nextLocations.Count == 0)
    {
        break;
    }
    Console.WriteLine($"{currentLocations.Count}");
    distance++;
    // Thread.Sleep(10);
}

// foreach (var location in Node.shortestPath)
// {
//     Console.CursorLeft = location.Column;
//     Console.CursorTop = location.Row;
//     Console.Write("#");
// }

Console.WriteLine(Node.LowestEnd);

class Node
{
    private char _char;
    public int HeatLoss { get; set; }
    public Location Location { get; }
    public Dictionary<Direction, Node?> Nodes = new Dictionary<Direction, Node?>();
    public Dictionary<State, int> Visited = new Dictionary<State, int>();
    public bool IsEnd { get; set; }
    public static int LowestEnd = int.MaxValue;
    public static List<Location> shortestPath = new List<Location>();

    public Node(char type, Location location)
    {
        Location = location;
        HeatLoss = int.Parse($"{type}");
        _char = type;
    }

    public List<BigState> Visit(Direction from, int currentHeatLoss, int currentStraightCount)
    {
        currentHeatLoss += HeatLoss;
        var state = new State(from, currentStraightCount);
        if (Visited.ContainsKey(state) && currentHeatLoss >= Visited[state] || currentHeatLoss >= LowestEnd)
        {
            return new();
        }
        Visited[state] = currentHeatLoss;
        if (IsEnd)
        {
            if (currentStraightCount >= 4)
            {
                LowestEnd = int.Min(currentHeatLoss, LowestEnd);
            }
            return new();
        }
        var straightAcross = GetOpposite(from);
        var nextNodes = new List<BigState>();
        if (currentStraightCount >= 4 || from == Direction.Any)
        {
            foreach (var node in Nodes.Where(x => x.Key != straightAcross && x.Key != from))
            {
                if (node.Value == null)
                {
                    continue;
                }
                nextNodes.Add(new BigState(node.Value.Location, GetOpposite(node.Key), 1, currentHeatLoss));
            }
        }
        if (straightAcross != Direction.Any && Nodes[straightAcross] != null && currentStraightCount != 10)
        {
            nextNodes.Add(new BigState(Nodes[straightAcross]!.Location, from, currentStraightCount + 1, currentHeatLoss));
        }

        return nextNodes;
    }

    public void Draw()
    {
        Console.CursorLeft = Location.Column;
        Console.CursorTop = Location.Row;
        Console.Write(_char);
    }

    public void DrawHere()
    {
        // Console.CursorLeft = Location.Column;
        // Console.CursorTop = Location.Row;
        // Console.Write("#");
    }

    private static Direction GetOpposite(Direction direction)
    {
        return direction switch
        {
            Direction.South => Direction.North,
            Direction.North => Direction.South,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            Direction.Any => Direction.Any,
            _ => throw new NotImplementedException(),
        };
    }
}

enum Direction
{
    North,
    South,
    East,
    West,
    Any
}

record struct Location(int Row, int Column);

record struct State(Direction From, int StraightCount);

record struct BigState(Location Location, Direction From, int CurrentStraightCount, int CurrentHeatLoss);
