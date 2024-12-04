
var input = File.ReadAllLines("input.txt");

var forest = input.Select(row => row.Select(tree => Convert.ToInt32(""+tree)).ToList()).ToList();

var rowIndex = 0;
var seenTrees = 0;

var max = 0;

foreach(var row in forest){
    var treeIndex = -1;
    foreach(var treeHeight in row){
        treeIndex++;
        var seenLeft = 0;
        foreach(var t in row.ToArray()[..treeIndex].Reverse()){
            seenLeft++;
            if( t >=treeHeight){
                break;
            }
        }
        
        var seenRight = 0;
        foreach(var t in row.ToArray()[(treeIndex+1)..]){
            seenRight++;
            if( t >= treeHeight){
                break;
            }
        }

        var seenAbove = 0;
        for(int i = rowIndex-1; i >= 0; i--){
            seenAbove++;
            if(forest[i][treeIndex] >= treeHeight){
                break;
            }
        }

        var seenBelow = 0;
        for(int i = rowIndex + 1; i < forest.Count; i++){
            seenBelow++;
            if(forest[i][treeIndex] >= treeHeight){
                break;
            }
        }

        var result = seenLeft* seenRight * seenBelow * seenAbove;
        max = Math.Max(result, max);



    }
    Console.WriteLine();
    rowIndex++;
}

Console.WriteLine(max);