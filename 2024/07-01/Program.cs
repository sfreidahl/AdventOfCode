// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt")
    .Select(x => x.Split(":"))
    .Select(x => new Calibration(long.Parse(x[0]), x[1].Trim().Split(" ").Select(long.Parse).ToArray()));
Stopwatch sw = new Stopwatch();
sw.Start();
var result = input.Where(x => x.IsValid()).Select(x => x.Result).Sum();
sw.Stop();

Console.WriteLine($"Elapsed milliseconds: {sw.ElapsedMilliseconds}");

sw.Restart();
var resultRecursive = input.Where(x => x.IsValid()).Select(x => x.Result).Sum();
sw.Stop();

Console.WriteLine($"Elapsed milliseconds: {sw.ElapsedMilliseconds}");

Console.WriteLine(result);
Console.WriteLine(resultRecursive);

record struct Calibration(long Result, long[] Values)
{
    public bool IsValid()
    {
        List<Operator[]> operatorsList = new List<Operator[]>();
        var combinations = (int)Math.Pow(2, Values.Length - 1);
        for (int i = 0; i <= combinations - 1; i++)
        {
            var operators = new BitArray([i]).Cast<bool>().Select(x => x ? Operator.Multiply : Operator.Add).ToArray()[..(Values.Length - 1)];
            operatorsList.Add(operators);
        }

        foreach (var operators in operatorsList)
        {
            var result = CheckOperators(operators);
            if (result)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckOperators(Operator[] operators)
    {
        var result = Values[0];
        var index = 1;
        foreach (var op in operators)
        {
            var val = Values[index];
            try
            {
                checked
                {
                    result = op switch
                    {
                        Operator.Add => result + val,
                        Operator.Multiply => result * val,
                        _ => throw new UnreachableException(),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Overflow");
                return false;
            }
            if (result > Result)
            {
                return false;
            }
            index++;
        }
        return Result == result;
    }

    public bool IsValidRecursive()
    {
        return CheckOperatorsRecursive(Values[0], Values[1..]);
    }

    private bool CheckOperatorsRecursive(long currentValue, long[] remainingValues)
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

        var addResult = CheckOperatorsRecursive(currentValue + nextValue, nextRemaining);
        var multiplyResult = CheckOperatorsRecursive(currentValue * nextValue, nextRemaining);

        return addResult || multiplyResult;
    }
}

enum Operator
{
    Add,
    Multiply,
}
