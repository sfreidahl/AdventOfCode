Console.Clear();

var input = File.ReadAllLines("input.txt");

// var grid = new Dictionary<Location, char>();

var sensors = input.Select(x => new Sensor(x)).ToList();
// var grid = new Grid(sensors, 2000000);
HashSet<Location> allEdges = new HashSet<Location>();
var sensorCount = 0;
var max = 4000000;
foreach(var sensor in sensors){
    Console.WriteLine("Sensor:" + sensorCount);
    Location? notCovered = sensor.TestEdge(sensors, max);
    if(notCovered != null){
        var loc = notCovered.Value;
        Console.WriteLine("x: " + loc!.X + " y: " + loc.Y + " Result: " + ((long)loc.X * 4000000L + (long)loc.Y));
        break;
    }
    sensorCount++;
}

struct Location{
    public int X { get; set; }
    public int Y { get; set; }
    public Location(int x, int y){
        X = x;
        Y = y;
    }

    public Location(string input){
        var s = input.Split(", ");
        X = ParseInt(s[0]);
        Y = ParseInt(s[1]);
    }

    private int ParseInt(string s){
        return int.Parse(s.Split("=")[1]);;
    }

    public int Distance(Location other){
        return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
    }
    
}

class Sensor {
    public Location Location { get; set; }
    public Beacon Beacon { get; set; }

    public Sensor(string input){
        var s = input.Split(": ");
        var locStr = s[0].Replace("Sensor at ", "");
        Location = new Location(locStr);
        Beacon = new Beacon(s[1], this);
    }

    public int DistanceToBeacon(){
        return Location.Distance(Beacon.Location);
    }

    public Location? TestEdge(List<Sensor> sensors, int max){
        return TestEdge(Direction.Up, sensors, max) ?? TestEdge(Direction.Down, sensors, max);
    }

    private enum Direction{
        Up,
        Down
    }

    private Location? TestEdge(Direction direction, List<Sensor> sensors, int max){
        var coverage = new List<Location>();
        int distance = DistanceToBeacon();
        // Console.WriteLine(distance);
        int yDistance = 0;
        int d = direction == Direction.Up ? -1 : +1;
        for(var y = Location.Y; y != Location.Y + ((distance + 2 ) * d); y += d){
        // Console.WriteLine(y);
            // Console.WriteLine(y);
            var xDistance = (distance - yDistance);
            var xStart =  Location.X - xDistance - 1;
            var endX = (Location.X + xDistance) + 1;
            var location = TestLocation(sensors, new Location( xStart, y ), max) ?? TestLocation(sensors, new Location( endX, y ), max);
            if(location != null){
                return location;
            }
            // coverage.Add(new Location( endX, y ));
            yDistance++;
            // var covered1 = false;
            // var covered2 = false;
            // foreach (var sensor in sensors)
            // {
            //     if(sensor.Covers(locationLeft)){
            //         covered1 = true;
            //         break;
            //     }
            //     if(sensor.Covers(locationRight)){
            //         covered2 = true;
            //         break;
            //     }
            // }
            // if(!covered1){
            //     return locationLeft;
            // }
            // if(!covered2){
            //     return new Location( endX, y );
            // }
        }
        return null;
    }

    public Location? TestLocation(List<Sensor> sensors, Location location, int max){
        var covered = false;

        if(location.X > max || location.X < 0 || location.Y > max || location.Y < 0){
            return null;
        }
        foreach (var sensor in sensors)
        {
            if(sensor.Covers(location)){
                covered = true;
                break;
            }
        }
        if(!covered){
            return location;
        }
        return null;
    }
    // public HashSet<Location> GetCoverage(int rowToCheck){
    //     var coverage = new List<Location>();
    //     coverage.AddRange(GetCoverage(Direction.Up, rowToCheck));
    //     coverage.AddRange(GetCoverage(Direction.Down, rowToCheck));
    //     return coverage.ToHashSet();
    // }

    public bool Covers(Location location){
        return location.Distance(Location) <= DistanceToBeacon(); 
    }

}

class Beacon {
    public Location Location { get; set; }
    public Sensor? Sensor { get; set; }
    public Beacon(string input, Sensor sensor){
        Sensor = sensor;
        var s = input.Replace("closest beacon is at ", "");
        Location = new Location(s);
    }
}

