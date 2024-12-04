
var input = File.ReadAllLines("input.txt");

var x = 1;
var cycle = 1;
var result = 0;
foreach(var line in input){
    var s = line.Split(" ");
    var instruction = s[0];
    if(instruction == "noop"){
        IncreaseCycle();
        continue;
    }
    var amount = Convert.ToInt32(s[1]);
    IncreaseCycle();
    IncreaseRegister(amount);
}

Console.WriteLine(result);

void IncreaseCycle(){
    DrawPixel();
    if((cycle - 20) % 40 == 0){
        // Console.WriteLine(cycle);
        // Console.WriteLine(x*cycle);
        result += cycle * x;
    }
    cycle++;
}

void IncreaseRegister(int val){
    
    IncreaseCycle();
    x += val;
}

void DrawPixel(){
    Console.ResetColor();
    var pixel = (cycle - 1) % 40;
    // Console.WriteLine(pixel);
    var d = '.';
    if(pixel >= x-1 && pixel <= x+1){
        Console.ForegroundColor = ConsoleColor.DarkRed;
        d = '#';
    }
    Console.Write(d);
    Thread.Sleep(50);
    if(pixel == 39){

        Console.WriteLine();
    }
}