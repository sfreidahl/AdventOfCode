// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");

List<Dictionary<int, char>> board = new();

for (int rowIndex = 0; rowIndex < input[0].Length; rowIndex++)
{
    var row = new Dictionary<int, char>();
    for (int lineIndex = 0; lineIndex < input.Length; lineIndex++)
    {
        var val = input[lineIndex][rowIndex];
        if (val == '.')
            continue;
        row.Add(lineIndex, val);
    }
    board.Add(row);
}

List<Dictionary<int, char>> newBoard = new();

foreach (var row in board)
{
    var newRow = new Dictionary<int, char>();
    var lastThing = -1;
    foreach (var line in row)
    {
        if (line.Value == '#')
        {
            newRow.Add(line.Key, line.Value);
            lastThing = line.Key;
            continue;
        }
        newRow.Add(lastThing + 1, line.Value);
        lastThing++;
    }
    newBoard.Add(newRow);
}

var result = 0;

int maxValue = input.Length;

Console.WriteLine(maxValue);

for (int i = 0; i < maxValue; i++)
{
    foreach (var row in newBoard)
    {
        if (row.ContainsKey(i))
        {
            if (row[i] == 'O')
            {
                result += maxValue - i;
            }
        }
    }
}

Console.WriteLine(result);
