// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var inputs = File.ReadAllLines("input-input.txt").Select(x => x.Split(": ")).Select(x => new Input(x[0], x[1] == "1")).ToList();
var wiresAndGates = File.ReadLines("input-gates.txt").ToList();

var wires = wiresAndGates.Select(x => x.Split(" -> ")).Select(x => new Wire(x[1])).ToList();
Dictionary<string, IInput> wireDic = wires.ToDictionary(x => x.Name, x => (IInput)x);
foreach (var input in inputs)
{
    wireDic.Add(input.Name, input);
}

var gates = wiresAndGates
    .Select(x => x.Split(" "))
    .Select(x => new Gate(Gate.GetTypeFromString(x[1]), wireDic[x[0]], wireDic[x[2]], (Wire)wireDic[x[4]]))
    .ToList();

var outputs = wires.Where(x => x.Name.StartsWith('z')).OrderByDescending(x => x.Name).ToList();

foreach (var output in outputs)
{
    Console.WriteLine(output.Name);
    _ = output.State;
    Console.WriteLine();
    Console.WriteLine();
}

Console.WriteLine(outputs.Aggregate(0L, (sum, val) => (sum * 2) + (val.State ? 1 : 0)));

class Gate : IInput
{
    public GateType Type { get; }

    public IInput InputA { get; }
    public IInput InputB { get; }
    public Wire Output { get; }

    public bool State =>
        Type switch
        {
            GateType.AND => InputA.State & InputB.State,
            GateType.OR => InputA.State | InputB.State,
            GateType.XOR => InputA.State ^ InputB.State,
            _ => throw new UnreachableException(),
        };

    public Gate(GateType type, IInput inputA, IInput inputB, Wire output)
    {
        Type = type;
        InputA = inputA;
        InputB = inputB;
        Output = output;
        Output.Input = this;
    }

    public static GateType GetTypeFromString(string s) =>
        s switch
        {
            "AND" => GateType.AND,
            "OR" => GateType.OR,
            "XOR" => GateType.XOR,
            _ => throw new UnreachableException(),
        };
}

class Input(string name, bool state) : IInput
{
    public string Name { get; } = name;
    public bool State
    {
        get
        {
            Console.Write(Name + " ");
            return state;
        }
    }
}

class Wire(string name) : IInput
{
    public IInput Input { get; set; } = null!;
    public string Name { get; } = name;
    public bool State => Input.State;
}

interface IInput
{
    bool State { get; }
}

enum GateType
{
    AND,
    OR,
    XOR,
}
