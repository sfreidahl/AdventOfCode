// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var races = new List<Race>() { new Race(44, 208), new Race(80, 1581), new Race(65, 1050), new Race(72, 1102) };

Console.WriteLine(races.Select(x => x.GetRecordDistances().Count).Aggregate(1, (src, acc) => acc * src));

record struct Race(int Time, int Distance)
{
    public List<int> GetDistances()
    {
        var distances = new List<int>();
        for (int i = 1; i < Time; i++)
        {
            var speed = i;
            distances.Add(speed * (Time - i));
        }

        return distances;
    }

    public List<int> GetRecordDistances()
    {
        var d = Distance;
        return GetDistances().Where(x => x > d).ToList();
    }
}
