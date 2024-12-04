// See https://aka.ms/new-console-template for more information
using System.Xml.XPath;

Console.WriteLine("Hello, World!");

List<int> left = new();
List<int> right = new();

File.ReadAllText("input.txt")
    .Split("\r\n")
    .ToList()
    .ForEach(x =>
    {
        var vals = x.Split("   ");
        left.Add(int.Parse(vals[0]));
        right.Add(int.Parse(vals[1]));
    });

left = left.Order().ToList();
right = right.Order().ToList();

var numberCount = right.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

var result = left.Aggregate(0, (acc, src) => acc + (src * numberCount.GetValueOrDefault(src)));

Console.WriteLine(result);
