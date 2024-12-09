// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllText("input.txt").Select(x => int.Parse(x.ToString())).ToList();

List<Memory> memory = new();

var isFreeSpace = false;
var fileIndex = 0;
foreach (var i in input)
{
    if (!isFreeSpace)
    {
        memory.Add(new FileSpace(fileIndex, i));
        fileIndex++;
    }
    else
    {
        memory.Add(new FreeSpace(i));
    }
    isFreeSpace = !isFreeSpace;
}

// PrintMemory();

var reverseIndex = input.Count - 1;
foreach (var file in memory.Where(x => x is FileSpace).Cast<FileSpace>().Reverse())
{
    var indexOfFile = memory.IndexOf(file);
    var firstSpace = memory.Find(x => x is FreeSpace && x.Size >= file.Size);
    if (firstSpace is null)
    {
        continue;
    }
    var indexOfFreeSpace = memory.IndexOf(firstSpace);
    if (indexOfFreeSpace > indexOfFile)
    {
        continue;
    }
    memory[indexOfFile] = new FreeSpace(file.Size);
    if (firstSpace.Size == file.Size)
    {
        memory[indexOfFreeSpace] = file;
    }
    else
    {
        memory[indexOfFreeSpace] = firstSpace with { Size = firstSpace.Size - file.Size };
        memory.Insert(indexOfFreeSpace, file);
    }
    // PrintMemory();
}

var index = 0;
long result = 0;
foreach (var m in memory)
{
    if (m is FileSpace file)
    {
        for (int i = 0; i < file.Size; i++)
        {
            result += file.Index * index;
            index++;
        }
    }
    if (m is FreeSpace)
    {
        index += m.Size;
    }
}

Console.WriteLine(result);

void PrintMemory()
{
    foreach (var m in memory)
    {
        if (m is FileSpace fileSpace)
        {
            for (int i = 0; i < fileSpace.Size; i++)
            {
                Console.Write(fileSpace.Index);
            }
        }
        if (m is FreeSpace freeSpace)
        {
            for (int i = 0; i < freeSpace.Size; i++)
            {
                Console.Write(".");
            }
        }
    }
    Console.WriteLine();
}

record Memory(int Size);

record FileSpace(int Index, int Size) : Memory(Size);

record FreeSpace(int Size) : Memory(Size);



// Console.WriteLine("big list");
// var multiplier = 0;
// var fileId = 0;
// var index = 0;
// var currentFreeSpace = 0;
// var reverseIndex = input.Count - 1;
// while(true){
//     var isFreeSpace = index % 2 == 1;
//     if(!isFreeSpace){
//         for(int i = 0; i < input[index]; i++){
//             result += multiplier *fileId;
//             multiplier++;
//         }
//         fileId++;
//     }else{
//         if(currentFreeSpace == 0){
//             currentFreeSpace = input[index];
//         }


//     }
//     index++;
// }



// Console.WriteLine(result);
