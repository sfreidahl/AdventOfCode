// var input = File.ReadAllLines("input.txt");

// Dictionary<Hand, int> scoreDict = new Dictionary<Hand, int>() {
//     {Hand.Rock, 1},
//     {Hand.Paper, 2},
//     {Hand.Scissors, 3},
// };

// var score = 0;

// foreach(var round in input){
//     var s = round.Split(" ");
//     var opponent = GetHand(s[0]);
//     var player = GetHand(s[1]);
//     score  += (int)player + GetResult(opponent, player);
// }

// Console.WriteLine(score);

// int GetResult(Hand a, Hand b){
//    if(a == b){
//     return 3;
//    } 
//    if((((int)a) % 3) + 1 == (int)b){
//     return 6;
//    }
//    return 0;
// }

// Hand GetHand( string hand){
//     switch (hand)
//     {
//         case "A": return Hand.Rock;
//         case "B": return Hand.Paper;
//         case "C": return Hand.Scissors;
//         case "X": return Hand.Rock;
//         case "Y": return Hand.Paper;
//         case "Z": return Hand.Scissors;
//         default: throw new Exception("BONK!");
//     }
// }

// enum Hand{
//     Rock = 1,
//     Paper = 2,
//     Scissors = 3
// }