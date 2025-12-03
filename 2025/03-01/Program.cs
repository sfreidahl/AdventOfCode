// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var result = File.ReadAllLines("input.txt").Select(x => x.Select(c => c - 48).ToList()).Select(FindJoltage).Sum();

Console.WriteLine(result);

static int FindJoltage(List<int> batteries)
{
    var maxLeft = batteries[..^1].Max();
    var maxIndex = batteries.IndexOf(maxLeft);
    var maxRight = batteries[(maxIndex + 1)..].Max();

    var joltage = maxLeft * 10 + maxRight;

    Console.WriteLine(joltage);

    return joltage;
}
