var input = File.ReadAllLines("input.txt");

var result = 0;
var result2 = 0;

foreach(var assignments in input){
    var groups = assignments.Split(",");
    var ass1 = new Assignment(groups[0]);
    var ass2 = new Assignment(groups[1]);
    var contains = ass1.Contains(ass2) || ass2.Contains(ass1);
    Console.WriteLine(assignments);
    Console.WriteLine(contains);
    result += contains ? 1 : 0;
    result2 += ass1.Overlaps(ass2) ? 1 : 0;
}

Console.WriteLine(result);
Console.WriteLine(result2);

class Assignment{
    public Assignment(string s){
        var ass = s.Split("-");
        start = Convert.ToInt32(ass[0]);
        end = Convert.ToInt32(ass[1]);
    }
    int start;
    int end;
    public bool Contains(Assignment b){
        return (b.start >= start && b.end <= end);
    }
    public bool Overlaps(Assignment b){
       
        return ( b.start <= end && b.end >= start);
    }
}