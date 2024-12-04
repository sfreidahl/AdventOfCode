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

    var possible =
        round.Where(x => x.Color == "red" && x.count > 12 || x.Color == "green" && x.count > 13 || x.Color == "blue" && x.count > 14).Count() == 0;
    if (possible)
    {
        result.Add(gameNumber);
    }
}

Console.WriteLine(result.Sum(x => x));

public record struct CubeCount(string Color, int count);
