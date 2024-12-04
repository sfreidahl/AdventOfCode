// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");

char[,] board = new char[input.Length, input[0].Length];

for (int columnIndex = 0; columnIndex < input[0].Length; columnIndex++)
{
    for (int lineIndex = 0; lineIndex < input.Length; lineIndex++)
    {
        board[lineIndex, columnIndex] = input[lineIndex][columnIndex];
    }
}

List<string> results = new List<string>();

int maxValue = input.GetLength(0);
StringBuilder sb = new StringBuilder();
for (int cycle = 0; cycle < 1000000000; cycle++)
{
    var cycleResult = 0;
    // Console.WriteLine(board.Clone);
    // if(cycle % 1000000 == 0){
    Console.WriteLine($"{cycle}/1000000000");
    // }
    // NORTH TILT
    for (int columnIndex = 0; columnIndex < board.GetLength(1); columnIndex++)
    {
        var lastThing = -1;
        for (int lineIndex = 0; lineIndex < board.GetLength(0); lineIndex++)
        {
            if (board[lineIndex, columnIndex] == '.')
                continue;
            if (board[lineIndex, columnIndex] == '#')
            {
                lastThing = lineIndex;
                continue;
            }
            board[lineIndex, columnIndex] = '.';
            board[lastThing + 1, columnIndex] = 'O';
            lastThing++;
        }
    }

    // WEST TILT
    for (int lineIndex = 0; lineIndex < board.GetLength(1); lineIndex++)
    {
        var lastThing = -1;
        for (int columnIndex = 0; columnIndex < board.GetLength(0); columnIndex++)
        {
            if (board[lineIndex, columnIndex] == '.')
                continue;
            if (board[lineIndex, columnIndex] == '#')
            {
                lastThing = columnIndex;
                continue;
            }
            board[lineIndex, columnIndex] = '.';
            board[lineIndex, lastThing + 1] = 'O';
            lastThing++;
        }
    }

    // SOUTH TILT
    for (int columnIndex = 0; columnIndex < board.GetLength(1); columnIndex++)
    {
        var lastThing = board.GetLength(1);
        for (int lineIndex = board.GetLength(1) - 1; lineIndex >= 0; lineIndex--)
        {
            if (board[lineIndex, columnIndex] == '.')
                continue;
            if (board[lineIndex, columnIndex] == '#')
            {
                lastThing = lineIndex;
                continue;
            }

            board[lineIndex, columnIndex] = '.';
            board[lastThing - 1, columnIndex] = 'O';
            lastThing--;
        }
    }

    // EAST TILT
    for (int lineIndex = 0; lineIndex < board.GetLength(1); lineIndex++)
    {
        var lastThing = board.GetLength(1);
        for (int columnIndex = board.GetLength(0) - 1; columnIndex >= 0; columnIndex--)
        {
            if (board[lineIndex, columnIndex] == '.')
                continue;
            if (board[lineIndex, columnIndex] == '#')
            {
                lastThing = columnIndex;
                continue;
            }
            board[lineIndex, columnIndex] = '.';
            board[lineIndex, lastThing - 1] = 'O';
            cycleResult += maxValue - lastThing - 1;
            lastThing--;
        }
    }
    foreach (var c in board)
    {
        sb.Append(c);
    }
    string r = sb.ToString();
    // Console.WriteLine(r);
    if (results.Contains(r))
    {
        var index = results.IndexOf(r);
        var arr = results.ToArray()[index..];
        var cycleLength = arr.Length;
        // Console.WriteLine(arr.Length);
        var remaining = 999999999 - cycle;
        var rem = remaining % cycleLength;
        var c = arr[rem];
        int row = 0;
        int col = 0;
        foreach (var ch in c)
        {
            // Console.WriteLine($"{row} - {col}");
            board[row, col] = ch;
            col++;
            if (col == board.GetLength(0))
            {
                row++;
            }
            col = col % board.GetLength(0);
        }
        break;
    }
    results.Add(r);
    sb.Clear();
}

var result = 0;

// Console.WriteLine(maxValue);



Console.WriteLine();
for (int i = 0; i < maxValue; i++)
{
    Console.WriteLine();
    for (int j = 0; j < maxValue; j++)
    {
        Console.Write(board[i, j]);
        if (board[i, j] == 'O')
        {
            result += maxValue - i;
        }
    }
}

Console.WriteLine(result);

// void PrintBoard(char[,] board){
//     Console.WriteLine();
//     for(int i = 0; i < board.GetLength(0); i++){
//         Console.WriteLine();
//         for(int j = 0; j < board.GetLength(1); j++){
//             Console.Write(board[i,j]);

//         }
//     }
// }
