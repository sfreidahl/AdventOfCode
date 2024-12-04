// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt")
    .Select(x => x.Split(" ").Select(x => int.Parse(x)).ToList())
    .ToList()
    .Select(x =>
    {
        x.Reverse();
        return x;
    });

var result = 0;

foreach (var sequence in input)
{
    var newSequences = new List<List<int>>() { sequence };
    var nextSequence = sequence.ToList();
    while (nextSequence.Any(x => x != 0))
    {
        nextSequence = NextSequense(nextSequence);
        newSequences.Add(nextSequence);
    }

    for (int i = newSequences.Count - 1; i >= 1; i--)
    {
        var newNumber = newSequences[i].Last() + newSequences[i - 1].Last();
        newSequences[i - 1].Add(newNumber);
    }
    result += newSequences.First().Last();
}

Console.WriteLine(result);

List<int> NextSequense(IEnumerable<int> sequence)
{
    List<int> nextSequense = new List<int>();
    var prevNumber = sequence.First();
    foreach (var number in sequence.ToArray()[1..])
    {
        nextSequense.Add(number - prevNumber);
        prevNumber = number;
    }

    return nextSequense;
}
