// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var races = new List<Race>() { new Race(44806572, 208158110501102) };

Console.WriteLine(races.Select(x => x.GetRecordDistances().Count).Aggregate(1, (src, acc) => acc * src));

record struct Race(long Time, long Distance)
{
    public List<long> GetDistances()
    {
        var distances = new List<long>();
        for (long i = 1; i < Time; i++)
        {
            var speed = i;
            distances.Add(speed * (Time - i));
        }

        return distances;
    }

    public List<long> GetRecordDistances()
    {
        var d = Distance;
        return GetDistances().Where(x => x > d).ToList();
    }
}
