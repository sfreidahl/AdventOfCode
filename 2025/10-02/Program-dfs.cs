// // See https://aka.ms/new-console-template for more information
// using System.Text.Json;

// Console.WriteLine("Hello, World!");

// var machines = File.ReadAllLines("input.txt").Select(Machine.FromString);

// var result = machines.Select(x => x.FewestPresses()).Sum();

// Console.WriteLine(result);

// record Button(int[] Connections)
// {
//     public static Button FromString(string s)
//     {
//         var ss = s.Replace("(", "").Replace(")", "");
//         return new(ss.Split(",").Select(int.Parse).ToArray());
//     }
// }

// record Machine(int[] Joltage, List<Button> Buttons)
// {
//     private string _joltageAsString = JoltageToString(Joltage);

//     public static Machine FromString(string s)
//     {
//         var ss = s.Split(" ");

//         return new(
//             ss[^1].Replace("{", "").Replace("}", "").Split(",").Select(int.Parse).ToArray(),
//             ss[1..^1].Select(Button.FromString).OrderBy(x => x.Connections.Count()).ToList()
//         );
//     }

//     private Dictionary<string, long> cache = [];
//     private long _fewestPresses = long.MaxValue;

//     public long FewestPresses()
//     {
//         Console.WriteLine();
//         return FewestPresses(new int[Joltage.Length], 0);
//     }

//     private long FewestPresses(int[] currentJoltage, long presses)
//     {
//         var joltageString = JoltageToString(currentJoltage);
//         if (
//             cache.TryGetValue(joltageString, out var cachedPresses) && (cachedPresses <= presses || cachedPresses == long.MaxValue)
//             || presses >= _fewestPresses
//         )
//         {
//             // Console.WriteLine("hit cache");
//             return cachedPresses;
//         }
//         // Console.WriteLine($"cacheSize: {cache.Count}");
//         // Console.WriteLine(joltageString);
//         if (joltageString == _joltageAsString)
//         {
//             Console.WriteLine($"Hit {presses}");
//             _fewestPresses = presses;
//             cache[joltageString] = presses;
//             return presses;
//         }
//         for (int i = 0; i < Joltage.Length; i++)
//         {
//             if (currentJoltage[i] > Joltage[i])
//             {
//                 cache[joltageString] = long.MaxValue;
//                 return long.MaxValue;
//             }
//         }

//         long result = long.MaxValue;
//         foreach (var button in Buttons)
//         {
//             var newLights = ClickButton(currentJoltage, button);
//             var r = FewestPresses(newLights, presses + 1);
//             result = Math.Min(result, r);
//         }
//         cache[joltageString] = result;
//         return result;
//     }

//     private static string JoltageToString(int[] joltage) => string.Join(",", joltage.Select(x => x.ToString()));

//     private static int[] ClickButton(int[] joltage, Button button)
//     {
//         int[] newJoltage = [.. joltage];
//         foreach (var connection in button.Connections)
//         {
//             newJoltage[connection] += 1;
//         }
//         return newJoltage;
//     }
// }
