using System;

namespace submarine;

internal class Board
{
    public int RowSize { get; set; }
    public int ColumnSize { get; set; }

    private Position inputPosition;

    private BaseSubmarine submarine;

    private enum CellState
    {
        Unselected,
        Selected,
        Sunk,
    }

    private CellState[,] cells;

    bool isFinished;
    public Board(int rowSize, int colSize)
    {
        RowSize = rowSize;
        ColumnSize = colSize;
        cells = new CellState[rowSize, colSize];
        for (int row = 0; row < rowSize; row++)
        {
            for (int col = 0; col < colSize; col++)
            {
                cells[row, col] = CellState.Unselected;
            }
        }
        
        Random random = new Random();
        int submarineRow = random.Next(0, rowSize);
        int submarineCol = random.Next(0, colSize);
        Position submarinePosition = new Position(submarineRow, submarineCol);
        submarine = new SimpleRowSubmarine(submarinePosition);
    }

    public void Print()
    {
        Console.Write(" ");
        for (int col = 0; col < ColumnSize; col++)
        {
            Console.Write($" {(char)('A' + col)}");
        }
        Console.WriteLine();

        for (int row = 0; row < RowSize; row++)
        {
            Console.Write($"{row + 1}");
            for (int col = 0; col < ColumnSize; col++)
            {
                if (cells[row, col] == CellState.Sunk)
                {
                    isFinished = true;
                    Console.Write("|X");
                }
                else if (cells[row, col] == CellState.Selected)
                {
                    Console.Write("|*");
                }
                else
                {
                    Console.Write("| ");
                }
            }
            Console.WriteLine("|");
        }

        if (isFinished == true)
        {
            Console.WriteLine($"潜水艦の位置は {submarine.Position.Row + 1} {(char)('A' + submarine.Position.Column)}");
            Console.WriteLine("撃沈しました。");
        }
        else
        {
            int dist = Math.Abs(inputPosition.Row - submarine.Position.Row) + Math.Abs(inputPosition.Column - submarine.Position.Column);
            Console.WriteLine($"入力位置から潜水艦までの距離は {dist}");
        }
    }

    public void Input()
    {
        Console.Write("位置を入力してください (例: 7H): ");
        string? input = Console.ReadLine();
        if(input == null || input.Length !=2)
        {
            Console.WriteLine("入力が無効です。");
            return;
        }
        if (char.IsDigit(input[0]) && char.IsLetter(input[1]))
        {
            int row = input[0] - '0';
            char colChar = char.ToUpper(input[1]);
            int col = colChar - 'A';
            inputPosition = new Position(row-1, col);
            Console.WriteLine($"入力した位置: {input[0]}{input[1]}");
        }
        else if (char.IsLetter(input[0]) && char.IsDigit(input[1]))
        {
            char colChar = char.ToUpper(input[0]);
            int col = colChar - 'A';
            int row = input[1] - '0';
            inputPosition = new Position(row-1, col);
            Console.WriteLine($"入力した位置: {input[0]}{input[1]}");
        }
        else
        {
            Console.WriteLine("入力が無効です。");
        }

        if (inputPosition.Row == submarine.Position.Row && inputPosition.Column == submarine.Position.Column)
        {
            cells[inputPosition.Row, inputPosition.Column] = CellState.Sunk;
        }
        else
        {
            cells[inputPosition.Row, inputPosition.Column] = CellState.Selected;
        }
    }

    public void Update()
    {
        submarine.Move();
    }

    public bool IsFinished()
    {
        return isFinished;
    }
}