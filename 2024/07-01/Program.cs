// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Diagnostics;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var result = File.ReadAllLines("test-input.txt")
    .Select(x => x.Split(":"))
    .Select(x => new Calibration(long.Parse(x[0]), x[1].Trim().Split(" ").Select(long.Parse).ToArray()))
    .Where(x => x.IsValid())
    .Select(x => x.Result)
    .Sum();

Console.WriteLine(result);

record struct Calibration(long Result, long[] Values)
{
    public bool IsValid()
    {
        List<Operator[]> operatorsList = new List<Operator[]>();
        var combinations = (int)Math.Pow(2, Values.Length - 1);
        for (int i = 0; i <= combinations - 1; i++)
        {
            var operators = new BitArray([i]).Cast<bool>().Select(x => x ? Operator.Multiply : Operator.Add).ToArray()[..(Values.Length - 1)];
            // Console.WriteLine($"{Result} - {Values.Length} - {i} - {JsonSerializer.Serialize(operators)}");
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
}

enum Operator
{
    Add,
    Multiply,
}
