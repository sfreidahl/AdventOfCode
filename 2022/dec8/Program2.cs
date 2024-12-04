
// var input = File.ReadAllLines("input.txt");

// var forest = input.Select(row => row.Select(tree => Convert.ToInt32(""+tree)).ToList()).ToList();

// var rowIndex = 0;
// var seenTrees = 0;
// foreach(var row in forest){
//     var treeIndex = -1;
//     foreach(var treeHeight in row){
//         treeIndex++;
//         if(rowIndex == 0 || treeIndex == 0 || rowIndex == forest.Count() - 1 || treeIndex == row.Count() - 1){
//             Console.Write("1");
//             seenTrees++;
//             continue;
//         }
//         if(row.ToArray()[..treeIndex].Max() < treeHeight ){
//             // Console.WriteLine(treeHeight);
//             // Console.WriteLine(string.Join("",row.ToArray()[..treeIndex]));
//             Console.Write("L");
//             seenTrees++;
//             continue;
//         };
//         if(row.ToArray()[(treeIndex+1)..].Max() < treeHeight){
//             Console.Write("R");
//             seenTrees++;
//             continue;
//         }
//         var maxAbove = 0;
//         for(int i = rowIndex-1; i >= 0; i--){
//             maxAbove = Math.Max(forest[i][treeIndex], maxAbove);
//         }
//         var maxBelow = 0;
//         for(int i = rowIndex + 1; i < forest.Count; i++){
//             maxBelow = Math.Max(forest[i][treeIndex], maxBelow);
//         }
//         if(maxAbove < treeHeight ){
//             Console.Write("A");
//             seenTrees++;
//             continue;
//         }
//         if( maxBelow < treeHeight){
//             Console.Write("B");
//             seenTrees++;
//             continue;
//         }
//         Console.Write(" ");



//     }
//     Console.WriteLine();
//     rowIndex++;
// }

// Console.WriteLine(seenTrees);