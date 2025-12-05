var input = File.ReadAllText("input.txt").Split(["\n\n", "\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries);

var ranges = input[0]
    .Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Split("-").Select(long.Parse).ToArray())
    .Select(x => new Range(x[0], x[1]))
    .OrderBy(x => x.Start);

var q = new Queue<Range>(ranges);

long result = 0;
while (q.Count > 0)
{
    var range = q.Dequeue();
    do
    {
        if (!q.TryPeek(out Range? next) || !range.Overlaps(next))
        {
            result += range.Count();
            break;
        }
        range = range.Combine(next);
        q.Dequeue();
    } while (true);
}

Console.WriteLine(result);

record Range(long Start, long End)
{
    public bool Contains(long value) => value >= Start && value <= End;

    public bool Overlaps(Range range) => Contains(range.Start) || Contains(range.End);

    public Range Combine(Range range) => new(Math.Min(Start, range.Start), Math.Max(End, range.End));

    public long Count() => End - Start + 1;
}
