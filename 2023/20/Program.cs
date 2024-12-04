using System.Text.Json;

var moduleList = File.ReadAllLines("input.txt").Select(x => IModule.FromString(x)).ToList();

moduleList.Where(x => x.GetType() == typeof(Conjunction)).Cast<Conjunction>().ToList().ForEach(module => {
    module.ConnectionsFrom = moduleList.Where(x => x.ConnectionsTo.Contains(module.Name)).ToDictionary(x => x.Name, x => false);
});

var modules = moduleList.ToDictionary(x => x.Name, x => x);

int highPulses = 0;
int lowPulses = 0;
// for(int i = 0; i < 1000; i++){
lowPulses+=1;
var presses = 1;
while(true){
    List<Pulse> currentModules = [new ("","broadcaster", false)];//[modules.Find(x => x.Name == "broadcaster") ?? throw new Exception()];
    while(currentModules.Count > 0){
        var nextModules = new List<Pulse>();
        foreach(var module in currentModules ){

            if(modules.ContainsKey(module.To)){
                nextModules.AddRange(modules[module.To].ReceivePulse(module.pulse, module.From));
            }
        }
        // Console.WriteLine(JsonSerializer.Serialize(nextModules));
        currentModules = nextModules;
        highPulses += nextModules.Count(x => x.pulse );
        lowPulses += nextModules.Count(x => !x.pulse );
    }
}
// }
Console.WriteLine(highPulses);
Console.WriteLine(lowPulses);
Console.WriteLine(highPulses*lowPulses);



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

    ModuleType IModule.Type => ModuleType.Conjunction;

    public Conjunction(string name, List<string> connectionsTo){
        Name = name;
        ConnectionsTo = connectionsTo;
    }

    public List<Pulse> ReceivePulse(bool pulse, string from)
    {
        ConnectionsFrom[from] = pulse;
        return ConnectionsTo.Select(x => new Pulse(Name, x, !ConnectionsFrom.Values.All(x => x == true))).ToList();
    }
}


enum ModuleType{
    FlipFlop,
    Conjunction,
    Broadcaster
}
