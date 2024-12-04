Console.WriteLine("Hello, World!");

var grid = File.ReadAllLines("input.txt").Aggregate("", (acc, src) => acc += src);
;

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
var fromDirection = startPipe.Type.Connections[0].GetOppositeDirection();
while (true)
{
    if (currentPipe.isStart)
    {
        break;
    }
    var nextDirection = currentPipe.Type.Connections.Where(x => x != fromDirection).First();
    currentPipe = gridAsPipes[currentPipe.Location + (int)nextDirection];
    fromDirection = nextDirection.GetOppositeDirection();
    loopLength++;
}

Console.WriteLine(loopLength / 2);

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

    public int GetLoopFromStart()
    {
        return Grid[Location + (int)Type.Connections[0]].GetLoopLength(Type.Connections[0].GetOppositeDirection(), 1);
    }

    public int GetLoopLength(Directions fromDirection, int steps)
    {
        Console.WriteLine(steps);
        if (this.isStart)
        {
            return steps;
        }
        var nextDirection = Type.Connections.Where(x => x != fromDirection).First();
        var nextPipe = Grid[Location + (int)nextDirection];
        return nextPipe.GetLoopLength(nextDirection.GetOppositeDirection(), steps + 1);
    }
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
