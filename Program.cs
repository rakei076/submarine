using System;
using submarine;

Board board = new Board(Program.ROW_SIZE, Program.COLUMN_SIZE);

while (true)
{
    board.Update();
    board.Input();
    board.Print();
    
    if (board.IsFinished())
    {
        break;
    }
}

partial class Program
{
    public const int ROW_SIZE = 9;
    public const int COLUMN_SIZE = 9;
}