// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt");

var results = new List<int>();

var numbers = new Dictionary<string, int>()
{
    { "0", 0 },
    { "1", 1 },
    { "2", 2 },
    { "3", 3 },
    { "4", 4 },
    { "5", 5 },
    { "6", 6 },
    { "7", 7 },
    { "8", 8 },
    { "9", 9 },
    { "zero", 0 },
    { "one", 1 },
    { "two", 2 },
    { "three", 3 },
    { "four", 4 },
    { "five", 5 },
    { "six", 6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine", 9 }
};

foreach (var line in lines)
{
    Console.WriteLine(line);

    var firstIndex = int.MaxValue;
    var firstNumber = 0;
    foreach (var number in numbers)
    {
        var index = line.IndexOf(number.Key);
        if (index < firstIndex && index != -1)
        {
            firstIndex = index;
            firstNumber = number.Value;
        }
    }

    var lastIndex = int.MinValue;
    var lastNumber = 0;
    foreach (var number in numbers)
    {
        var index = line.LastIndexOf(number.Key);
        if (index > lastIndex)
        {
            lastIndex = index;
            lastNumber = number.Value;
        }
    }

    results.Add(firstNumber * 10 + lastNumber);
    Console.WriteLine($"{firstNumber}{lastNumber}");
}

Console.WriteLine(results.Sum(x => x));
