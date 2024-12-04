using System.Text.Json;

var moduleList = File.ReadAllLines("input.txt").Select(x => IModule.FromString(x)).ToList();

moduleList.Where(x => x.GetType() == typeof(Conjunction)).Cast<Conjunction>().ToList().ForEach(module => {
    module.ConnectionsFrom = moduleList.Where(x => x.ConnectionsTo.Contains(module.Name)).ToDictionary(x => x.Name, x => false);
});

var modules = moduleList.ToDictionary(x => x.Name, x => x);

var presses = 1;
var hitRx = false;
Dictionary<string, long> mod = new Dictionary<string, long>();
while(!hitRx){
    List<Pulse> currentModules = [new ("","broadcaster", false)];
    while(currentModules.Count > 0){
        var nextModules = new List<Pulse>();
        foreach(var module in currentModules ){

            if(modules.ContainsKey(module.To)){
                nextModules.AddRange(modules[module.To].ReceivePulse(module.pulse, module.From));
                if(modules[module.To] is Conjunction con){
                    if(con.AllTrue && !mod.ContainsKey(module.To)){

                        mod.Add(module.To,presses);
                        if(mod.Values.Where(x => x != 1).Count() == 4){
                            hitRx= true;
                        }

                    }
                }
            }
        }
        currentModules = nextModules;
    }
    presses++;
}
Console.WriteLine(JsonSerializer.Serialize(mod));
Console.WriteLine(CalculateLcmList(mod.Values.Where(x => x != 1).ToList()));



long CalculateLcmList(List<long> values)
{
    long lcm = values[0];
    foreach (var val in values.ToArray()[1..])
    {
        lcm = CalculateLcm(lcm, val);
        Console.WriteLine(lcm);
    }
    return lcm;
}

long CalculateLcm(long a, long b)
{
    for (long tempNum = 1; ; tempNum++)
    {
        if (a * tempNum % b == 0)
        {
            return a * tempNum;
        }
    }
}

record struct Pulse(string From, string To, bool pulse);

interface IModule{
    public ModuleType Type { get; }
    public List<string> ConnectionsTo {get; set;}
    public string Name { get; set; }
    public List<Pulse> ReceivePulse(bool pulse, string from);

    public static IModule FromString(string input ){
        var s = input.Split(" -> ", StringSplitOptions.TrimEntries);
        var connectionTo = s[1].Split(",", StringSplitOptions.TrimEntries).ToList();
        var name = s[0];
        if(name == "broadcaster"){
            return new Broadcaster(connectionTo);
        }else{
            var type = GetModuleType(s[0][0]);
            return type switch
            {
                ModuleType.FlipFlop => new FlipFlop(name[1..], connectionTo),
                ModuleType.Conjunction => new Conjunction(name[1..], connectionTo)
            };
        }
    }

    public static ModuleType GetModuleType(char c){
        return c switch{
            '%' => ModuleType.FlipFlop,
            '&' => ModuleType.Conjunction
        };
    }

}

class Broadcaster : IModule
{
    public List<string> ConnectionsTo { get; set; } = new List<string>();
    public string Name { get; set; } = "broadcaster";

    ModuleType IModule.Type => ModuleType.Broadcaster;

    public Broadcaster(List<string> connectionsTo){
        ConnectionsTo = connectionsTo;
    }

    public List<Pulse> ReceivePulse(bool pulse, string from)
    {
        return ConnectionsTo.Select(x => new Pulse(Name, x, false)).ToList();
    }
}

class FlipFlop : IModule
{
    public List<string> ConnectionsTo { get; set; } = new List<string>();
    public string Name { get; set; }
    public bool IsOn { get; set; }

    ModuleType IModule.Type => ModuleType.FlipFlop;

    public FlipFlop(string name, List<string> connectionsTo){
        Name = name;
        ConnectionsTo = connectionsTo;
    }

    public List<Pulse> ReceivePulse(bool pulse, string from)
    {
        if(!pulse){
            IsOn = !IsOn;
            return ConnectionsTo.Select(x => new Pulse(Name, x, IsOn)).ToList();
        }
        return new();
    }
}

class Conjunction : IModule
{
    public List<string> ConnectionsTo { get; set; } = new List<string>();
    public Dictionary<string, bool> ConnectionsFrom { get; set; } = new Dictionary<string, bool>();
    public string Name { get; set; }
    public bool AllTrue { get; set; }

    ModuleType IModule.Type => ModuleType.Conjunction;

    public Conjunction(string name, List<string> connectionsTo){
        Name = name;
        ConnectionsTo = connectionsTo;
    }

    public List<Pulse> ReceivePulse(bool pulse, string from)
    {
        ConnectionsFrom[from] = pulse;
        AllTrue = ConnectionsFrom.Values.All(x => x == true);
        return ConnectionsTo.Select(x => new Pulse(Name, x, !AllTrue)).ToList();
    }
}


enum ModuleType{
    FlipFlop,
    Conjunction,
    Broadcaster
}
