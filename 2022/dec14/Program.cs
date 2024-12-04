var input = File.ReadAllLines("input.txt");

Console.Clear();
Console.CursorVisible = false;
// Console.

// char[,] grid = new char[100,150]; 

Grid grid = new Grid(1000,200, new Location(){ X = 500, Y = 0});


foreach(var wallStr in input){
    var wallPointsStr = wallStr.Split(" -> ");
    List<Location> wallPoints = new List<Location>();
    foreach(var wallPointStr in wallPointsStr){
        wallPoints.Add( new Location(wallPointStr));
    }
    // Console.WriteLine(wallPoints.Count);
    grid.AddWall(wallPoints);
}

grid.InitFloor();

int i = 0;
while(true){
    
    if(grid.Sand()){
        break;
    }
    // Thread.Sleep(50);
i++;
}
Console.WriteLine(i);


struct Point{
    public char Type { get; set; }

}

struct Location{
    public int X { get; set; }
    public int Y { get; set; }
    public Location(string input){
        var s = input.Split(",");
        X = int.Parse(s[0]);
        Y = int.Parse(s[1]);
    }

    public override bool Equals(object? obj)
    {
        return obj is Location location &&
               X == location.X &&
               Y == location.Y;
    }

    public override int GetHashCode()
    {
        
        return HashCode.Combine(X, Y);
    }
}

class Grid{

    Point[,] points;
    public Location Hole { get; set; }
    public int Floor { get; set; }
    public Grid(int width, int height, Location hole){
        Hole = hole;
        Console.BufferHeight = Math.Max(height, Console.BufferHeight);
        Console.BufferWidth = Math.Max(width, Console.BufferWidth);
        points = new Point[width,height];
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                points[x, y] = new Point(){
                    Type = '.'
                };
                // Print(new Location(){X = x, Y = y});
            }
        }
    }

    public void InitFloor(){
        for(var x = 0; x < points.GetLength(0); x++){
            points[x, Floor] = new Point(){
                Type = '#'
            };
            Print(new Location(){
                X = x,
                Y = Floor
            });
        }
    }

    public void AddWall(List<Location> wall){
        var start = wall[0];
        Floor = Math.Max(Floor, start.Y + 2);
        for(var i = 1; i < wall.Count; i++){
            
            var end = wall[i];
            Floor = Math.Max(Floor, end.Y + 2);
            // Console.WriteLine(end.X + "," + end.Y);
            AddSingleWall(start, end);
            start = end;
        }
    }

    public void AddSingleWall(Location start, Location end){
        
        if(start.X != end.X){
            var direction = start.X - end.X > 0 ? -1 : +1;
            for(int x = start.X; x != end.X + direction; x += direction){
                points[x, start.Y].Type = '#';
                Print(new Location{X = x, Y = start.Y});
            }
            return;
        }
        // Console.WriteLine("Y");
        var ydirection = start.Y - end.Y > 0 ? -1 : +1;
        for(int y = start.Y; y != end.Y + ydirection; y += ydirection){
            points[start.X, y].Type = '#';
            Print(new Location{X = start.X, Y = y});
        }

    }
    
    public bool Sand(){
        Location currentLoc = Hole;
        if(!IsFree(currentLoc)){
            return true;
        }
        Point sand = new Point(){Type = 'o'};
        Point air = new Point(){Type = '.'};
        while(true){
            points[currentLoc.X, currentLoc.Y] = sand;
            // Print(currentLoc);
            Location nextLocation = NextPosition(currentLoc);
            if(nextLocation.Equals(currentLoc)){
                Print(currentLoc);
                break;
            }
            if(currentLoc.Y == points.GetLength(1) - 5){
                return true;
            }
            points[currentLoc.X, currentLoc.Y] = air;
            // Print(currentLoc);
            currentLoc = nextLocation;

        };

        return false;


    }

    public Location NextPosition(Location loc){
        Location down = new Location(){X = loc.X, Y = loc.Y + 1};
        if(IsFree(down)){
            return down;
        }

        Location diagLeft = new Location(){X = down.X - 1, Y = down.Y};
        if(IsFree(diagLeft)){
            return diagLeft;
        }

        Location diagRight = new Location(){X = down.X + 1, Y = down.Y};
        
        if(IsFree(diagRight)){
            return diagRight;
        }
        return loc;

    }

    public bool IsFree(Location loc){
        return points[loc.X, loc.Y].Type == '.';
    }

    void Print(Location loc){
        // if(loc.X < 300 || loc.X >700){
        //     return;
        // }
        Console.ForegroundColor = GetColor(points[loc.X,loc.Y].Type);
        Console.SetCursorPosition(loc.X, loc.Y);
        Console.Write(points[loc.X,loc.Y].Type);
    }

    static ConsoleColor GetColor(char c){
        switch (c)
        {
            case '.':
                return ConsoleColor.Black;
            
            case 'o':
                return ConsoleColor.Yellow;

            case '#':
                return ConsoleColor.Gray;
            default:
                Console.ResetColor();
                return Console.ForegroundColor;

        }
    }

}

