// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllText("input.txt").Split(',');

List<Lens>[] boxes = new List<Lens>[256];
for (int i = 0; i < 256; i++)
{
    boxes[i] = new List<Lens>();
}

foreach (var command in input)
{
    if (command.Contains('-'))
    {
        var label = command.Replace("-", "");
        var hash = CreateHash(label);
        boxes[hash].Remove(boxes[hash].Find(x => x.Label == label));
    }
    else
    {
        var s = command.Split('=');
        var label = s[0];
        var power = int.Parse(s[1]);
        var lens = new Lens(label, power);
        var hash = CreateHash(label);
        var lensIndex = boxes[hash].IndexOf(boxes[hash].Find(x => x.Label == label));
        if (lensIndex == -1)
        {
            boxes[hash].Add(lens);
        }
        else
        {
            boxes[hash][lensIndex] = lens;
        }
    }
}

var result = 0;
for (int i = 0; i < boxes.Length; i++)
{
    Console.WriteLine();
    Console.Write($"Box {i}:");
    for (int index = 0; index < boxes[i].Count; index++)
    {
        Console.Write($" [{boxes[i][index].Label} {boxes[i][index].Power}]");
        result += (i + 1) * (index + 1) * boxes[i][index].Power;
    }
}
Console.WriteLine();

Console.WriteLine(result);

int CreateHash(string s)
{
    return s.Aggregate(0, (acc, src) => (acc + src) * 17 % 256);
}

record struct Lens(string Label, int Power);
