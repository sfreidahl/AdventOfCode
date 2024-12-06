// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var Maze = new Maze(File.ReadAllLines("input.txt").Select(x => x.Select(x => x).ToArray()).ToArray());

Console.WriteLine(Maze.FindLoops());

class Maze
{
    private const char _startIcon = '^';
    private char[][] _map;

    public Maze(char[][] map)
    {
        _map = map;
    }

    public HashSet<Position> MoveThroughMaze(){
        var currentPosition = (MoveResult: MoveResult.Start, Position: GetStartPosition(), Direction: Direction.Up);
        HashSet<Position> visited = [];
        do{
            currentPosition = Move(currentPosition.Position, currentPosition.Direction);
            if(currentPosition.MoveResult == MoveResult.Exit){
                return visited;
            }
            visited.Add(currentPosition.Position);
        }while(true);
    }

    public int FindLoops(){
        var sw = new Stopwatch();
        sw.Start();
        var startPosition = GetStartPosition();
        var positionsToBlock = MoveThroughMaze();
        positionsToBlock.Remove(startPosition);
        var mapsToTry =positionsToBlock.Select((position) => {
            char[][] newMap = [.._map.Select(x => x.ToArray())];
            newMap[position.Row][position.Column] = '#';
            return new Maze(newMap);
        });
        var result = mapsToTry.Where(x => x.IsLoop()).Count();
        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
        return result;
    }

    private bool IsLoop(){
        var currentPosition = (MoveResult: MoveResult.Start, Position: GetStartPosition(), Direction: Direction.Up);
        HashSet<PreviousPosition> visited = [new(currentPosition.Position, currentPosition.Direction) ];
        do{
            currentPosition = Move(currentPosition.Position, currentPosition.Direction);
            if(currentPosition.MoveResult == MoveResult.Exit){
                return false;
            }
            if(!visited.Add(new(currentPosition.Position, currentPosition.Direction))){
                return true;
            }
        }while(true);
    }

    private Position GetStartPosition()
    {
        for (int rowIndex = 0; rowIndex < _map.Count(); rowIndex++)
        {
            var row = _map[rowIndex];
            var startIndex = Array.IndexOf(row,_startIcon);
            if (startIndex == -1)
            {
                continue;
            }

            return new(rowIndex, startIndex);
        }
        throw new UnreachableException("");
    }

    private (MoveResult MoveResult, Position Position, Direction Direction ) Move(Position position, Direction direction) {
        var nextPos = position.GetNextPosition(direction);
        if(IsExit(nextPos)){
            return (MoveResult.Exit, nextPos, direction);
        }
        if(IsWall(nextPos)){
            var newDirection = Turn(direction);
            return Move(position, newDirection);
        }
        return (MoveResult.Moved, nextPos, direction);
    }

    private bool IsWall(Position position){
        return _map[position.Row][position.Column] == '#';
    }

    private bool IsExit(Position position){
        if(position.Row == -1) {
            return true;
        }
        if(position.Column == -1) {
            return true;
        }
        if(position.Row == _map.Count()) {
            return true;
        }
        if(position.Column == _map[0].Count()) {
            return true;
        }
        return false;
    }

    private Direction Turn(Direction direction){
        return direction switch {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new UnreachableException("invalid direction")
        };
    }
}
enum Direction
{
    Up,
    Right,
    Down,
    Left
}

enum MoveResult
{
    Start,
    Moved,
    Wall,
    Exit
}

record struct PreviousPosition(Position Position, Direction direction);

record struct Position(int Row, int Column)
{
    public Position GetNextPosition(Direction direction)
    {
        return direction switch
        {
            Direction.Up => this with { Row = Row - 1 },
            Direction.Right => this with { Column = Column + 1 },
            Direction.Down => this with { Row = Row + 1 },
            Direction.Left => this with { Column = Column - 1 },
            _ => throw new UnreachableException("invalid direction")
        };
    }
}
