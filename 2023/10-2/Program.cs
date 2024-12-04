Console.WriteLine("Hello, World!");

var grid = File.ReadAllLines("input.txt").Aggregate("", (acc, src) => acc += src);
var grid2d = File.ReadAllLines("input.txt");

var start = grid.IndexOf("S");

var gridAsPipes = grid.Select((x, index) => new Pipe(index, new PipeType(x), false)).ToArray();
for (int i = 0; i < gridAsPipes.Length; i++)
{
    gridAsPipes[i].Grid = gridAsPipes;
}
var startPipeConnections = new List<Directions>();
if (start > 140 && gridAsPipes[start + (int)Directions.North].Type.Connections.Contains(Directions.South))
{
    startPipeConnections.Add(Directions.North);
}
if (gridAsPipes[start + (int)Directions.South].Type.Connections.Contains(Directions.North))
{
    startPipeConnections.Add(Directions.South);
}
if (gridAsPipes[start + (int)Directions.East].Type.Connections.Contains(Directions.West))
{
    startPipeConnections.Add(Directions.East);
}
if (gridAsPipes[start + (int)Directions.West].Type.Connections.Contains(Directions.East))
{
    startPipeConnections.Add(Directions.West);
}

var startPipe = new Pipe(start, new PipeType(startPipeConnections), true) { Grid = gridAsPipes };
gridAsPipes[start] = startPipe;
var loopLength = 1;
var currentPipe = gridAsPipes[startPipe.Location + (int)startPipe.Type.Connections[0]];
var locations = new HashSet<Location>() { new Location(start), new Location(currentPipe.Location) };
var fromDirection = startPipe.Type.Connections[0].GetOppositeDirection();
while (true)
{
    var nextDirection = currentPipe.Type.Connections.Where(x => x != fromDirection).First();
    currentPipe = gridAsPipes[currentPipe.Location + (int)nextDirection];
    locations.Add(new Location(currentPipe.Location));
    if (currentPipe.isStart)
    {
        break;
    }
    fromDirection = nextDirection.GetOppositeDirection();
    loopLength++;
}

var upDownDic = new Dictionary<char, Directions>
{
    { 'J', Directions.North },
    { 'L', Directions.North },
    { '7', Directions.South },
    { 'F', Directions.South },
};

var count = 0;
var inside = false;
var inWall = false;
var wallDir = Directions.North;
var rowIndex = 0;
var ConsoleColor = Console.ForegroundColor;
foreach (var row in grid2d)
{
    var colIndex = 0;
    inWall = false;
    inside = false;
    foreach (var l in row)
    {
        var loc = new Location(rowIndex, colIndex);
        var pipe = grid[rowIndex * 140 + colIndex];
        if (locations.Contains(loc))
        {
            if (upDownDic.Keys.Contains(pipe))
            {
                if (!inWall)
                {
                    wallDir = upDownDic[pipe];
                    inWall = true;
                }
                else
                {
                    inWall = false;
                    if (wallDir != upDownDic[pipe])
                    {
                        inside = !inside;
                    }
                }
            }
            else if (pipe == '|' || pipe == 'S')
            {
                inside = !inside;
            }
            Console.ForegroundColor = ConsoleColor;
            Console.Write(pipe);
        }
        else
        {
            if (inside)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("I");
                count++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("O");
            }
        }
        colIndex++;
    }
    Console.WriteLine();
    rowIndex++;
}

Console.WriteLine(count);

enum Directions
{
    North = -140,
    South = 140,
    East = 1,
    West = -1
}

static class DirectionsExtensions
{
    public static Directions GetOppositeDirection(this Directions direction)
    {
        return (Directions)(-(int)direction);
    }
}

record struct Pipe(int Location, PipeType Type, bool isStart)
{
    public Pipe[] Grid { get; set; }
}

record struct Location(int Row, int Column)
{
    public Location(int i) : this(i / 140, i % 140) { }
}

struct PipeType
{
    public List<Directions> Connections = new List<Directions>();

    public PipeType(List<Directions> connections)
    {
        Connections = connections;
    }

    public PipeType(char c)
    {
        if (c == '|')
        {
            Connections.Add(Directions.North);
            Connections.Add(Directions.South);
            return;
        }
        if (c == '-')
        {
            Connections.Add(Directions.West);
            Connections.Add(Directions.East);
            return;
        }
        if (c == 'L')
        {
            Connections.Add(Directions.North);
            Connections.Add(Directions.East);
            return;
        }
        if (c == 'J')
        {
            Connections.Add(Directions.North);
            Connections.Add(Directions.West);
            return;
        }
        if (c == '7')
        {
            Connections.Add(Directions.South);
            Connections.Add(Directions.West);
            return;
        }
        if (c == 'F')
        {
            Connections.Add(Directions.South);
            Connections.Add(Directions.East);
            return;
        }
    }
}
