// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var orderDic = File.ReadAllLines("input-order.txt").Select(x => x.Split("|")).GroupBy(x => int.Parse(x[0]), x => int.Parse(x[1])).ToList();
var lines = File.ReadAllLines("input-rows.txt").Select(x => x.Split(",").Select(int.Parse).ToList()).ToList();

var result = lines.Where(x => x.IsSorted(orderDic)).Select(x => x[x.Count / 2]).Sum();

Console.WriteLine(result);

public static class ListExtensions
{
    public static bool IsSorted(this List<int> srcList, List<IGrouping<int, int>> sortRules)
    {
        List<int> list = [.. srcList];
        for (int i = 0; i < list.Count; i++)
        {
            var val = list[i];
            List<int> valuesToBeBefore = sortRules.Find(x => x.Key == val)?.ToList() ?? [];
            if (valuesToBeBefore.Count == 0)
            {
                continue;
            }
            var lowestIndex = list.LowestIndexOf(valuesToBeBefore);
            if (lowestIndex == -1)
            {
                continue;
            }
            if (i < lowestIndex)
            {
                continue;
            }
            return false;
        }

        return true;
    }

    public static int LowestIndexOf(this List<int> srcList, List<int> ints)
    {
        var lowest = ints.Select(x => srcList.IndexOf(x)).Where(x => x != -1);
        if (lowest.Count() == 0)
        {
            return -1;
        }
        return lowest.Min();
    }
}
