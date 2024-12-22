// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var codes = File.ReadAllLines("input.txt");

Keypad? keypad = null;

for (var i = 0; i < 25; i++)
{
    keypad = new Keypad([new(new(0, 1), '^'), new(new(0, 2), 'A'), new(new(1, 0), '<'), new(new(1, 1), 'v'), new(new(1, 2), '>')], Type.DPad, keypad);
}

keypad = new Keypad(
    [
        new(new(0, 0), '7'),
        new(new(0, 1), '8'),
        new(new(0, 2), '9'),
        new(new(1, 0), '4'),
        new(new(1, 1), '5'),
        new(new(1, 2), '6'),
        new(new(2, 0), '1'),
        new(new(2, 1), '2'),
        new(new(2, 2), '3'),
        new(new(3, 1), '0'),
        new(new(3, 2), 'A'),
    ],
    Type.KeyPad,
    keypad
);

long result = 0;

foreach (var code in codes)
{
    var cost = keypad.Move(code.ToList());
    Console.WriteLine(result);
    Console.WriteLine($"{cost}*{long.Parse(code[..^1])} = {cost * long.Parse(code[..^1])}");
    result += cost * long.Parse(code[..^1]);
}

Console.WriteLine(result);

record Distance(int Row, int Column);

record struct Coordinate(int Row, int Column)
{
    public Distance GetDistance(Coordinate coordinate) => new(coordinate.Row - Row, coordinate.Column - Column);
}

record Button(Coordinate Coordinate, char Value);

class Keypad(List<Button> buttons, Type type, Keypad? SourceKeypad)
{
    private readonly List<Button> _buttons = buttons;
    private readonly Type _type = type;
    private Coordinate _location = buttons.Where(x => x.Value == 'A').First().Coordinate;
    private Keypad? _sourceKeypad = SourceKeypad;
    private Dictionary<(Coordinate Start, Coordinate End), long> _memo = [];

    public long Move(List<char> code)
    {
        long result = 0;
        foreach (char c in code)
        {
            result += CostToMove(c);
        }
        return result;
    }

    public long CostToMove(char c)
    {
        var newLocation = _buttons.Where(x => x.Value == c).First().Coordinate;
        var memoKey = (_location, newLocation);
        if (_memo.TryGetValue(memoKey, out var r))
        {
            _location = newLocation;
            return r;
        }

        long cost = 0;
        var distance = _location.GetDistance(newLocation);
        if (_type == Type.KeyPad)
        {
            if (CrossesInvalid(newLocation) && distance.Row < 0)
            {
                List<char> tempMoves1 = [.. GetUpDownMovement(distance), .. GetLeftRightMovement(distance), 'A'];
                cost = _sourceKeypad?.Move(tempMoves1) ?? 0;
            }
            else if (CrossesInvalid(newLocation) && distance.Row > 0)
            {
                List<char> tempMoves1 = [.. GetLeftRightMovement(distance), .. GetUpDownMovement(distance), 'A'];
                cost = _sourceKeypad?.Move(tempMoves1) ?? 0;
            }
            else
            {
                List<char> tempMoves1 = [.. GetUpDownMovement(distance), .. GetLeftRightMovement(distance), 'A'];
                List<char> tempMoves2 = [.. GetLeftRightMovement(distance), .. GetUpDownMovement(distance), 'A'];

                long cost1 = _sourceKeypad?.Move(tempMoves1) ?? 0;
                long cost2 = _sourceKeypad?.Move(tempMoves2) ?? 0;
                cost = Math.Min(cost1, cost2);
            }
        }
        if (_type == Type.DPad)
        {
            if (CrossesInvalid(newLocation) && distance.Row < 0)
            {
                List<char> tempMoves1 = [.. GetLeftRightMovement(distance), .. GetUpDownMovement(distance), 'A'];
                cost = _sourceKeypad?.Move(tempMoves1) ?? tempMoves1.Count;
            }
            else if (CrossesInvalid(newLocation) && distance.Row > 0)
            {
                List<char> tempMoves1 = [.. GetUpDownMovement(distance), .. GetLeftRightMovement(distance), 'A'];
                cost = _sourceKeypad?.Move(tempMoves1) ?? tempMoves1.Count;
            }
            else
            {
                List<char> tempMoves1 = [.. GetUpDownMovement(distance), .. GetLeftRightMovement(distance), 'A'];
                List<char> tempMoves2 = [.. GetLeftRightMovement(distance), .. GetUpDownMovement(distance), 'A'];

                long cost1 = _sourceKeypad?.Move(tempMoves1) ?? tempMoves1.Count;
                long cost2 = _sourceKeypad?.Move(tempMoves2) ?? tempMoves1.Count;
                cost = Math.Min(cost1, cost2);
            }
        }

        _location = newLocation;
        _memo.Add(memoKey, cost);
        return cost;
    }

    private bool CrossesInvalid(Coordinate newLocation)
    {
        var val = _type switch
        {
            Type.KeyPad => newLocation.Column == 0 && _location.Row == 3 || newLocation.Row == 3 && _location.Column == 0,
            Type.DPad => newLocation.Column == 0 && _location.Row == 0 || newLocation.Row == 0 && _location.Column == 0,
            _ => throw new UnreachableException(),
        };
        return val;
    }

    private List<char> GetLeftRightMovement(Distance distance)
    {
        List<char> list = [];
        if (distance.Column > 0)
        {
            list.AddMany('>', distance.Column);
            return list;
        }

        list.AddMany('<', -distance.Column);
        return list;
    }

    private List<char> GetUpDownMovement(Distance distance)
    {
        List<char> list = [];
        if (distance.Row > 0)
        {
            list.AddMany('v', distance.Row);
            return list;
        }

        list.AddMany('^', -distance.Row);
        return list;
    }
}

enum Type
{
    KeyPad,
    DPad,
}

static class ListExtensions
{
    public static void AddMany<T>(this List<T> list, T value, int times)
    {
        for (var i = 0; i < times; i++)
        {
            list.Add(value);
        }
    }
}
