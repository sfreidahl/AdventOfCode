Console.WriteLine("Hello, World!");

var input = File.ReadAllLines("input.txt");

var numberOfCards = input.Select((x, index) => (1, index + 1)).ToDictionary(x => x.Item2, x => x.Item1);

var cardToProcess = 1;
foreach (var line in input)
{
    var cardCount = numberOfCards[cardToProcess];
    var card = line.Split("|", StringSplitOptions.TrimEntries);
    var winners = card[0].Split(":")[1]
        .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(x => int.Parse(x))
        .ToHashSet();
    var myNumbers = card[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));
    var numberOfWinners = myNumbers.Aggregate(0, (acc, src) => acc + (winners.Contains(src) ? 1 : 0));

    for (int x = cardToProcess + 1; x <= cardToProcess + numberOfWinners; x++)
    {
        numberOfCards[x] += cardCount;
    }

    cardToProcess++;
}

Console.WriteLine(numberOfCards.Sum(x => x.Value));
