using System;

namespace submarine;

/// <summary>
/// Board类 - 潜水艇游戏的核心游戏逻辑类
/// 
/// 【面向对象编程的核心概念】:
/// - 封装(Encapsulation): 将数据和操作数据的方法组织在一起
/// - 单一职责原则: 这个类专门负责游戏棋盘的管理和游戏逻辑
/// 
/// 【internal访问修饰符】:
/// - internal: 只能在同一程序集内访问
/// - 比public限制更严格，适合内部实现
/// - 防止外部程序直接访问内部实现细节
/// 
/// 【类的主要职责】:
/// 1. 管理游戏棋盘状态
/// 2. 处理用户输入
/// 3. 显示游戏界面
/// 4. 控制游戏流程
/// 5. 判断游戏结束条件
/// </summary>
// 3 個の会話
internal class Board
{
    /// <summary>
    /// 游戏棋盘的行数
    /// 
    /// 【属性vs字段】:
    /// - 属性提供对数据的受控访问
    /// - 可以在get/set中添加验证逻辑
    /// - 更好的封装性
    /// </summary>
    // 4 個の会話
    public int RowSize { get; set; }

    /// <summary>
    /// 游戏棋盘的列数
    /// 
    /// 【为什么需要Size属性】:
    /// - 便于动态调整棋盘大小
    /// - 提高代码的可维护性
    /// - 支持不同难度的游戏模式
    /// </summary>
    // 5 個の会話
    public int ColumnSize { get; set; }

    // 【注释掉的代码说明设计演进】:
    // 早期设计使用分离的int变量存储坐标
    // 后来改进为使用Position结构体，更面向对象
    //private int inputRow;
    //private int inputCol;

    /// <summary>
    /// 存储用户最后一次输入的位置
    /// 
    /// 【结构体的优势】:
    /// - 将相关数据(行和列)组织在一起
    /// - 避免数据分离造成的逻辑错误
    /// - 提供更清晰的代码结构
    /// </summary>
    private Position inputPosition;

    /// <summary>
    /// 潜水艇的行坐标 (内部表示，0-based)
    /// 
    /// 【为什么使用private】:
    /// - 隐藏实现细节，外部不能直接访问
    /// - 防止外部代码意外修改潜水艇位置
    /// - 保证游戏的公平性
    /// </summary>
    private int submarineRow;

    /// <summary>
    /// 潜水艇的列坐标 (内部表示，0-based)
    /// 
    /// 【游戏设计考虑】:
    /// - 潜水艇位置应该对玩家保密
    /// - 只有游戏结束时才显示真实位置
    /// </summary>
    private int submarineCol;

    /// <summary>
    /// 单元格状态枚举 - 定义棋盘上每个位置的可能状态
    /// 
    /// 【枚举(Enum)的概念和优势】:
    /// - 枚举是一组命名的整数常量
    /// - 提高代码可读性：用有意义的名称代替数字
    /// - 类型安全：编译器会检查类型匹配
    /// - 便于维护：修改状态名称时有IDE支持
    /// 
    /// 【为什么使用private enum】:
    /// - 这个枚举只在Board类内部使用
    /// - 隐藏实现细节，外部不需要知道内部状态表示
    /// 
    /// 【状态转换逻辑】:
    /// Unselected -> Selected (玩家选择但未击中)
    /// Unselected -> Sunk    (玩家选择且击中潜水艇)
    /// Selected -> Selected  (重复选择已选位置，状态不变)
    /// Sunk -> Sunk         (游戏结束后状态不再改变)
    /// </summary>
    // 7 個の会話
    private enum CellState
    {
        /// <summary>未选择状态 - 棋盘初始状态</summary>
        Unselected,
        /// <summary>已选择状态 - 玩家选择过但未击中潜水艇</summary>
        Selected,
        /// <summary>击沉状态 - 玩家击中潜水艇</summary>
        Sunk,
    }

    /// <summary>
    /// 二维数组存储棋盘上每个位置的状态
    /// 
    /// 【二维数组的概念】:
    /// - 可以理解为"数组的数组"或"表格"
    /// - cells[i,j] 表示第i行第j列的单元格
    /// - 适合表示网格、矩阵等二维数据结构
    /// 
    /// 【内存布局】:
    /// - 在内存中按行优先存储
    /// - 访问效率高，缓存友好
    /// 
    /// 【索引说明】:
    /// - cells[0,0] 对应棋盘A1位置
    /// - cells[0,8] 对应棋盘I1位置  
    /// - cells[8,0] 对应棋盘A9位置
    /// - cells[8,8] 对应棋盘I9位置
    /// </summary>
    private CellState[,] cells;

    /// <summary>
    /// 游戏结束标志
    /// 
    /// 【布尔变量的命名约定】:
    /// - 使用is/has/can等前缀
    /// - 或者使用表示状态的形容词
    /// - 名称应该清楚表达变量的含义
    /// 
    /// 【游戏状态管理】:
    /// - false: 游戏进行中
    /// - true: 游戏已结束(玩家击中潜水艇)
    /// 
    /// 【为什么需要这个标志】:
    /// - 控制游戏主循环的终止条件
    /// - 决定是否显示胜利信息
    /// - 防止游戏结束后继续接受输入
    /// </summary>
    bool isFinished;

    /// <summary>
    /// Board类的构造函数 - 初始化游戏棋盘
    /// 
    /// 【构造函数的职责】:
    /// - 初始化对象的状态
    /// - 分配必要的资源
    /// - 执行初始化逻辑
    /// 
    /// 【初始化步骤】:
    /// 1. 设置棋盘尺寸
    /// 2. 创建并初始化二维数组
    /// 3. 生成潜水艇的随机位置
    /// 
    /// 【参数说明】:
    /// rowSize: 棋盘行数，通常为9
    /// colSize: 棋盘列数，通常为9
    /// </summary>
    /// <param name="rowSize">棋盘行数</param>
    /// <param name="colSize">棋盘列数</param>
    // 1 個の会話
    public Board(int rowSize, int colSize)
    {
        // 【属性初始化】
        // 将传入的参数保存到对象属性中
        // 这样类的其他方法就可以使用这些值
        RowSize = rowSize;
        ColumnSize = colSize;
        
        // 【二维数组的创建】
        // new CellState[rowSize, colSize] 创建一个二维数组
        // 数组的每个元素都会自动初始化为枚举的第一个值(Unselected)
        cells = new CellState[rowSize, colSize];
        
        // 【双重循环初始化数组】
        // 外层循环遍历所有行
        for (int row = 0; row < rowSize; row++)
        {
            // 内层循环遍历当前行的所有列
            for (int col = 0; col < colSize; col++)
            {
                // 显式设置每个位置为未选择状态
                // 虽然默认值已经是Unselected，但显式设置提高代码可读性
                cells[row, col] = CellState.Unselected;
            }
        }
        
        // 【随机数生成和潜水艇位置设定】
        // Random类用于生成伪随机数
        Random random = new Random();
        
        // Next(0, rowSize) 生成 [0, rowSize) 范围内的随机整数
        // 注意：上界是开区间，不包含rowSize本身
        submarineRow = random.Next(0, rowSize);
        submarineCol = random.Next(0, colSize);
        
        // 【调试信息】(在实际部署时可以注释掉)
        // Console.WriteLine($"DEBUG: 潜水艇位置 ({submarineRow+1}, {(char)('A'+submarineCol)})");
    }

    /// <summary>
    /// 显示游戏棋盘的方法
    /// 
    /// 【用户界面设计原则】:
    /// - 清晰的视觉布局
    /// - 一致的符号系统
    /// - 实时反馈游戏状态
    /// 
    /// 【显示内容】:
    /// 1. 列标题 (A-I)
    /// 2. 行标题 (1-9) 和对应的单元格状态
    /// 3. 游戏状态信息（距离或胜利消息）
    /// 
    /// 【字符映射】:
    /// - 空格：未选择的位置
    /// - *：已选择但未击中的位置  
    /// - X：击中潜水艇的位置
    /// </summary>
    public void Print()
    {
                // 【简洁的棋盘显示格式 - 修复对齐】
        // 列标题显示 - 为行号留出空间
        Console.Write(" ");
        for (int col = 0; col < ColumnSize; col++)
        {
            Console.Write($" {(char)('A' + col)}");
        }
        Console.WriteLine();

        // 棋盘内容显示
        for (int row = 0; row < RowSize; row++)
        {
            Console.Write($"{row + 1}");
            for (int col = 0; col < ColumnSize; col++)
            {
                if (cells[row, col] == CellState.Sunk)
                {
                    isFinished = true;
                    Console.Write(" X");
                }
                else if (cells[row, col] == CellState.Selected)
                {
                    Console.Write(" *");
                }
                else
                {
                    Console.Write("  ");
                }
            }
            Console.WriteLine();
        }

        // 【游戏状态反馈】
        if (isFinished == true)
        {
            // 【游戏胜利时的信息显示】
            // +1是因为内部使用0-based索引，显示时转换为1-based
            Console.WriteLine($"潜水艦の位置は {submarineRow + 1} {(char)('A' + submarineCol)}");
            Console.WriteLine("撃沈しました。");
        }
        else
        {
            // 【距离计算和提示】
            // 使用曼哈顿距离算法
            // 曼哈顿距离 = |x1-x2| + |y1-y2|
            // Math.Abs()函数计算绝对值，确保距离为正数
            int dist = Math.Abs(inputPosition.Row - submarineRow) + Math.Abs(inputPosition.Column - submarineCol);
            Console.WriteLine($"入力位置から潜水艦までの距離は {dist}");
        }
    }

    /// <summary>
    /// 处理用户输入的方法
    /// 
    /// 【用户输入处理的挑战】:
    /// - 用户可能输入无效数据
    /// - 需要进行数据验证和转换
    /// - 提供友好的错误提示
    /// 
    /// 【输入流程】:
    /// 1. 提示用户输入行号
    /// 2. 读取并解析行号
    /// 3. 提示用户输入列号
    /// 4. 读取并解析列号
    /// 5. 创建Position对象
    /// 6. 更新棋盘状态
    /// 
    /// 【坐标转换】:
    /// - 用户输入：1-9 (行), A-I (列)
    /// - 内部存储：0-8 (行), 0-8 (列)
    /// </summary>
    // 1 個の会話
    public void Input()
    {
        // 【行号输入处理】
        Console.Write("位置を入力してください (例: 7H): ");
        string? input = Console.ReadLine();
        if(input == null || input.Length !=2)
        {
            Console.WriteLine("入力が無効です。");
            return;
        }
        if (char.IsDigit(input[0]) && char.IsLetter(input[1]))
        {
            // 数字在前，字母在后 (如 "7H")
            int row = input[0] - '0'; // 将字符'7'转换为数字7
            char colChar = char.ToUpper(input[1]); // 转换为大写
            int col = colChar - 'A'; // 将字符'H'转换为数字7
            inputPosition = new Position(row-1, col);
            Console.WriteLine($"入力した位置: {input[0]}{input[1]}");
        }
        else if (char.IsLetter(input[0]) && char.IsDigit(input[1]))
        {
            // 字母在前，数字在后 (如 "H7")
            char colChar = char.ToUpper(input[0]); // 转换为大写
            int col = colChar - 'A'; // 将字符'H'转换为数字7
            int row = input[1] - '0'; // 将字符'7'转换为数字7
            inputPosition = new Position(row-1, col);
            Console.WriteLine($"入力した位置: {input[0]}{input[1]}");
        }
        else
        {
            Console.WriteLine("入力が無効です。");
        }




        // 【游戏逻辑判断】
        // 检查玩家输入位置是否击中潜水艇
        if (inputPosition.Row == submarineRow && inputPosition.Column == submarineCol)
        {
            // 【击中处理】
            // 将对应位置设置为击沉状态
            cells[inputPosition.Row, inputPosition.Column] = CellState.Sunk;
        }
        else
        {
            // 【未击中处理】  
            // 将对应位置设置为已选择状态
            cells[inputPosition.Row, inputPosition.Column] = CellState.Selected;
        }
    }

    /// <summary>
    /// 判断游戏是否结束的方法
    /// 
    /// 【方法设计原则】:
    /// - 单一职责：只负责判断游戏状态
    /// - 简单明了：返回布尔值，易于理解
    /// - 状态封装：外部通过这个方法获取游戏状态
    /// 
    /// 【返回值】:
    /// - true: 游戏已结束（玩家击沉潜水艇）
    /// - false: 游戏继续进行
    /// 
    /// 【使用场景】:
    /// - 游戏主循环的终止条件
    /// - 决定是否继续接受用户输入
    /// </summary>
    /// <returns>如果游戏结束返回true，否则返回false</returns>
    public bool IsFinished()
    {
        // 直接返回内部状态标志
        // 这种设计将内部实现与外部接口分离
        return isFinished;
    }
}