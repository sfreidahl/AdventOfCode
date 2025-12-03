var result = File.ReadAllLines("input.txt").Select(x => x.Select(c => c - 48).ToList()).Select(x => FindJoltage(x)).Sum();

Console.WriteLine(result);

static long FindJoltage(List<int> batteries, int batteryCount = 12)
{
    if (batteryCount == 0)
    {
        return 0;
    }
    var maxLeftIndex = batteryCount - 1;
    var maxLeft = batteries[..^maxLeftIndex].Max();
    var maxIndex = batteries.IndexOf(maxLeft);
    var maxRight = FindJoltage(batteries[(maxIndex + 1)..], maxLeftIndex);

    var joltage = maxLeft * (long)Math.Pow(10, maxLeftIndex) + maxRight;

    return joltage;
}
