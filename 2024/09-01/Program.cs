// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = File.ReadAllText("input.txt").Select(x => int.Parse(x.ToString())).ToList();

long result = 0;
var index = 0;
var memoryIndex = 0;
var fileIndex = 0;
var reverseIndex = input.Count - 1;
Console.WriteLine(reverseIndex);
if (reverseIndex % 2 == 1)
{
    reverseIndex--;
}

var h = input[reverseIndex];
Console.WriteLine(h);

while (true)
{
    var isFreeSpace = memoryIndex % 2 == 1;
    var val = input[memoryIndex];
    if (!isFreeSpace)
    {
        if (memoryIndex >= reverseIndex)
        {
            for (int j = 0; j < h; j++)
            {
                result += fileIndex * index;
                index++;
            }
            break;
        }
        else
        {
            for (int j = 0; j < val; j++)
            {
                result += fileIndex * index;
                index++;
            }
        }
        fileIndex++;
    }
    if (isFreeSpace)
    {
        var remainingToFill = val;

        for (int j = 0; j < remainingToFill; j++)
        {
            result += reverseIndex / 2 * index;
            h--;
            if (h == 0)
            {
                reverseIndex -= 2;
                h = input[reverseIndex];
            }
            index++;
        }
    }
    memoryIndex++;
}

Console.WriteLine(result);
