var input = File.ReadAllLines("input.txt");

var currentDir = new Stack<string>();
var dirSizes =  new Dictionary<string, int>();

foreach(var command in input){
    var parameters = command.Split(" ");
    if(parameters[0] == "$"){
        switch( parameters[1]){
            case "cd":
                switch (parameters[2])
                {
                    case "/":
                        currentDir = new Stack<string>();
                        dirSizes[""] = 0;
                        break;
                    case "..":
                        currentDir.Pop();
                        break;
                    default:
                        currentDir.Push(parameters[2]);
                        var dir = string.Join("/", currentDir.ToArray().Reverse());
                        Console.WriteLine("nav: " + dir);
                        dirSizes.Add(dir, 0);
                        break;
                }
                break;
            case "ls":
                break;
        }

    }else{
        if(int.TryParse( parameters[0], out int size)){
            var curStack = new Stack<string>(currentDir.Reverse());
            do{
                var d = string.Join("/",curStack.ToArray().Reverse());
                Console.WriteLine("Size: " + d);
                dirSizes[d] += size;
                
            }while(curStack.TryPop(out _));

        }
    }
}

var diskSize = 70000000;
var required = 30000000;
var total = dirSizes[""];
var unused = (diskSize - total);

var toDelete = required - unused;

var dirsBelow = dirSizes.Where( x => x.Value <= 100000).Select(x => x.Value);
var totalDirsBelow =  dirsBelow.Sum();
Console.WriteLine(totalDirsBelow);

//dirSizes.Where( x => x.Value <= 100000).Select(x => x.Value);

Console.WriteLine(dirSizes.Where(x => x.Value > toDelete).Select(x => x.Value).Min());