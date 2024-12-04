var input = File.ReadAllLines("input.txt");

var Head = new Knot(2);
foreach(var line in input){
    var command = line.Split(" ");
    var dir = command[0];
    var length = Convert.ToInt32(command[1]);
    var sign = dir == "L" || dir == "D" ? -1 : 1;
    var isX = dir == "L" || dir == "R";
    for(int i = 0; i < length; i++){
        Head.Move(sign, isX);
    }
    // var headLocation = Head.Loc;
    // var left = headLocation.X - 15;
    // var top = headLocation.Y - 15;
    // Console.SetCursorPosition(0,0);
    // for(var i = 0; i < 30;i++){
    //     for(var j = 0; j < 30; j++){
    //         var vertLine = (left - i) % 5 == 0;
    //         var horLine = (top - j) % 5 == 0;
    //         var toDraw = ' ';
    //         if((left - i) % 5 == 0){

    //         }
    //     }
    // }
}

Console.WriteLine(Head.GetTailLocationCount());

struct Location{
    public int X {get;set;}
    public int Y {get;set;}
}
class Knot{
    public Location Loc = new Location();
    public Knot? Tail;

    public Knot(){

    }

    public Knot(int length){
        length--;
        if(length > 0){
            Tail = new Knot(length);
        }
    }

    public void Move(int sign, bool isX){
        if(isX){
            Loc.X += sign;
        }else{
            Loc.Y += sign;
        }
        Tail?.Follow(this);
    }

    public void Follow(Knot head){
        Follow2(head);
        HasBeen.Add(new Location(){
            X = Loc.X,
            Y = Loc.Y
        });
        Tail?.Follow(this);
    }

    private void Follow2(Knot head){
        var xMove = ShouldMove(Loc.X, head.Loc.X);
        var yMove = ShouldMove(Loc.Y, head.Loc.Y);
        if(xMove != 0 && Loc.Y != head.Loc.Y || yMove != 0 && Loc.X != head.Loc.X){
            MoveDiagonal(head);
            return;
        }
        Loc.X += xMove;
        Loc.Y += yMove;

    }

    private void MoveDiagonal(Knot other){
            Loc.X += Loc.X > other.Loc.X ? -1 : 1;
            Loc.Y += Loc.Y > other.Loc.Y ? -1 : 1;
            return;
    }

    private int ShouldMove(int a, int b){
        if(Math.Abs(a - b) > 1){
            return a > b ? -1 : 1;
        }
        return 0;
    }

    public int GetTailLocationCount(){
        Console.WriteLine(HasBeen.Count);
        return Tail?.GetTailLocationCount() ?? HasBeen.Count;
    }

    public HashSet<Location> HasBeen = new HashSet<Location>();
}