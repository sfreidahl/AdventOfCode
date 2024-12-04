// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Dictionary<int, IList<IList<CubeCount>>> input = new();

List<int> result = new();

foreach (var line in File.ReadAllLines("input.txt"))
{
    var game = line.Split(":");
    var gameNumber = int.Parse(game[0].Replace("Game", "").Trim());

    var round = game[1]
        .Split(";,".ToCharArray())
        .Select(x =>
        {
            var y = x.Trim().Split(" ");
            return new CubeCount(y[1], int.Parse(y[0]));
        })
        .ToList();

    var redCount = round.Where(x => x.Color == "red").Max(x => x.Count);
    var blueCount = round.Where(x => x.Color == "blue").Max(x => x.Count);
    var greenCount = round.Where(x => x.Color == "green").Max(x => x.Count);
    result.Add(redCount * blueCount * greenCount);
}

Console.WriteLine(result.Sum(x => x));

public record struct CubeCount(string Color, int Count);
