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
            if (s.Length % 2 == 1)
                continue;

            var halfLength = s.Length / 2;
            if (s[..halfLength] == s[halfLength..])
            {
                yield return i;
            }
        }
    }
}
