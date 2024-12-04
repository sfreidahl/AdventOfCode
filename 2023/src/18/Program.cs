var commands = File.ReadAllLines("input.txt").Select(x => x.Split(" ")).Select(x => new Command(char.Parse(x[0]), int.Parse(x[1])));

List<Location> wallLocations = new List<Location>();

foreach (var command in commands)
{
    var lastLocation = wallLocations.LastOrDefault();
    for (int i = 1; i <= command.Length; i++)
    {
        if (command.Direction == 'U')
        {
            wallLocations.Add(new Location(lastLocation.Row + i, lastLocation.Column));
            continue;
        }
        if (command.Direction == 'D')
        {
            wallLocations.Add(new Location(lastLocation.Row - i, lastLocation.Column));
            continue;
        }
        if (command.Direction == 'L')
        {
            wallLocations.Add(new Location(lastLocation.Row, lastLocation.Column - i));
            continue;
        }
        if (command.Direction == 'R')
        {
            wallLocations.Add(new Location(lastLocation.Row, lastLocation.Column + i));
            continue;
        }
    }
}

Console.WriteLine(wallLocations.Last());

var minRow = wallLocations.Min(x => x.Row);
var minCol = wallLocations.Min(x => x.Column);
var maxRow = wallLocations.Max(x => x.Row);
var maxCol = wallLocations.Max(x => x.Column);

Console.WriteLine(minRow);
Console.WriteLine(minCol);
Console.WriteLine(maxRow);
Console.WriteLine(maxCol);

var zeroedWallLocation = wallLocations.Select(x => new Location((x.Row + -minRow) + 1, (x.Column + -minCol) + 1));

var minRowZeroed = zeroedWallLocation.Min(x => x.Row);
var minColZeroed = zeroedWallLocation.Min(x => x.Column);
var maxRowZeroed = zeroedWallLocation.Max(x => x.Row);
var maxColZeroed = zeroedWallLocation.Max(x => x.Column);

Console.WriteLine(minRowZeroed);
Console.WriteLine(minColZeroed);
Console.WriteLine(maxRowZeroed);
Console.WriteLine(maxColZeroed);

var currentLocations = new HashSet<Location>() { new Location(0, 0) };

var visitedLocations = new HashSet<Location>() { };

while (true)
{
    var newLocationsToVisit = new List<Location>();
    foreach (var location in currentLocations)
    {
        visitedLocations.Add(location);
        var locationsToVisit = new List<Location>()
        {
            new Location(location.Row - 1, location.Column),
            new Location(location.Row + 1, location.Column),
            new Location(location.Row, location.Column - 1),
            new Location(location.Row, location.Column + 1)
        }
            .Where(
                x =>
                    x.Row >= 0
                    && x.Row <= (maxRowZeroed + 1)
                    && x.Column >= 0
                    && x.Column <= (maxColZeroed + 1)
                    && !zeroedWallLocation.Contains(x)
                    && !visitedLocations.Contains(x)
            )
            .ToList();
        locationsToVisit.ForEach(x => visitedLocations.Add(x));
        newLocationsToVisit.AddRange(locationsToVisit);
    }
    Console.WriteLine(newLocationsToVisit.Count);
    currentLocations = newLocationsToVisit.ToHashSet();
    if (currentLocations.Count == 0)
    {
        break;
    }
}

for (int i = 0; i <= maxRowZeroed + 1; i++)
{
    Console.WriteLine();
    for (int j = 0; j <= maxColZeroed + 1; j++)
    {
        if (zeroedWallLocation.Contains(new Location(i, j)))
        {
            Console.Write("#");
            continue;
        }
        if (visitedLocations.Contains(new Location(i, j)))
        {
            Console.Write("+");
            continue;
        }
        Console.Write(".");
    }
}
Console.WriteLine();
Console.WriteLine();

var boardSize = (maxRowZeroed + 2) * (maxColZeroed + 2);

Console.WriteLine(boardSize);
Console.WriteLine(visitedLocations.Count);

var hole = boardSize - visitedLocations.Count;

Console.WriteLine(hole);

record struct Command(char Direction, int Length);

record struct Location(int Row, int Column);

enum WallType
{
    Horizontal,
    Vertical,
    CornerUp,
    CornerDown
}
