// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var bricks = File.ReadLines("input.txt")
    .Select(x => x.Split(";")
        .Select(x => x.Split(","))
        .Select(x => new Location(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
        .ToList()
    ).Select(x => new Brick(x[0], x[1]));


List<Brick> fallenBricks = new List<Brick>();

foreach(var brick in bricks.OrderBy(x => Math.Min(x.Start.Z, x.End.Z))){
    var Z = Math.Min(brick.Start.Z, brick.End.Z);
}



record struct Location(int X, int Y, int Z);
record struct Brick(Location Start, Location End){
    public Direction GetDirection(){
        if(Start.X != End.X){
            return Direction.X;
        }
        if(Start.Y != End.Y){
            return Direction.Y;
        }
        if(Start.Z != End.Z){
            return Direction.Z;
        }
        return Direction.None;
    }
}

enum Direction{
    X,
    Y,
    Z,
    None
}
