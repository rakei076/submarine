using System;

namespace submarine;

public class Board
{
    public int ROW_SIZE{get;set;}
    public int COLUMN_SIZE{get;set;}
    private int[,] cells;
    
    private int inputRow;
    private int inputCol;

    private int submarineRow;
    private int submarineCol;

    enum CellState{
        Unselected,
        Selected,
        Surk,
    }

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
        Console.Write(" ");
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
                if(cells[i,j] == CellState.Surk){
                    flag=true;
                    Console.Write("X"+"|");
                }else if(cells[i,j] == CellState.Selected){
                    Console.Write("*"+"|");
                }else{
                    Console.Write(" "+"|");
                }
                Console.Write("*"+"|");
                }else{
                    Console.Write(" "+"|");
                }
            }
            
            Console.WriteLine();

        }
        Console.WriteLine();
        if(flag==true){
            Console.WriteLine($"潜水艦の位置は,{submarineRow + 1} {(char)('A'+ submar ineCol)}");
            Console.WriteLine("潜水艦を撃沈しました！");
        }else{
            int dist=Math.Abs(inputRow-submarineRow)+Math.Abs(inputCol-submarineCol);
            Console.WriteLine($"入力した位置から潜水艦までの距離は{dist}");
        }
    }
    public void Input()
    {
        Console.Write("縦軸の値を入力してください[1-9]: ");
        int.TryParse(Console.ReadLine(), out inputRow);

        Console.Write("横軸の値を入力してください[A-I]: ");
        char[] ch = Console.ReadLine().ToCharArray();
        inputCol = ch[0] - 'A';
        
        Console.WriteLine($"入力した位置{strRow}{serCol}");
        
        if (inputRow==submarineRow && inputCol==submarineCol){
            cells[inputRow-1,inputCol]=CellState.Surk;
        }else{
            cells[inputRow-1,inputCol]=CellState.Selected;
        }
    }

}
//dotnet run