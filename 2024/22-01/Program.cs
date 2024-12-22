// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var startNumbers = File.ReadAllLines("input.txt").Select(long.Parse);

long result = 0;
foreach (var startNumber in startNumbers)
{
    result += startNumber.NSecretNumber(2000);
}
Console.WriteLine(result);

static class LongExtensions
{
    public static long NSecretNumber(this long secretNumber, long n)
    {
        for (var i = 0; i < n; i++)
        {
            secretNumber = secretNumber.NextSecretNumber();
        }
        return secretNumber;
    }

    public static long NextSecretNumber(this long secretNumber)
    {
        secretNumber = secretNumber.Mix(secretNumber * 64L).Prune();
        secretNumber = secretNumber.Mix(secretNumber / 32L).Prune();
        secretNumber = secretNumber.Mix(secretNumber * 2048L).Prune();
        return secretNumber;
    }

    public static long Mix(this long secretNumber, long number) => secretNumber ^ number;

    public static long Prune(this long secretNumber) => secretNumber % 16777216;
}
