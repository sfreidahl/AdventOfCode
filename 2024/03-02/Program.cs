// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

var regex = new Regex(@"(mul\(\d{1,3},\d{1,3}\))");

var result = File.ReadAllText("input.txt")
    .Replace("do()", StringExtensions.Do)
    .Replace("don't()", StringExtensions.Dont)
    .GetDos()
    .SelectMany(GetGroups)
    .Select(x => x.Calc())
    .Sum();

Console.WriteLine(result);

List<Group> GetGroups(string input)
{
    var matches = regex.Matches(input);
    var result = matches
        .Where(x => x.Success)
        .Select(x => x.Value)
        .Select(x => x.Replace("mul(", "").Replace(")", "").Split(","))
        .Select(x => new Group(x));
    return result.ToList();
}

record struct Group(int X, int Y)
{
    public Group(int[] ints)
        : this(ints[0], ints[1]) { }

    public Group(string[] strings)
        : this(strings.Select(int.Parse).ToArray()) { }

    public int Calc() => X * Y;
}

public static class StringExtensions
{
    public const string Do = "|";
    public const string Dont = "§";

    public static IEnumerable<string> GetDos(this string input)
    {
        var firstDont = input.IndexOf(Dont);
        var currentString = input;
        yield return currentString[0..firstDont];
        currentString = input[firstDont..];
        while (true)
        {
            var indexOfDo = currentString.IndexOf(Do);
            if (indexOfDo == -1)
            {
                yield break;
            }
            currentString = currentString[indexOfDo..];
            var indexOfDont = currentString.IndexOf(Dont);
            if (indexOfDont == -1)
            {
                yield return currentString;
                yield break;
            }
            yield return currentString[..indexOfDont];
            currentString = currentString[indexOfDont..];
        }
    }
}
