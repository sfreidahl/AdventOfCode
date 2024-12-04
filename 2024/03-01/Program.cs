// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

var regex = new Regex(@"(mul\(\d{1,3},\d{1,3}\))");

var matches = regex.Matches(File.ReadAllText("input.txt"));
var result = matches
    .Where(x => x.Success)
    .Select(x => x.Value)
    .Select(x => x.Replace("mul(", "").Replace(")", "").Split(","))
    .Select(x => new Group(x))
    .Select(x => x.Calc())
    .Sum();

Console.WriteLine(result);

record struct Group(int X, int Y)
{
    public Group(int[] ints) : this(ints[0], ints[1]) { }

    public Group(string[] strings) : this(strings.Select(int.Parse).ToArray()) { }

    public int Calc() => X * Y;
}
