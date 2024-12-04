
var input = File.ReadAllLines("input.txt");

var counter = 0;
List<int> all = new List<int>();

foreach(string s in input ){
    if(string.IsNullOrEmpty(s)){
        all.Add(counter);
        counter = 0;
        continue;
    }
    var amount = Convert.ToInt32(s);
    counter += amount;
}

all.Sort();

Console.WriteLine(all.Last());

Console.WriteLine(all.ToArray()[^3..].Sum());