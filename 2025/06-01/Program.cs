// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt")
    .Select(row => row.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    .ToArray();

var numbers = input.Length;
var problems = input[0].Length;

long result = 0;
for (int problem = 0; problem < problems; problem++)
{
    var sign = input[numbers - 1][problem];
    var innerResult = long.Parse(input[0][problem]);
    for (int number = 1; number < numbers - 1; number++)
    {
        innerResult = sign switch
        {
            "+" => innerResult + long.Parse(input[number][problem]),
            "*" => innerResult * long.Parse(input[number][problem]),
            _ => throw new Exception("the fuck?"),
        };
    }
    result += innerResult;
}

Console.WriteLine(result);
