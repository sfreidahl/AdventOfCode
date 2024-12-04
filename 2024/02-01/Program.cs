// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var result = File.ReadLines("input.txt")
    .Select(x => x.Split(" ").Select(int.Parse).ToList())
    .Aggregate(0, (acc, src) => acc + src.IsSafe());

Console.WriteLine(result);

public static class ListExtensions
{
    public static int IsSafe(this List<int> ints)
    {
        var prevValue = ints[1];
        int index = 2;
        var prevDirection = GetDirection(ints[0], ints[1]);
        do
        {
            var currentValue = ints[index];
            var direction = GetDirection(prevValue, currentValue);
            if (prevDirection != direction)
            {
                Console.WriteLine("UnSafe");
                return 0;
            }
            prevDirection = direction;
            prevValue = currentValue;
            index++;
        } while (index < ints.Count);

        Console.WriteLine("Safe");
        return 1;
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
}

enum Direction
{
    Increasing,
    Decreasing,
    Unsafe
}
