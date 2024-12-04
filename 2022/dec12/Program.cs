var input = File.ReadAllLines("input.txt");

Console.CursorVisible = false;
Console.Clear();

var grid = new List<List<Location>>();

int x = 0;

Location Start = new Location();
Location End = new Location();

foreach(var row in input){
    grid.Add(new List<Location>());
    int y = 0;
    foreach(var loc in row){
        var location = new Location(){
            X = x,
            Y = y,
            Height = (int)loc,
        };
        if(loc == 'S'){
            location.Type = LocationType.Start;
            Start = location;
            location.Height = (int)'a';
            location.LeastDistance = 0;
        }
        if(loc == 'E'){
            location.Type = LocationType.End;
            End = location;
            location.Height = (int)'z';
        }
        if(location.Height == (int)'a'){
            // currentLocations.Add(location);
        }
        grid[x].Add(location);
        Console.Write(loc);
        y++;
    }
    Console.WriteLine();
    x++;
}

foreach (var row in grid)
{
    foreach(var Location in row){
        if(Location.Y != row.Count - 1){
            Location.AddLocation(grid[Location.X][Location.Y+1]);
        }
        if(Location.Y != 0){
            Location.AddLocation(grid[Location.X][Location.Y-1]);
        }
        if(Location.X != grid.Count - 1){
            Location.AddLocation(grid[Location.X+1][Location.Y]);
        }
        if(Location.X != 0){
            Location.AddLocation(grid[Location.X-1][Location.Y]);
        }
    }
}

// Console.WriteLine(Start.FindDistance(End, 0));

HashSet<Location> currentLocations = new HashSet<Location>(){
    Start
};

var distance = 0;
while (true)
{
    // Print(grid, null);
    distance++;
    HashSet<Location> nextLocations = new HashSet<Location>();
    foreach(var location in currentLocations){
        foreach(var toAdd in location.NextLocations(distance)){
            nextLocations.Add(toAdd);
        }
    }
    currentLocations = nextLocations;
    if(nextLocations.Count == 0){
        break;
    }
    Thread.Sleep(10);
}

var path = End.GetPath(new List<Location>());
Console.ResetColor();
Console.SetCursorPosition(0, grid.Count);
Console.WriteLine(End.LeastDistance);
// Print(grid, path);

// Console.WriteLine(End.LeastDistance);


class Location{

    public LocationType Type {get; set;} = LocationType.Ground;
    public Location? Prev = null;
    public int X { get; set; }
    public int Y { get; set; }
    public int Height { get; set; }
    public List<Location> ValidLocations { get; set; } = new List<Location>();

    public Location(){
        // Print();
    }

    public void AddLocation(Location location){
        if((location.Height)  <= Height + 1 ){
            ValidLocations.Add(location);
            return;
        }
    }

    public int LeastDistance {get;set;}= int.MaxValue;

    public List<Location> NextLocations(int currentDist){
        var locations = new List<Location>();
        Print();
        foreach(var location in ValidLocations){
            if(location.IsValid(currentDist, this)){
                locations.Add(location);
            }
        }
        return locations;
    }

    public bool IsValid(int currentDist, Location prev){
        if(LeastDistance <= currentDist ){
            return false;
        }
        LeastDistance = currentDist;
        Prev = prev;
        return true;
    }

    public List<Location> GetPath(List<Location> path){
        Print(true);
        if(Prev != null){
            path.Add(Prev);
            Prev.GetPath(path);
        }
        return path;
    }

    private void Print(bool path = false){
        
        Console.SetCursorPosition(Y, X);

        // Console.CursorLeft = Y;
        // Console.CursorTop = X;
        char toPrint = (char)Height;
        if(LeastDistance == int.MaxValue){
            Console.ResetColor();
        }else if(!path){
            Console.ForegroundColor = ConsoleColor.DarkRed;
            toPrint = (char)(Height - 32);
        }else{
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }
        Console.Write(toPrint);
    }
}

enum LocationType{
    Start,
    End,
    Ground
}