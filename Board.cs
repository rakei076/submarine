using System;

namespace submarine;

public class Board
{
    public int ROW_SIZE{get;set;}
    public int COLUMN_SIZE{get;set;}
    private int[,] cells;
    private int inputRow;
    private int inputCol;

    public Board(int rowSize,int columnSize)
    {
        ROW_SIZE = rowSize;
        COLUMN_SIZE = columnSize;
        cells=new int[rowSize,columnSize];

        for(int i=0;i<rowSize;i++)
        {
            for(int j=0;j<columnSize;j++)
            {
                cells[i,j]=0;
            }
        }
    }
    
    public void Print()
    {
        Console.Write("   ");
        for(int col=0;col<COLUMN_SIZE;col++)
        {
            Console.Write($"{(char)('A' + col)} ");
        }
        Console.Write(" ");
        Console.WriteLine(); // 换行，让列标题和数据分开显示
        
        for(int i=0;i<ROW_SIZE;i++)
        {
            Console.Write(i+1);
            for(int j=0;j<COLUMN_SIZE;j++)
            {
                if(cells[i,j] == 1){
                    Console.Write(" "+'*');
                }
                Console.Write(" "+"|");
            }
            Console.Write(" "+"|");
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    public void Input()
    {
        Console.Write("縦軸の値を入力してください[1-9]: ");
        int.TryParse(Console.ReadLine(), out inputRow);

        Console.Write("横軸の値を入力してください[A-I]: ");
        char[] ch = Console.ReadLine().ToCharArray();
        inputCol = ch[0] - 'A';
        
        
        cells[inputRow-1,inputCol]=1
    }

}
//dotnet run