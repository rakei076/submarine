using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace submarine
{
    /// <summary>
    /// Position结构体 - 表示游戏棋盘上的位置坐标
    /// 
    /// 【结构体(struct)的特点】:
    /// - 值类型：直接存储数据，不是引用
    /// - 内存效率高：直接在栈上分配内存
    /// - 适合表示简单的数据组合，如坐标、颜色等
    /// - 默认情况下不能为null
    /// 
    /// 【为什么使用internal】:
    /// - internal：只能在同一个程序集内部访问
    /// - 比public限制更严格，比private限制更宽松
    /// - 适合内部实现的数据结构
    /// </summary>
    // 4 個の会話
    internal struct Position
    {
        /// <summary>
        /// 行号属性 - 表示棋盘上的纵坐标
        /// 
        /// 【属性(Property)的概念】:
        /// - 提供对私有字段的受控访问
        /// - { get; set; } 是自动属性的语法
        /// - 编译器会自动生成私有字段和getter/setter方法
        /// 
        /// 【坐标系统】:
        /// - Row = 0 对应棋盘的第1行
        /// - Row = 1 对应棋盘的第2行
        /// - 以此类推...
        /// - 使用0-based索引便于数组操作
        /// </summary>
        // 6 個の会話
        public int Row { get; set; }

        /// <summary>
        /// 列号属性 - 表示棋盘上的横坐标
        /// 
        /// 【坐标系统】:
        /// - Column = 0 对应棋盘的A列
        /// - Column = 1 对应棋盘的B列
        /// - 以此类推...
        /// - 将字母转换为数字便于数组索引
        /// </summary>
        // 6 個の会話
        public int Column { get; set; }

        /// <summary>
        /// Position结构体的构造函数
        /// 
        /// 【构造函数的作用】:
        /// - 用于创建和初始化结构体实例
        /// - 接收行号和列号参数
        /// - 将参数值赋给属性
        /// 
        /// 【使用示例】:
        /// Position pos = new Position(3, 5);  // 创建第4行第F列的位置
        /// </summary>
        /// <param name="row">行号 (0-8，对应棋盘1-9行)</param>
        /// <param name="col">列号 (0-8，对应棋盘A-I列)</param>
        // 1 個の会話
        public Position(int row, int col)
        {
            // 将传入的参数赋值给属性
            // 这样就完成了Position对象的初始化
            Row = row;
            Column = col;
        }

        /// <summary>
        /// 判断两个Position是否相等的方法
        /// 
        /// 【方法的作用】:
        /// - 比较当前Position与另一个Position是否指向同一位置
        /// - 用于判断玩家输入位置是否击中潜水艇
        /// 
        /// 【逻辑原理】:
        /// - 使用逻辑AND运算符(&&)
        /// - 只有当行号AND列号都相等时，才返回true
        /// - 任何一个不相等，整个表达式返回false
        /// 
        /// 【Boolean逻辑】:
        /// - true && true = true   (两个位置完全相同)
        /// - true && false = false (列号不同)
        /// - false && true = false (行号不同)
        /// - false && false = false (行号列号都不同)
        /// </summary>
        /// <param name="other">要比较的另一个Position对象</param>
        /// <returns>如果两个位置相同返回true，否则返回false</returns>
        // 0 個の会話
        public bool Equals(Position other)
        {
            // 使用短路求值：如果Row不相等，不会再检查Column
            // 这样可以提高程序执行效率
            return Row == other.Row && Column == other.Column;
        }

        /// <summary>
        /// 验证当前位置是否在棋盘有效范围内
        /// 
        /// 【输入验证的重要性】:
        /// - 防止数组越界错误(IndexOutOfRangeException)
        /// - 确保游戏逻辑的正确性
        /// - 提供更好的用户体验
        /// 
        /// 【有效范围】:
        /// - Row: 0 <= Row < 9   (对应用户输入的1-9)
        /// - Column: 0 <= Column < 9  (对应用户输入的A-I)
        /// 
        /// 【边界检查逻辑】:
        /// - 使用逻辑AND连接四个条件
        /// - 所有条件都为true时，位置才有效
        /// - 任何一个条件为false，位置就无效
        /// 
        /// 【为什么要比较常量】:
        /// - Program.ROW_SIZE和COLUMN_SIZE是游戏棋盘的尺寸定义
        /// - 使用常量而不是硬编码数字，便于维护和修改
        /// </summary>
        /// <returns>如果位置有效返回true，否则返回false</returns>
        // 0 個の会話
        public bool IsValid()
        {
            // 检查行号是否在有效范围内：0 <= Row < ROW_SIZE
            // 检查列号是否在有效范围内：0 <= Column < COLUMN_SIZE
            // 四个条件用&&连接，必须全部为true
            return 0 <= Row && Row < Program.ROW_SIZE
                && 0 <= Column && Column < Program.COLUMN_SIZE;
        }
    }
} 