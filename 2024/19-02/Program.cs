// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var towels = File.ReadAllText("input-towels.txt").Split(", ");
var patterns = File.ReadAllLines("input-patterns.txt");
Dictionary<string, long> memo = [];

var result = patterns.Select(HandlePattern).Sum();

Console.WriteLine(result);

long HandlePattern(string pattern)
{
    if (memo.ContainsKey(pattern))
    {
        return memo[pattern];
    }

    if (pattern == string.Empty)
    {
        return 1;
    }

    var newPatterns = FindNextTowels(pattern);
    if (newPatterns.Count == 0)
    {
        memo[pattern] = 0;
        return 0;
    }
    var result = newPatterns.Select(x => HandlePattern(x)).Sum();
    memo[pattern] = result;
    return result;
}

List<string> FindNextTowels(string pattern)
{
    List<string> newPatterns = [];

    foreach (var towel in towels)
    {
        if (pattern.StartsWith(towel))
        {
            newPatterns.Add(pattern[towel.Length..]);
        }
    }

    return newPatterns;
}
