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

//         return new(ss[^1].Replace("{", "").Replace("}", "").Split(",").Select(int.Parse).ToArray(), ss[1..^1].Select(Button.FromString).ToList());
//     }

//     private Dictionary<string, long> cache = [];
//     private long _fewestPresses = long.MaxValue;

//     public long FewestPresses()
//     {
//         long presses = 0;
//         long prevStateHits = 0;
//         HashSet<string> currentStates = [JoltageToString(new int[Joltage.Length])];
//         HashSet<string> previousStates = [];
//         while (true)
//         {
//             Console.WriteLine(
//                 $"status: presses: {presses}, states: {currentStates.Count}, previous states: {previousStates.Count}, previous state hits: {prevStateHits}"
//             );
//             if (currentStates.Contains(_joltageAsString))
//             {
//                 return presses;
//             }
//             HashSet<string> nextStates = [];
//             foreach (var state in currentStates)
//             {
//                 var joltage = state.Split(",").Select(int.Parse).ToArray();
//                 foreach (var button in Buttons)
//                 {
//                     var nextJoltage = ClickButton(joltage, button);
//                     var nextState = JoltageToString(nextJoltage);
//                     if (previousStates.Contains(nextState) || Invalid(nextJoltage))
//                     {
//                         prevStateHits++;
//                         continue;
//                     }
//                     nextStates.Add(nextState);
//                 }
//             }
//             foreach (var state in nextStates)
//             {
//                 previousStates.Add(state);
//             }
//             currentStates = nextStates;
//             presses++;
//         }
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

//     private bool Invalid(int[] joltage)
//     {
//         for (int i = 0; i < Joltage.Length; i++)
//         {
//             if (joltage[i] > Joltage[i])
//             {
//                 return true;
//             }
//         }
//         return false;
//     }
// }
