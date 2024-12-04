// Console.Clear();

// var input = File.ReadAllLines("input.txt");

// // var grid = new Dictionary<Location, char>();

// var sensors = input.Select(x => new Sensor(x)).ToList();
// var grid = new Grid(sensors, 2000000);

// // grid.Draw();

// Console.WriteLine("result: " + grid.GetRowNotCovered(2000000));

// struct Point{
//     public char Type { get; set; }
//     public Point(char type){
//         Type = type;
//     }
// }

// class Grid{
//     public int MinX { get; set; } = int.MaxValue;
//     public int MaxX { get; set; }
//     public int MinY { get; set; } = int.MaxValue;
//     public int MaxY { get; set; }
//     private int Width => MaxX - MinX;
//     private int Height => MaxY - MinY;
//     private int XOffset => MinX;
//     private int YOffset => MinY;
//     public int RowToCheck { get; set; }


//     public Dictionary<Location, Point> Points;

//     public Grid(List<Sensor> sensors, int rowToCheck){
//         RowToCheck = rowToCheck;
//         foreach(var sensor in sensors){
//             SetMinMax(sensor);
//         }
//         Points = new Dictionary<Location, Point>();
//         // Points = new Point?[Width, Height];
//         foreach(var sensor in sensors){
//             AddSensor(sensor);
//         }
//         // AddSensor(sensors)
//     }

//     public void AddSensor(Sensor sensor){
//         ChangeLocation(sensor.Location, new Point('S'));
//         ChangeLocation(sensor.Beacon.Location, new Point('B'));
//         HashSet<Location> coverage = sensor.GetCoverage(RowToCheck);
//         foreach(var location in coverage){
//             ChangeLocation(location, new Point('#'));
//         }
//     }

//     public void ChangeLocation(Location location, Point point){
//         if(location.Y != RowToCheck){
//             return;
//         }
//         var exists = Points.ContainsKey(location);
//         if(exists){
//             return;
//         }
//         // if(p?.Type != 'B' && p?.Type == 'S' ){
//         Points[location] = point;
//         // }
//         // location = GetOffsetLocation(location);

//         // Console.WriteLine("x: " + location.X + " y: " + location.Y);
//         // Points[location.X, location.Y] ??= point;
//     }

//     private void SetMinMax(Sensor s){
//         // SetMinMax(s.Location);
//         MinX = Math.Min(s.Location.X - (s.DistanceToBeacon()), MinX);
//         MaxX = Math.Max(s.Location.X + (s.DistanceToBeacon() + 1), MaxX);
//         MinY = Math.Min(s.Location.Y - (s.DistanceToBeacon()), MinY);
//         MaxY = Math.Max(s.Location.Y + (s.DistanceToBeacon() + 1), MaxY);
//         // SetMinMax(s.Beacon.Location);
//     }

//     private void SetMinMax(Location loc){

//     }

//     private Location GetOffsetLocation(Location location){
//         return new Location(location.X-XOffset, location.Y-YOffset);
//     }

//     public int GetRowNotCovered(int row){
//         int count = 0;
//         for(int i = MinX; i <= MaxX; i++){
//             if(GetPoint(new Location(i, row))?.Type == '#'){
//                 count++;
//             }
//         }
//         return count;
//     }

//     private Point? GetPoint(Location location){
//         if(!Points.ContainsKey(location)){
//             return null;
//         }
//         // location = GetOffsetLocation(location);
//         return Points.GetValueOrDefault(location);
//     }
    
//     public void Draw(){
//         var c = 0;
//         for(var i = MinY; i <= MaxY; i++){
//             Console.SetCursorPosition(0, c);
//             Console.Write(i);
//             c++;
//         }

//         foreach (var kv in Points){
//             var drawLoc = GetOffsetLocation(kv.Key);
//             Console.SetCursorPosition(drawLoc.X + 3, drawLoc.Y);
//             Console.Write(Points.GetValueOrDefault(kv.Key).Type);
//         }
//         // Console.Clear();
//         // for(var x = 0; x < Points.GetLength(0); x++){
//         //     for(var y = 0; y < Points.GetLength(1); y++){
//         //     }
//         // }
//         Console.SetCursorPosition(0, MaxY - YOffset);
//     }
// }

// struct Location{
//     public int X { get; set; }
//     public int Y { get; set; }
//     public Location(int x, int y){
//         X = x;
//         Y = y;
//     }

//     public Location(string input){
//         var s = input.Split(", ");
//         X = ParseInt(s[0]);
//         Y = ParseInt(s[1]);
//     }

//     private int ParseInt(string s){
//         return int.Parse(s.Split("=")[1]);;
//     }

//     public int Distance(Location other){
//         return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
//     }
// }

// class Sensor {
//     public Location Location { get; set; }
//     public Beacon Beacon { get; set; }

//     public Sensor(string input){
//         var s = input.Split(": ");
//         var locStr = s[0].Replace("Sensor at ", "");
//         Location = new Location(locStr);
//         Beacon = new Beacon(s[1], this);
//     }

//     public int DistanceToBeacon(){
//         return Location.Distance(Beacon.Location);
//     }

//     public HashSet<Location> GetCoverage(int rowToCheck){
//         var coverage = new List<Location>();
//         coverage.AddRange(GetCoverage(Direction.Up, rowToCheck));
//         coverage.AddRange(GetCoverage(Direction.Down, rowToCheck));
//         return coverage.ToHashSet();
//     }

//     private enum Direction{
//         Up,
//         Down
//     }

//     private List<Location> GetCoverage(Direction direction, int rowToCheck){
//         var coverage = new List<Location>();
//         int distance = DistanceToBeacon();
//         // Console.WriteLine(distance);
//         int yDistance = 0;
//         int d = direction == Direction.Up ? -1 : +1;
//         for(var y = Location.Y; y != Location.Y + ((distance +1 ) * d); y += d){
//             if(y != rowToCheck){
//                 yDistance++;
//                 continue;
//             }
//             var xDistance = (distance - yDistance);
//             var xStart =  Location.X - xDistance;
//             for(var x = xStart; x != (Location.X + xDistance) + 1; x++){
//                 coverage.Add(new Location(x,y));
//             }
//             yDistance++;
//         }
//         return coverage;
//     }

// }

// class Beacon {
//     public Location Location { get; set; }
//     public Sensor? Sensor { get; set; }
//     public Beacon(string input, Sensor sensor){
//         Sensor = sensor;
//         var s = input.Replace("closest beacon is at ", "");
//         Location = new Location(s);
//     }
// }

