// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var board = new XMasPuzzle(File.ReadAllLines("input.txt"));

Console.WriteLine(board.XmasCount);

public record XMasPuzzle(string[] Board)
{
    public int XmasCount =>
        Board[1..^1]
            .SelectMany((x, row) => x[1..^1].Select((letter, col) => (Row: row+1, Col: col+1, Letter: letter)))
            .Where(x => x.Letter == 'A')
            .Where(x => XMas(x.Row, x.Col))
            .Count();

    private bool XMas(int row, int column)
    {
        return Mas(row, column, 1) && Mas(row, column, -1);
    }

    private bool Mas(int row, int col, int dir)
    {
        string result = "";
        result += Board[row + 1][col + 1 * dir];
        result += Board[row + -1][col + -1 * dir];

        return result == "MS" || result == "SM";
    }
}
