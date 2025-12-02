// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var result = File.ReadAllText("input.txt")
    .Split(",")
    .Select(x => x.Split("-"))
    .Select(x => new Range(long.Parse(x[0]), long.Parse(x[1])))
    .SelectMany(x => x.GetFakeIds())
    .Sum();

Console.WriteLine(result);

public record Range(long Start, long End)
{
    public IEnumerable<long> GetFakeIds()
    {
        for (long i = Start; i <= End; i++)
        {
            var s = i.ToString();
            var halfLength = s.Length / 2;

            for (int l = 1; l <= halfLength; l++)
            {
                if (s.Length % l != 0)
                    continue;

                var segments = s.Chunkify(l);
                if (segments.Same())
                {
                    Console.WriteLine(i);
                    yield return i;
                    break;
                }
            }
        }
    }
}

public static class StringExtensions
{
    public static IEnumerable<string> Chunkify(this string s, int length)
    {
        var chunks = s.Length / length;
        for (int i = 0; i < chunks; i++)
        {
            yield return s[(i * length)..((i + 1) * length)];
        }
    }
}

public static class EnumerableExtensions
{
    public static bool Same<T>(this IEnumerable<T> items)
    {
        var l = items.ToList();
        var f = l[0]!;
        foreach (var item in l[1..])
        {
            if (!f.Equals(item))
            {
                return false;
            }
        }
        return true;
    }
}
