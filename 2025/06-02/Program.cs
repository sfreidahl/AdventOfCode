// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt").ToArray();

var rows = input.Length;
var columns = input[0].Length;

long result = 0;
var currentNumbers = new List<long>();
for (int column = columns - 1; column >= 0; column--)
{
    var currentNumber = "";
    for (int row = 0; row < rows; row++)
    {
        if (row == rows - 1)
        {
            if (string.IsNullOrWhiteSpace(currentNumber))
            {
                continue;
            }
            currentNumbers.Add(long.Parse(currentNumber));
            var sign = input[row][column];
            if (sign == ' ')
            {
                break;
            }
            var innerResult = currentNumbers[0];
            for (int i = 1; i < currentNumbers.Count; i++)
            {
                innerResult = sign switch
                {
                    '+' => innerResult + currentNumbers[i],
                    '*' => innerResult * currentNumbers[i],
                    _ => throw new Exception("the fuck?"),
                };
            }
            result += innerResult;
            currentNumber = "";
            currentNumbers = [];
            break;
        }
        currentNumber += input[row][column];
    }
}

Console.WriteLine(result);
