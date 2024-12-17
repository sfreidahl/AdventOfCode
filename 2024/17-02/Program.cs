﻿// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

long a = 0;
long b = 0;
long c = 0;
int pointer = 0;

string program = "2,4,1,1,7,5,0,3,1,4,4,5,5,5,3,0,";

List<int> instructions = [2, 4, 1, 1, 7, 5, 0, 3, 1, 4, 4, 5, 5, 5, 3, 0];

string result = "";

long val = 0;
while (true)
{
    val *= 8;
    Console.WriteLine($"val - {val}");
    for (int i = 0; i <= 16; i++)
    {
        Console.WriteLine($"i - {i}");
        a = val + i;
        Run();
        if (result == "")
        {
            continue;
        }
        if (program.EndsWith(result))
        {
            Console.WriteLine($"{result} - {val + i}");
            val += i;
            break;
        }
    }
    if (program == result)
    {
        break;
    }
}

Console.WriteLine(val);

void Run()
{
    b = 0;
    c = 0;
    pointer = 0;
    result = "";
    while (pointer < instructions.Count)
    {
        OpCode opCode = (OpCode)instructions[pointer];
        long operand = instructions[pointer + 1];
        var increasePointer = opCode switch
        {
            OpCode.Adv => Adv(operand),
            OpCode.Bxl => Bxl(operand),
            OpCode.Bst => Bst(operand),
            OpCode.Jnz => Jnz(operand),
            OpCode.Bxc => Bxc(operand),
            OpCode.Out => Out(operand),
            OpCode.Bdv => Bdv(operand),
            OpCode.Cdv => Cdv(operand),
            _ => throw new UnreachableException("Invalid opcode"),
        };

        if (increasePointer)
        {
            pointer += 2;
        }
        if (result.Length > program.Length)
        {
            pointer = int.MaxValue;
        }
    }
}

bool Adv(long operand)
{
    a /= (long)Math.Pow(2, ComboOperand(operand));
    return true;
}

bool Bxl(long operand)
{
    b ^= operand;
    return true;
}

bool Bst(long operand)
{
    b = ComboOperand(operand) % 8;
    return true;
}

bool Jnz(long operand)
{
    if (a == 0)
    {
        return true;
    }
    pointer = (int)operand;
    return false;
}

bool Bxc(long operand)
{
    b ^= c;
    return true;
}

bool Out(long operand)
{
    result += $"{ComboOperand(operand) % 8},";
    // Console.WriteLine(result);
    return true;
}

bool Bdv(long operand)
{
    b = a / (long)Math.Pow(2, ComboOperand(operand));
    return true;
}
bool Cdv(long operand)
{
    c = a / (long)Math.Pow(2, ComboOperand(operand));
    return true;
}

long ComboOperand(long operand)
{
    return operand switch
    {
        0 or 1 or 2 or 3 => operand,
        4 => a,
        5 => b,
        6 => c,
        _ => throw new UnreachableException(),
    };
}

public enum OpCode
{
    Adv,
    Bxl,
    Bst,
    Jnz,
    Bxc,
    Out,
    Bdv,
    Cdv,
}
