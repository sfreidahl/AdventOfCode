// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

List<Stone> stones = File.ReadAllText("input.txt").Split(" ").Select(long.Parse).Select(x => new Stone(x)).ToList();

var sw = new Stopwatch();
sw.Start();
var result = stones.Select(x => x.Blink(0)).Sum();
sw.Stop();

Console.WriteLine($"milliseconds elapsed: {sw.ElapsedMilliseconds}");

Console.WriteLine(result);

record struct Memo(long Blink, long Number);

record Stone(long Number){
    const int _maxBlink = 75;
    private static Dictionary<Memo, long> _memo = [];
    public Stone(string number) : this(long.Parse(number)){

    }

    public long Blink(int blinkCount){
        var newBLinkCount = blinkCount += 1;
        if(blinkCount > _maxBlink){
            return 1;
        }
        var memo = new Memo(blinkCount, Number);
        if(_memo.TryGetValue(memo, out var r)){
            return r;
        }
        long result = Next(newBLinkCount);
        _memo.Add(memo, result);
        return result;
    }

    private long Next(int blinkCount){
        if(Number == 0){
            return new Stone(1).Blink(blinkCount);
        }
        var stringNumber = Number.ToString();
        if(stringNumber.Length % 2 == 0){

            var midPoint = stringNumber.Length / 2;
            return new Stone(stringNumber[..midPoint]).Blink(blinkCount) + new Stone(stringNumber[midPoint..]).Blink(blinkCount);

        }
        return new Stone(Number * 2024).Blink(blinkCount);

    }
}
