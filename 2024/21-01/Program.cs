// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var codes = File.ReadAllLines("input.txt");

var keypad = new Keypad(
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
    "Keypad"
);

var dPad1 = new Keypad([new(new(0, 1), '^'), new(new(0, 2), 'A'), new(new(1, 0), '<'), new(new(1, 1), 'v'), new(new(1, 2), '>')], Type.DPad, "dPad1");
var dPad2 = new Keypad([new(new(0, 1), '^'), new(new(0, 2), 'A'), new(new(1, 0), '<'), new(new(1, 1), 'v'), new(new(1, 2), '>')], Type.DPad, "dPad2");

var result = 0;

foreach (var code in codes)
{
    List<char> moves = [];
    foreach (var b in code)
    {
        Console.Write($"{b}: ");
        var keypadMoves = keypad.Move(b);
        foreach (var keypadMove in keypadMoves)
        {
            var dpad1Moves = dPad1.Move(keypadMove);
            foreach (var dpad1Move in dpad1Moves)
            {
                var dpad2Moves = dPad2.Move(dpad1Move);
                foreach (var c in dpad2Moves)
                {
                    Console.Write(c);
                }
                moves.AddRange(dpad2Moves);
            }

            // Console.Write($"{keypad.CurrentVal}");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    foreach (var c in moves)
    {
        Console.Write(c);
    }
    Console.WriteLine();
    Console.WriteLine($"Code:{code} Value: {int.Parse(code[..^1])} Length: {moves.Count} Result: {moves.Count * int.Parse(code[..^1])} ");
    result += moves.Count * int.Parse(code[..^1]);
}

Console.WriteLine(result);

record Distance(int Row, int Column);

record Coordinate(int Row, int Column)
{
    public Distance GetDistance(Coordinate coordinate) => new(coordinate.Row - Row, coordinate.Column - Column);
}

record Button(Coordinate Coordinate, char Value);

class Keypad(List<Button> buttons, Type type, string name)
{
    private readonly List<Button> _buttons = buttons;
    private readonly Type _type = type;
    private Coordinate _location = buttons.Where(x => x.Value == 'A').First().Coordinate;
    public char CurrentVal => _buttons.Where(x => x.Coordinate == _location).First().Value;

    public string Name { get; } = name;

    public List<char> Move(char c)
    {
        var newLocation = _buttons.Where(x => x.Value == c).First().Coordinate;
        List<char> moves = [];
        var distance = _location.GetDistance(newLocation);
        if (_type == Type.KeyPad)
        {
            Console.Write($"{c}: ");
            if (CrossesInvalid(newLocation) && distance.Row < 0 || !CrossesInvalid(newLocation) && distance.Row > 0)
            {
                Console.Write($"{c}: UpDown");
                moves.AddRange(GetUpDownMovement(distance));
                moves.AddRange(GetLeftRightMovement(distance));
            }
            else
            {
                Console.Write($"{c}: LeftRight");
                moves.AddRange(GetLeftRightMovement(distance));
                moves.AddRange(GetUpDownMovement(distance));
            }
            Console.WriteLine();
        }
        if (_type == Type.DPad)
        {
            if (CrossesInvalid(newLocation) && distance.Row > 0)
            {
                moves.AddRange(GetUpDownMovement(distance));
                moves.AddRange(GetLeftRightMovement(distance));
            }
            else
            {
                moves.AddRange(GetLeftRightMovement(distance));
                moves.AddRange(GetUpDownMovement(distance));
            }
        }
        // Console.WriteLine($"{name} - from: {_location} to: {newLocation} moves: {new string([.. moves, 'A'])}");
        _location = newLocation;
        return [.. moves, 'A'];
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
