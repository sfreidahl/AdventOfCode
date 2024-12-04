Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");
char[,] board = new char[input.Length,input[0].Length];
Dictionary<Location, Node> nodes = new Dictionary<Location, Node>();
int rowIndex= 0;
Node? startNode = null;
foreach(var row in input ){
    int colIndex = 0;
    foreach (var col in row)
    {
        board[rowIndex, colIndex] = col;
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

HashSet<Node> currentNodes = [startNode!];

var result = 0;

for(int step = 0; step <= 200; step++){
    var nextNodes = new List<Node>();
    foreach(var node in currentNodes){
        node.Visited = true;
        nextNodes.AddRange(node.Nodes.Where(x => !x.Visited));
        if(step % 2 != 0){
            board[node.Location.Row, node.Location.Col] = 'O';
            result++;
        }
    }
    currentNodes = nextNodes.ToHashSet();
}

for(int i = 0; i < board.GetLength(0); i++){
    Console.WriteLine();
    for(int j = 0; j < board.GetLength(1); j++){
        Console.Write(board[i,j]);
    }
}

Console.WriteLine(result);

record struct Location(int Row, int Col);
record class Node(Location Location){
    public List<Node> Nodes { get; set; } = new List<Node>();
    public bool Visited { get; set; }
}
