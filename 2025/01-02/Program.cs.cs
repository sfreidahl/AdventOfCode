// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var result = File.ReadAllLines("input.txt")
    .Select(x => (x[..1], x[1..]))
    .Select(x => (Direction: x.Item1 == "L" ? -1 : 1, Distance: int.Parse(x.Item2)))
    .Aggregate(
        (Zeros: 0, Position: 50),
        (acc, src) =>
        {
            var extraZeroes = src.Distance / 100;
            var actualDist = src.Distance % 100;
            var locAbs = acc.Position + (actualDist * src.Direction);
            var zero = (locAbs >= 100 || locAbs <= 0) && acc.Position != 0;
            var loc = Mod(locAbs, 100);
            return (acc.Zeros + (zero ? 1 : 0) + extraZeroes, loc);
        }
    );

Console.WriteLine(result);

static int Mod(int x, int m)
{
    int r = x % m;
    return r < 0 ? r + m : r;
}
