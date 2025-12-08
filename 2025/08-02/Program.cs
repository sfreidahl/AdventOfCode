// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var junctionBoxes = File.ReadAllLines("input.txt")
    .Select(x => x.Split(",").Select(int.Parse).ToArray())
    .Select((x, index) => new Coordinate(index, x[0], x[1], x[2]))
    .ToList();

List<CoordinatePair> distances = [];

for (int i = 0; i < junctionBoxes.Count; i++)
{
    for (int j = i + 1; j < junctionBoxes.Count; j++)
    {
        var one = junctionBoxes[i];
        var two = junctionBoxes[j];
        distances.Add(new(one, two));
    }
}

var sortedPairs = distances.OrderBy(x => x.distance).ToList();

HashSet<HashSet<Coordinate>> circuits = [];
long result = 0;
foreach (var pair in sortedPairs)
{
    if (pair.One.Circuit is null && pair.Two.Circuit is null)
    {
        HashSet<Coordinate> circuit = [pair.One, pair.Two];
        circuits.Add(circuit);
        pair.One.Circuit = circuit;
        pair.Two.Circuit = circuit;
        continue;
    }

    if (pair.One.Circuit is not null && pair.Two.Circuit is not null)
    {
        if (pair.One.Circuit == pair.Two.Circuit)
        {
            continue;
        }
        circuits.Remove(pair.One.Circuit);
        circuits.Remove(pair.Two.Circuit);
        HashSet<Coordinate> circuit = [.. pair.One.Circuit, .. pair.Two.Circuit];
        foreach (var box in circuit)
        {
            box.Circuit = circuit;
        }
        circuits.Add(circuit);
    }
    else if (pair.One.Circuit is not null)
    {
        pair.One.Circuit.Add(pair.Two);
        pair.Two.Circuit = pair.One.Circuit;
    }
    else if (pair.Two.Circuit is not null)
    {
        pair.Two.Circuit.Add(pair.One);
        pair.One.Circuit = pair.Two.Circuit;
    }
    if (pair.One.Circuit?.Count == junctionBoxes.Count)
    {
        result = pair.One.X * pair.Two.X;
        break;
    }
}

Console.WriteLine(result);

class Coordinate(int Number, int X, int Y, int Z)
{
    public HashSet<Coordinate>? Circuit;

    public int Number { get; } = Number;
    public int X { get; } = X;
    public int Y { get; } = Y;
    public int Z { get; } = Z;

    public double GetDistance(Coordinate coordinate) =>
        Math.Sqrt(Math.Pow(X - coordinate.X, 2) + Math.Pow(Y - coordinate.Y, 2) + Math.Pow(Z - coordinate.Z, 2));
}

record CoordinatePair(Coordinate One, Coordinate Two)
{
    public double distance = One.GetDistance(Two);
}
