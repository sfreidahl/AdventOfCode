// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");

var result = 0;

foreach (var line in input)
{
    var card = line.Split("|", StringSplitOptions.TrimEntries);

    var winners = card[0].Split(":")[1]
        .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(x => int.Parse(x))
        .ToHashSet();
    var myNumbers = card[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));

    var numberOfWinners = myNumbers.Aggregate(0, (acc, src) => acc + (winners.Contains(src) ? 1 : 0));
    Console.WriteLine(numberOfWinners);
    if (numberOfWinners > 0)
    {
        result += (int)Math.Pow(2, numberOfWinners - 1);
    }
}

Console.WriteLine(result);
