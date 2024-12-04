var input = File.ReadAllLines("input.txt");

var score = 0;

foreach(var round in input){
    var s = round.Split(" ");
    var opponent = GetHand(s[0]);
    var result = GetSupposedResult(s[1]);
    score  += (int)result + (int)GetResult(opponent, result);
}

Console.WriteLine(score);

Hand GetResult(Hand a, Result b){
    if(b == Result.Draw){
        return a;
    } 
    if(b == Result.Win){
        return (Hand)(((int)a) % 3) + 1;
    }
    return (Hand)(((int)a+1) % 3) + 1;
}

Hand GetHand( string hand){
    switch (hand)
    {
        case "A": return Hand.Rock;
        case "B": return Hand.Paper;
        case "C": return Hand.Scissors;
        default: throw new Exception("BONK!");
    }
}

Result GetSupposedResult( string hand){
    switch (hand)
    {
        case "X": return Result.Loss;
        case "Y": return Result.Draw;
        case "Z": return Result.Win;
        default: throw new Exception("BONK!");
    }
}

enum Hand{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

enum Result{
    Loss = 0,
    Draw = 3,
    Win = 6
}