// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, World!");

var result = File.ReadLines("input.txt").Select(x => x.Split(" ").Select(int.Parse).ToList()).Aggregate(0, (acc, src) => acc + src.IsSafe());

Console.WriteLine(result);

public static class ListExtensions
{
    public static int IsSafe(this List<int> ints, bool hasIssue = false)
    {
        var prevValue = ints[0];
        var direction = Direction.Unknown;
        for (int i = 1; i < ints.Count; i++)
        {
            var value = ints[i];
            var isSafe = IsSafe(prevValue, value, direction);
            if (!isSafe.IsSafe)
            {
                if (hasIssue)
                {
                    return 0;
                }
                return ints.IsSafe([i - 2, i - 1, i]);
            }
            prevValue = value;
            direction = isSafe.Direction;
        }
        return 1;
    }

    public static int IsSafe(this List<int> ints, List<int> indexesToRemove)
    {
        foreach (var index in indexesToRemove)
        {
            if (index < 0)
            {
                continue;
            }
            var r = ints.IsSafe(index);
            if (r == 1)
            {
                return 1;
            }
        }
        return 0;
    }

    public static int IsSafe(this List<int> ints, int indexToRemove)
    {
        List<int> newInts = [.. ints];
        newInts.RemoveAt(indexToRemove);
        return newInts.IsSafe(true);
    }

    static Direction GetDirection(int first, int second)
    {
        if (Math.Abs(first - second) > 3)
        {
            return Direction.Unsafe;
        }
        if (first == second)
        {
            return Direction.Unsafe;
        }

        if (first > second)
        {
            return Direction.Decreasing;
        }

        return Direction.Increasing;
    }

    static (bool IsSafe, Direction Direction) IsSafe(int first, int second, Direction direction)
    {
        var newDirection = GetDirection(first, second);
        if (newDirection == Direction.Unsafe)
        {
            return (false, newDirection);
        }
        if (newDirection != direction && direction != Direction.Unknown)
        {
            return (false, newDirection);
        }

        return (true, newDirection);
    }
}

enum Direction
{
    Increasing,
    Decreasing,
    Unsafe,
    Unknown,
}
