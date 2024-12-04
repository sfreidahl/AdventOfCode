// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var board = new XmasPuzzle(File.ReadAllLines("input.txt"));

Console.WriteLine(board.XmasCount);

public record XmasPuzzle(string[] Board)
{

    public int XmasCount =>
        Board
            .SelectMany((x, row) => x.Select((letter, col) => (Row: row, Col: col, Letter: letter)))
            .Where(x => x.Letter == 'X')
            .Select(x => Xmas(x.Row, x.Col))
            .Sum();

    private int Xmas(int row, int column)
    {
        var result = Mas(row, column, 0, 1);
        result += Mas(row, column, 1, 1);
        result += Mas(row, column, 1, 0);
        result += Mas(row, column, 1, -1);
        result += Mas(row, column, 0, -1);
        result += Mas(row, column, -1, -1);
        result += Mas(row, column, -1, 0);
        return result + Mas(row, column, -1, 1);
    }

    private int Mas(int row, int col, int rowInc, int colInc)
    {
        string mas = "MAS";
        for (int i = 0; i < mas.Length; i++)
        {
            row += rowInc;
            col += colInc;
            if (row == -1 || col == -1 || row == 140 || col == 140)
            {
                return 0;
            }
            var letter = Board[row][col];
            if (letter != mas[i])
                return 0;
        }
        return 1;
    }
}
