// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, World!");

var machines = File.ReadAllLines("input.txt").Select(Machine.FromString);

var result = machines.Select(x => x.FewestPresses()).Sum();

Console.WriteLine(result);

record Button(int[] Connections)
{
    public static Button FromString(string s)
    {
        var ss = s.Replace("(", "").Replace(")", "");
        return new(ss.Split(",").Select(int.Parse).ToArray());
    }
}

record Machine(int[] Joltage, List<Button> Buttons)
{
    private string _joltageAsString = JoltageToString(Joltage);

    public static Machine FromString(string s)
    {
        var ss = s.Split(" ");

        return new(
            ss[^1].Replace("{", "").Replace("}", "").Split(",").ToDictionary((x, index) => int.Parse).ToArray(),
            ss[1..^1].Select(Button.FromString).OrderByDescending(x => x.Connections.Count()).ToList()
        );
    }

    private Dictionary<string, long> cache = [];
    private long _fewestPresses = long.MaxValue;

    public long FewestPresses()
    {
        Dictionary<int, int> indexOrder = [];
        for (int i = 0; i <= 9; i++)
        {
            indexOrder[i] = Joltage[i];
            // var count = Buttons.Where(x => x.Connections.Contains(i)).Count();
            // Console.WriteLine($"{i}: {count}");
        }
        Console.ReadLine();
        return 0;
    }

    private static string JoltageToString(int[] joltage) => string.Join(",", joltage.Select(x => x.ToString()));

    private static int[] ClickButton(int[] joltage, Button button)
    {
        int[] newJoltage = [.. joltage];
        foreach (var connection in button.Connections)
        {
            newJoltage[connection] += 1;
        }
        return newJoltage;
    }
}
