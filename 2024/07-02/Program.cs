// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt")
    .Select(x => x.Split(":"))
    .Select(x => new Calibration(long.Parse(x[0]), x[1].Trim().Split(" ").Select(long.Parse).ToArray()));
var sw = new Stopwatch();
sw.Start();
var result = input.Where(x => x.IsValid()).Select(x => x.Result).Sum();
sw.Stop();
Console.WriteLine($"milliseconds elapsed: {sw.ElapsedMilliseconds}");
Console.WriteLine(result);

record struct Calibration(long Result, long[] Values)
{
    public bool IsValid()
    {
        return CheckOperators(Values[0], Values[1..]);
    }

    private bool CheckOperators(long currentValue, long[] remainingValues)
    {
        if (currentValue == Result && remainingValues.Length == 0)
        {
            return true;
        }
        if (currentValue > Result)
        {
            return false;
        }
        if (remainingValues.Length == 0)
        {
            return false;
        }
        var nextValue = remainingValues[0];
        var nextRemaining = remainingValues[1..];

        var addResult = CheckOperators(currentValue + nextValue, nextRemaining);
        var multiplyResult = CheckOperators(currentValue * nextValue, nextRemaining);
        var concatValues = CheckOperators(long.Parse($"{currentValue}{nextValue}"), nextRemaining);

        return addResult || multiplyResult || concatValues;
    }
}

enum Operator
{
    Add,
    Multiply,
}
