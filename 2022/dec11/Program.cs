// using System.Numerics;
using System.Numerics;
using System.Text.Json.Serialization;
// using BigMath;

var input = File.ReadAllLines("input.txt");

List<Monkey> monkeys = new List<Monkey>();
for(int i = 0; i < input.Length; i += 7){
    var monkeyStrings = input[i..(i+6)];
    monkeys.Add(new Monkey(monkeyStrings, monkeys));
}

Monkey.LCM = monkeys.Select(x => x.Divisor).Aggregate((S, val) => S * val / gcd(S, val));
Console.WriteLine("LC:" + Monkey.LCM);

static long gcd(long n1, long n2)
{
    if (n2 == 0)
    {
        return n1;
    }
    else
    {
        return gcd(n2, n1 % n2);
    }
}



for(int i = 0; i < 10000; i++ ){
    foreach(var monkey in monkeys){
        monkey.InspectItems();
    }
}

foreach(var monkey in monkeys){
    Console.WriteLine(monkey.ItemsInspected);
}

var mostActiveMonkeys = monkeys.Select(x => x.ItemsInspected).OrderDescending().Take(2).ToArray();

Console.WriteLine(mostActiveMonkeys[0] * mostActiveMonkeys[1]);


class Monkey{
    public Queue<long> Items = new Queue<long>();
    public Operation Operation {get;init;}
    public long Divisor {get;init;}
    public int TrueMonkey {get;init;}
    public int FalseMonkey {get;init;}
    [JsonIgnore]
    public List<Monkey> Monkeys {get; init;}
    public long ItemsInspected {get;private set;} = 0;
    public static long LCM = 96577;

    public Monkey(string[] init, List<Monkey> monkeys){
        Items = new Queue<long>(init[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[2..].Select(x => long.Parse(x.Replace(",",""))));
        Operation = InitOperation(init[2]);
        Divisor = long.Parse(init[3].Split(" ")[^1]);
        TrueMonkey = int.Parse(init[4].Split(" ")[^1]);
        FalseMonkey = int.Parse(init[5].Split(" ")[^1]);
        Monkeys = monkeys;
    }

    public void InspectItems(){
        while(Items.Count > 0){
            ItemsInspected++;
            long item = Items.Dequeue();
            long worryLevel = Operation.PerformOperation(item ) ;
            var toMonkey = worryLevel % Divisor == 0 ? Monkeys[TrueMonkey] :  Monkeys[FalseMonkey];

            toMonkey.ThrowItem(worryLevel % LCM);
        }
    }

    public void ThrowItem(long item){
        Items.Enqueue(item);
    }

    private Operation InitOperation(string input){
        var pars = input.Split(" ");
        var value = pars[^1];
        if( value == "old" ){
            return new PowerOperation();
        }
        var longVal = long.Parse(value);
        if(pars[^2] == "*"){
            return new MultiplicationOperation(){OperatorValue = longVal};
        }
        
        return new AddOperation(){OperatorValue = longVal};
    }

    private long CalculateLcm(long a, long b){
        for (var tempNum = 1; ; tempNum++){
            if (tempNum % a == 0 && tempNum % b == 0){
                return tempNum;
            }
        }
    }

}

abstract class Operation{
    public abstract long PerformOperation(long input);
}

class AddOperation : Operation{
    public long OperatorValue {get;init;}
    public override long PerformOperation(long input)
    {
        return input + OperatorValue;
    }
}

class MultiplicationOperation : Operation{
    public long OperatorValue {get;init;}
    public override long PerformOperation(long input)
    {
        return input * OperatorValue;
    }
}

class PowerOperation : Operation{
    public override long PerformOperation(long input)
    {
        return input * input;
    }
}

