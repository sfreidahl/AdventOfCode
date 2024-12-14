// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata.Ecma335;

Console.WriteLine("Hello, World!");

List<Stone> stones = File.ReadAllText("input.txt").Split(" ").Select(long.Parse).Select(x => new Stone(x)).ToList();

for (int i = 0; i < 25; i++)
{
    stones = stones.SelectMany(x => x.Blink()).ToList();
}

Console.WriteLine(stones.Count);

record Stone(long number)
{
    public Stone(string number)
        : this(long.Parse(number)) { }

    public Stone[] Blink()
    {
        if (number == 0)
        {
            return [new(1)];
        }
        var stringNumber = number.ToString();
        if (stringNumber.Length % 2 == 0)
        {
            var midPoint = stringNumber.Length / 2;
            return [new(stringNumber[..midPoint]), new(stringNumber[midPoint..])];
        }
        return [new(number * 2024)];
    }
}
