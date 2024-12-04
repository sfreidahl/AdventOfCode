var input = File.ReadAllLines("input.txt");

Dictionary<char, int> values = new ();

for(int i = 1; i <= 26; i++){
    values.Add((char)(i+96), i);
    values.Add((char)(i+64), i+26);
    Console.WriteLine((char)(i+96));
    Console.WriteLine((char)(i+64));
}
var result = 0;
var result2 = 0;
var group = 0;
Dictionary<char, int> wat = InitDict();
foreach(string backpack in input){

    var comp1 = backpack.Substring(backpack.Length/2).ToArray().ToHashSet();
    var comp2 = backpack.Substring(0, backpack.Length/2).ToArray().ToHashSet();
    foreach(var c in comp1){
        wat[c]++;
    }
    foreach(var c in comp2){
        wat[c]++;
    }
    // comp1.Where
    var inBoth = comp1.Where(x => comp2.Contains(x));
    result += inBoth.Select(x => values[x]).Sum();

    group++;
    if(group == 3){
        result2 += values[wat.Where(x => x.Value == 3).Single().Key];
        group = 0;
        wat = InitDict();
    }
}

Console.WriteLine(result);
Console.WriteLine(result2);

Dictionary<char, int> InitDict(){
    Dictionary<char, int> dict = new ();
    foreach(var c in values){
        dict[c.Key] = 0;
    }

    return dict;
}