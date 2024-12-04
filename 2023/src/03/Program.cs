// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");

var result = 0;

var numbers = new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

for (int row = 0; row < input.Length; row++)
{
    var line = input[row];
    var foundNumber = "";
    var adjacentSymbol = false;
    for (int column = 0; column < line.Length; column++)
    {
        var symbol = line[column];
        if (numbers.Contains(symbol))
        {
            foundNumber += symbol;
            adjacentSymbol = adjacentSymbol || HasAdjacentSymbol(row, column);
            continue;
        }
        if (!string.IsNullOrEmpty(foundNumber))
        {
            if (adjacentSymbol)
            {
                result += int.Parse(foundNumber);
            }
            adjacentSymbol = false;
            foundNumber = "";
        }
    }
    if (!string.IsNullOrEmpty(foundNumber))
    {
        if (adjacentSymbol)
        {
            result += int.Parse(foundNumber);
        }
        adjacentSymbol = false;
        foundNumber = "";
    }
}

Console.WriteLine(result);

bool HasAdjacentSymbol(int row, int column)
{
    var result = false;
    if (row > 0)
    {
        result = result || SymbolsOnRow(row - 1, column);
    }
    result = result || SymbolsOnRow(row, column);
    if (row < input.Length - 1)
    {
        result = result || SymbolsOnRow(row + 1, column);
    }

    return result;
}

bool SymbolsOnRow(int row, int column)
{
    Console.WriteLine($"{row}x{column}");
    var result = false;
    result = result || IsSymbol(input[row][column]);
    if (column > 0)
    {
        result = result || IsSymbol(input[row][column - 1]);
    }
    if (column < input[row].Length - 1)
    {
        result = result || IsSymbol(input[row][column + 1]);
    }

    return result;
}

bool IsSymbol(char c)
{
    if (numbers.Contains(c))
        return false;
    if (c == '.')
        return false;
    return true;
}
