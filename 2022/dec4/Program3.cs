// var input = File.ReadAllLines("input.txt");

// var result = 0;

// List<Assignment> assignments = new List<Assignment>();

// foreach(var ass in input){
//     var groups = ass.Split(",");
//     var ass1 = new Assignment(groups[0]);
//     var ass2 = new Assignment(groups[1]);
//     assignments.Add(ass1);
//     assignments.Add(ass2);
// }

// for(int i = 0; i < assignments.Count; i++){
//     var ass1 = assignments[i];
//     for(int j = i; j < assignments.Count; j++){
//         var ass2 = assignments[j];
//         result += (ass1.Contains(ass2) || ass2.Contains(ass1)) ? 1 : 0;
//     }
// }

// Console.WriteLine(result);

// class Assignment{
//     public Assignment(string s){
//         var ass = s.Split("-");
//         start = Convert.ToInt32(ass[0]);
//         end = Convert.ToInt32(ass[1]);
//     }
//     int start;
//     int end;
//     public bool Contains(Assignment b){
//         return (b.start >= start && b.end <= end);
//     }
// }