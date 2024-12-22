// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var startNumbers = File.ReadAllLines("input.txt").Select(long.Parse);

List<Dictionary<Sequence, long>> dics = [];
foreach (var startNumber in startNumbers)
{
    Dictionary<Sequence, long> values = [];
    Queue<long> queue = new Queue<long>();
    var price = startNumber % 10;
    var secretNumber = startNumber;
    for (int i = 1; i <= 2000; i++)
    {
        var nextNumber = secretNumber.NextSecretNumber();
        var newPrice = nextNumber % 10;
        queue.Enqueue(newPrice - price);
        price = newPrice;
        secretNumber = nextNumber;
        if (queue.Count == 5)
        {
            queue.Dequeue();
        }
        if (queue.Count < 4)
        {
            continue;
        }
        var sequence = new Sequence(queue.ToList());
        if (values.ContainsKey(sequence))
        {
            continue;
        }
        values[sequence] = newPrice;
    }
    dics.Add(values);
}

var result = dics.SelectMany(x => x).GroupBy(x => x.Key, x => x.Value).ToDictionary(x => x.Key, x => x.Sum()).MaxBy(x => x.Value);

Console.WriteLine(result);

record Sequence(long one, long two, long three, long four)
{
    public Sequence(List<long> ints)
        : this(ints[0], ints[1], ints[2], ints[3]) { }
}

static class LongExtensions
{
    public static long NSecretNumber(this long secretNumber, long n)
    {
        for (var i = 0; i < n; i++)
        {
            secretNumber = secretNumber.NextSecretNumber();
        }
        return secretNumber;
    }

    public static long NextSecretNumber(this long secretNumber)
    {
        secretNumber = secretNumber.Mix(secretNumber * 64L).Prune();
        secretNumber = secretNumber.Mix(secretNumber / 32L).Prune();
        secretNumber = secretNumber.Mix(secretNumber * 2048L).Prune();
        return secretNumber;
    }

    public static long Mix(this long secretNumber, long number) => secretNumber ^ number;

    public static long Prune(this long secretNumber) => secretNumber % 16777216;
}
