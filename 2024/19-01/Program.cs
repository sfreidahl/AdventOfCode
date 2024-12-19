// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var towels = File.ReadAllText("input-towels.txt").Split(", ");
var patterns = File.ReadAllLines("input-patterns.txt");
Dictionary<string, bool> memo = [];

var result = patterns.Select(HandlePattern).Where(x => x).Count();

Console.WriteLine(result);

bool HandlePattern(string pattern){
    if(memo.ContainsKey(pattern)){
        return memo[pattern];
    }

    var newPatterns = FindNextTowels(pattern);
    if(newPatterns.Contains(string.Empty)){
        memo[pattern] = true;
        return true;
    }
    if(newPatterns.Count == 0){
        memo[pattern] = false;
        return false;
    }
    var result = newPatterns.Select(x => HandlePattern(x)).Any(x => x);
    memo[pattern] = result;
    return result;
}

List<string> FindNextTowels(string pattern){

    List<string> newPatterns = [];

    foreach(var towel in towels){
        if(pattern.StartsWith(towel)){
            newPatterns.Add(pattern[towel.Length..]);
        }
    }

    return newPatterns;

}
