using System;

namespace submarine
{
    internal abstract class BaseSubmarine
    {
        public Position Position { get; set; }

        public BaseSubmarine(Position position)
        {
            Position = position;
        }

        public abstract void Move();
    }

    internal class SimpleRowSubmarine : BaseSubmarine
    {
        public SimpleRowSubmarine(Position position) : base(position)
        {
        }

        public override void Move()
        {
            if (Position.Row < Program.ROW_SIZE - 1)
            {
                Position = new Position(Position.Row + 1, Position.Column);
            }
            else
            {
                Position = new Position(0, Position.Column);
            }
        }
    }

    internal class RandomSubmarine : BaseSubmarine
    {
        private Random random = new Random();

        public RandomSubmarine(Position position) : base(position)
        {
        }

        public override void Move()
        {
            int direction = random.Next(4);
            Position newPosition;

            switch (direction)
            {
                case 0: // 上
                    newPosition = new Position(Position.Row - 1, Position.Column);
                    break;
                case 1: // 下
                    newPosition = new Position(Position.Row + 1, Position.Column);
                    break;
                case 2: // 左
                    newPosition = new Position(Position.Row, Position.Column - 1);
                    break;
                case 3: // 右
                    newPosition = new Position(Position.Row, Position.Column + 1);
                    break;
                default:
                    newPosition = Position;
                    break;
            }

            if (newPosition.IsValid())
            {
                Position = newPosition;
            }
        }
    }
}
