// See https://aka.ms/new-console-template for more information
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

record Machine(bool[] IndicatorLights, List<Button> Buttons)
{
    private string _lightsAsString = LightsToString(IndicatorLights);

    public static Machine FromString(string s)
    {
        var ss = s.Split(" ");

        return new(ss[0].Replace("[", "").Replace("]", "").Select(x => x == '#').ToArray(), ss[1..^1].Select(Button.FromString).ToList());
    }

    private Dictionary<string, long> cache = [];

    public long FewestPresses()
    {
        return FewestPresses(new bool[IndicatorLights.Length], 0);
    }

    private long FewestPresses(bool[] currentLights, long presses)
    {
        var lightsString = LightsToString(currentLights);

        if (lightsString == _lightsAsString)
        {
            return presses;
        }
        if (cache.TryGetValue(lightsString, out var cachedPresses) && cachedPresses <= presses)
        {
            return long.MaxValue;
        }

        cache[lightsString] = presses;

        long result = long.MaxValue;
        foreach (var button in Buttons)
        {
            var newLights = ClickButton(currentLights, button);
            var r = FewestPresses(newLights, presses + 1);
            result = Math.Min(result, r);
        }
        return result;
    }

    private static string LightsToString(bool[] lights) => new string(lights.Select(x => x ? '#' : '.').ToArray());

    private static bool[] ClickButton(bool[] lights, Button button)
    {
        bool[] newLights = [.. lights];
        foreach (var connection in button.Connections)
        {
            newLights[connection] = !newLights[connection];
        }
        return newLights;
    }
}
