// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");

var numbers = new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

Dictionary<Location, List<int>> starDic = new Dictionary<Location, List<int>>();

for (int row = 0; row < input.Length; row++)
{
    var line = input[row];
    var foundNumber = "";
    Location? adjacentStar = null;
    for (int column = 0; column < line.Length; column++)
    {
        var symbol = line[column];
        if (numbers.Contains(symbol))
        {
            foundNumber += symbol;
            adjacentStar ??= HasAdjacentStar(row, column);
            continue;
        }
        if (!string.IsNullOrEmpty(foundNumber))
        {
            if (adjacentStar != null)
            {
                var number = int.Parse(foundNumber);
                if (!starDic.ContainsKey((Location)adjacentStar))
                {
                    starDic.Add((Location)adjacentStar, new List<int>());
                }
                starDic[(Location)adjacentStar].Add(number);
            }
            adjacentStar = null;
            foundNumber = "";
        }
    }
    if (!string.IsNullOrEmpty(foundNumber))
    {
        if (adjacentStar != null)
        {
            var number = int.Parse(foundNumber);
            if (!starDic.ContainsKey((Location)adjacentStar))
            {
                starDic.Add((Location)adjacentStar, new List<int>());
            }
            starDic[(Location)adjacentStar].Add(number);
        }
        adjacentStar = null;
        foundNumber = "";
    }
}

var result = starDic.Where(x => x.Value.Count == 2).Select(x => x.Value).Select(x => x[0] * x[1]).Sum(x => x);

Console.WriteLine(result);

Location? HasAdjacentStar(int row, int column)
{
    Location? result = null;
    if (row > 0)
    {
        result ??= SymbolsOnRow(row - 1, column);
    }
    result = result ??= SymbolsOnRow(row, column);
    if (row < input.Length - 1)
    {
        result = result ??= SymbolsOnRow(row + 1, column);
    }

    return result;
}

Location? SymbolsOnRow(int row, int column)
{
    Console.WriteLine($"{row}x{column}");
    if (IsStar(input[row][column]))
    {
        return new Location(row, column);
    }
    if (column > 0)
    {
        if (IsStar(input[row][column - 1]))
        {
            return new Location(row, column - 1);
        }
    }
    if (column < input[row].Length - 1)
    {
        if (IsStar(input[row][column + 1]))
        {
            return new Location(row, column + 1);
        }
    }

    return null;
}

bool IsStar(char c)
{
    return c == '*';
}

record struct Location(int Row, int Column);
