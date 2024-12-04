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
// nodeDic[new Location(0,0)].Visit(Direction.West);
var result = 0;

var maxRow = nodes.Max(x => x.Location.Row);
var maxColumn = nodes.Max(x => x.Location.Column);

for (int row = 0; row <= maxRow; row++)
{
    nodes.ForEach(x => x.VisitedFrom.Clear());
    nodeDic[new Location(row, 0)].Visit(Direction.West);
    var westResult = nodes.Where(x => x.VisitedFrom.Count > 0).Count();

    nodes.ForEach(x => x.VisitedFrom.Clear());
    nodeDic[new Location(row, maxColumn)].Visit(Direction.East);
    var eastResult = nodes.Where(x => x.VisitedFrom.Count > 0).Count();

    result = Math.Max(Math.Max(eastResult, westResult), result);
}

for (int column = 0; column <= maxColumn; column++)
{
    nodes.ForEach(x => x.VisitedFrom.Clear());
    nodeDic[new Location(0, column)].Visit(Direction.North);
    var northResult = nodes.Where(x => x.VisitedFrom.Count > 0).Count();

    nodes.ForEach(x => x.VisitedFrom.Clear());
    nodeDic[new Location(maxRow, column)].Visit(Direction.South);
    var southResult = nodes.Where(x => x.VisitedFrom.Count > 0).Count();

    result = Math.Max(Math.Max(northResult, southResult), result);
}

Console.WriteLine(result);

class Node
{
    private char _char;
    public NodeType Type { get; set; }
    public Location Location { get; }
    public Dictionary<Direction, Node?> Nodes = new Dictionary<Direction, Node?>();
    public HashSet<Direction> VisitedFrom = new HashSet<Direction>();

    public Node(char type, Location location)
    {
        Type = type switch
        {
            '.' => NodeType.Empty,
            '-' => NodeType.SplitHorizontal,
            '|' => NodeType.SplitVertical,
            '/' => NodeType.MirrorSouthEast,
            '\\' => NodeType.MirrorSouthWest,
            _ => throw new NotImplementedException()
        };
        Location = location;

        _char = type;
    }

    public bool Visit(Direction from)
    {
        if (VisitedFrom.Contains(from))
        {
            return true;
        }
        DrawHere();
        // Thread.Sleep(500);
        Draw();
        VisitedFrom.Add(from);
        return Type switch
        {
            NodeType.Empty => PassThrough(from),
            NodeType.SplitHorizontal => SplitHorizontal(from),
            NodeType.SplitVertical => SplitVertical(from),
            NodeType.MirrorSouthWest => MirrorSouthWest(from),
            NodeType.MirrorSouthEast => MirrorSouthEast(from),
        };
    }

    private bool PassThrough(Direction from)
    {
        var to = GetOpposite(from);
        Nodes[to]?.Visit(from);
        return true;
    }

    private bool MirrorSouthEast(Direction from)
    {
        var to = from switch
        {
            Direction.South => Direction.East,
            Direction.North => Direction.West,
            Direction.East => Direction.South,
            Direction.West => Direction.North,
            _ => throw new NotImplementedException(),
        };
        Nodes[to]?.Visit(GetOpposite(to));
        return true;
    }

    private bool MirrorSouthWest(Direction from)
    {
        var to = from switch
        {
            Direction.South => Direction.West,
            Direction.North => Direction.East,
            Direction.East => Direction.North,
            Direction.West => Direction.South,
            _ => throw new NotImplementedException(),
        };
        Nodes[to]?.Visit(GetOpposite(to));
        return true;
    }

    private bool SplitHorizontal(Direction from)
    {
        if (from == Direction.East || from == Direction.West)
        {
            PassThrough(from);
            return true;
        }
        Nodes[Direction.East]?.Visit(Direction.West);
        Nodes[Direction.West]?.Visit(Direction.East);
        return true;
    }

    private bool SplitVertical(Direction from)
    {
        if (from == Direction.North || from == Direction.South)
        {
            PassThrough(from);
            return true;
        }
        Nodes[Direction.North]?.Visit(Direction.South);
        Nodes[Direction.South]?.Visit(Direction.North);
        return true;
    }

    private static Direction GetOpposite(Direction direction)
    {
        return direction switch
        {
            Direction.South => Direction.North,
            Direction.North => Direction.South,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            _ => throw new NotImplementedException(),
        };
    }

    public void Draw()
    {
        // Console.CursorLeft = Location.Column;
        // Console.CursorTop = Location.Row;
        // Console.Write(_char);
    }

    public void DrawHere()
    {
        // Console.CursorLeft = Location.Column;
        // Console.CursorTop = Location.Row;
        // Console.Write("#");
    }
}

enum NodeType
{
    Empty,
    SplitHorizontal,
    SplitVertical,
    MirrorSouthWest,
    MirrorSouthEast
}

enum Direction
{
    North,
    South,
    East,
    West
}

record struct Location(int Row, int Column);
