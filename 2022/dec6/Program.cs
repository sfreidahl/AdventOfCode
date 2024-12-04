
Console.Clear();
var input = File.ReadAllText("input.txt");
Console.CursorVisible = false;
var queue = new Queue<char>(input.Take(14));
for(int i = 14; i < input.Length; i++){
    var start = int.Max(0, i -20);
    var end = i +34;
    var c = input[i];
    queue.Dequeue();
    queue.Enqueue(c);

    Console.CursorLeft = 0;
    Console.ResetColor();
    Console.Write(input[start..end]);
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.CursorLeft = int.Max(0, i-start);
    foreach (char item in queue)
    {
        Console.Write(item);
    }
    Thread.Sleep(50);
    if( queue.ToHashSet().Count() == 14){
        Console.WriteLine(i + 1);
        break;
 
    }
}