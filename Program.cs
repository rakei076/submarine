using System;

namespace submarine;

public class Program
{
    public const int ROW_SIZE=9;
    public const int COLUMN_SIZE=9;

    public static void Main()
    {
        Board board=new Board(ROW_SIZE,COLUMN_SIZE);
        board.Print();
        board.Input();
        board.PlaceSymbol();
        
        Console.WriteLine("\n更新后的棋盘:");
        board.Print();
    }
}
