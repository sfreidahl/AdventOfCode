// See https://aka.ms/new-console-template for more information
using System.Xml.XPath;

Console.WriteLine("Hello, World!");

List<int> left = new();
List<int> right = new();

File.ReadAllText("input.txt")
    .Split("\r\n")
    .ToList()
    .ForEach(x =>
    {
        var vals = x.Split("   ");
        left.Add(int.Parse(vals[0]));
        right.Add(int.Parse(vals[1]));
    });

left = left.Order().ToList();
right = right.Order().ToList();

List<Pair> pairs = new();

for (int i = 0; i < left.Count; i++)
{
    pairs.Add(new(left[i], right[i]));
}

int result = pairs.Select(x => x.Difference).Sum();

Console.WriteLine(result);

record struct Pair(int Left, int Right)
{
    public int Difference = Math.Abs(Left - Right);
}
