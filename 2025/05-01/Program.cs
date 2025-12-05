// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllText("input.txt").Split(["\n\n", "\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries);

var ranges = input[0]
    .Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Split("-").Select(long.Parse).ToArray())
    .Select(x => new Range(x[0], x[1]));
var ingredients = input[1].Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

var result = ingredients.Where(ingredient => ranges.Any(range => range.Contains(ingredient))).Count();

Console.WriteLine(result);

record Range(long Start, long End)
{
    public bool Contains(long value) => value >= Start && value <= End;
}
