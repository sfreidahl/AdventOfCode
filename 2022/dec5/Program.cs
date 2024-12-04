var input = File.ReadAllLines("input.txt");
var inputTower = File.ReadAllLines("input2.txt").Reverse();
var tower = new string[9];

for(int i = 0; i < tower.Length; i++){
    tower[i] = "";
}

foreach(var row in inputTower){
    int i = 0;
    foreach(var letter in row){
        if(letter != '0'){
            tower[i] = tower[i] + letter;
        }
        i++;
    }
}

foreach (var move in input)
{
    var ins = move.Split(" ");
    var count = Convert.ToInt32(ins[1]);
    var from = Convert.ToInt32(ins[3]) - 1;
    var to = Convert.ToInt32(ins[5]) - 1;

    var toMove = new string(tower[from][^count..].Reverse().ToArray());
    tower[from] = tower[from][..^count];

    tower[to] = tower[to] + toMove;
}

foreach (var column in tower)
{
    Console.Write(column.Last());
}