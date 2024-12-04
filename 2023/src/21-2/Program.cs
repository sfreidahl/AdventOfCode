Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");
// char[,] board = new char[input.Length,input[0].Length];
Dictionary<Location, Node> nodes = new Dictionary<Location, Node>();
int rowIndex= 0;
Node? startNode = null;
foreach(var row in input ){
    int colIndex = 0;
    foreach (var col in row)
    {
        // board[rowIndex, colIndex] = col;
        if(col != '#'){
            Location l = new Location(rowIndex, colIndex);
            var n = new Node(l);
            nodes.Add(l, n);
            if(col == 'S'){
                startNode = n;
            }
        }
        colIndex++;
    }
    rowIndex++;
}
foreach(var node in nodes.Values){
    List<Location> connectedLocs = [
        new Location(node.Location.Row, node.Location.Col-1),
        new Location(node.Location.Row, node.Location.Col+1),
        new Location(node.Location.Row+1, node.Location.Col),
        new Location(node.Location.Row-1, node.Location.Col)
    ];
    foreach(var loc in connectedLocs){
        if(nodes.ContainsKey(loc)){
            node.Nodes.Add(nodes[loc]);
        }
    }
}

var plotsForEntireBoard = GetPlotsForLocation(startNode!, 1000);
var plotsForMiddle = GetPlotsForLocation(startNode!, 65);
var upperLeft = GetPlotsForLocation(nodes[new Location(0,0)], 64);
var lowerLeft = GetPlotsForLocation(nodes[new Location(input.Length - 1,0)], 64);
var upperRight = GetPlotsForLocation(nodes[new Location(0,input.Length - 1)], 64);
var lowerRight = GetPlotsForLocation(nodes[new Location(input.Length - 1,input.Length - 1)], 64);

var oddEdge =  plotsForEntireBoard.odd - plotsForMiddle.odd;
var evenEdge =  upperLeft.even + upperRight.even + lowerLeft.even + lowerRight.even;

Console.WriteLine($"Entire board: even: {plotsForEntireBoard.even} , odd: {plotsForEntireBoard.odd}");
Console.WriteLine($"Middle: even: {plotsForMiddle.even} , odd: {plotsForMiddle.odd}");
Console.WriteLine($"Edge: Even: {evenEdge} , odd: {oddEdge}");
Console.WriteLine($"Edge: Even: {plotsForEntireBoard.even - plotsForMiddle.even} , odd: {plotsForEntireBoard.odd - plotsForMiddle.odd}");



long stepsToTake = 26501365;
var remainder = stepsToTake % input.Length;
long boardsInDir = stepsToTake/ input.Length;
Console.WriteLine(boardsInDir);

var result = ((boardsInDir+1)*(boardsInDir+1) * plotsForEntireBoard.odd)
    + (boardsInDir*boardsInDir * plotsForEntireBoard.even)
    - ((boardsInDir+1) * oddEdge)
    + (boardsInDir*evenEdge);
Console.WriteLine(result);

// long averageBoard = (plotsForEntireBoard.even+plotsForEntireBoard.odd) / 2;
// long left = (boardsInDir - 2) * averageBoard + plotsForEntireBoard.even + (plotsForEntireBoard.odd - upperLeft.odd - lowerLeft.odd);
// long right = (boardsInDir - 2) * averageBoard + plotsForEntireBoard.even + (plotsForEntireBoard.odd - upperRight.odd - lowerRight.odd);
// long middle = left+right+plotsForEntireBoard.odd;

// long total = middle;


// for(long i = 1; i <= boardsInDir; i++){
//     long length = boardsInDir - i;
//     if(i % 2 == 0){
//         var ez = (length-1) * averageBoard;
//         long mid = plotsForEntireBoard.even;
//         long upLeft = plotsForEntireBoard.odd + lowerRight.even - upperLeft.odd;
//         long upRight = plotsForEntireBoard.odd + lowerLeft.even - upperRight.odd;

//         long downLeft = plotsForEntireBoard.odd + upperRight.even - lowerLeft.odd;
//         long downRight = plotsForEntireBoard.odd + upperLeft.even - lowerRight.odd;

//         total += (mid*2) + upLeft+upRight+downLeft+downRight + 4*ez;
//     }else{
//         var ez = (length * averageBoard) -plotsForEntireBoard.odd;
//         long mid = plotsForEntireBoard.odd;
//         long upLeft = plotsForEntireBoard.even + lowerRight.odd - upperLeft.even;
//         long upRight = plotsForEntireBoard.even + lowerLeft.odd - upperRight.even;

//         long downLeft = plotsForEntireBoard.even +upperRight.odd - lowerLeft.even;
//         long downRight = plotsForEntireBoard.even + upperLeft.odd - lowerRight.even;
//         total += (mid*2) + upLeft+upRight+downLeft+downRight + 4*ez;
//     }

// }

// total += (plotsForEntireBoard.odd*2) - upperLeft.odd - upperRight.odd - lowerLeft.odd - lowerRight.odd + upperLeft.even + upperRight.even + lowerLeft.even + lowerRight.even;

// Console.WriteLine(total);

// Console.WriteLine(evenBoard);
// Console.WriteLine(oddBoard);
// Console.WriteLine(remainder);
// Console.WriteLine(boardsInDir);
// var width = boardsInDir *2;
// long boardsNotInMiddle = 0;
// for (int i = width; i > 0; i--)
// {
//     boardsNotInMiddle += boardsInDir;
// }
// boardsNotInMiddle*=2;

// long middle = (boardsInDir * 2) + oddBoard + (oddBoard / 2);

// Console.WriteLine(middle+ (boardsNotInMiddle*averageBoard));




(long even, long odd) GetPlotsForLocation(Node StartNode, int Steps){
    HashSet<Node> currentNodes = [StartNode];

    var evenBoard = 0;
    var oddBoard = 0;

    var steps = 0;
    while(currentNodes.Count > 0 && steps <= Steps){
        var nextNodes = new List<Node>();
        foreach(var node in currentNodes){
            node.Visited = true;
            nextNodes.AddRange(node.Nodes.Where(x => !x.Visited));
            if(steps % 2 == 0){
                evenBoard++;
            }else{
                oddBoard++;
            }
        }
        currentNodes = nextNodes.ToHashSet();
        steps++;
    }
    nodes.Values.ToList().ForEach(x => x.Visited = false);
    return (evenBoard, oddBoard);
}

record struct Location(int Row, int Col);
record class Node(Location Location){
    public List<Node> Nodes { get; set; } = new List<Node>();
    public bool Visited { get; set; }
}

