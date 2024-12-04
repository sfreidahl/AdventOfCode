// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt");

var results = new List<int>();

var numbers = new HashSet<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

foreach (var line in lines)
{
    var result = 0;
    foreach (var character in line)
    {
        if (numbers.Contains(character))
        {
            result += (character - 48) * 10;
            break;
        }
    }

    for (int i = line.Length - 1; i >= 0; i--)
    {
        var character = line[i];
        if (numbers.Contains(character))
        {
            result += character - 48;
            break;
        }
    }
    results.Add(result);
}

Console.WriteLine(results.Sum(x => x));
