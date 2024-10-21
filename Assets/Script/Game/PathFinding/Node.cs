using System;
namespace lzengine
{
    public class NodeDefine
    {
        public const uint MaxCost = 99999; 
    }

    public readonly struct Direction: IEquatable<Direction>
    {
        public static readonly Direction None = new Direction(0, 0);
        public static readonly Direction Up = new Direction(0, -1);
        public static readonly Direction Down = new Direction(0, 1);
        public static readonly Direction Left = new Direction(-1, 0);
        public static readonly Direction Right = new Direction(1, 0);
        private readonly int _x;
        private readonly int _y;

        public Direction(in int x, in int y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            if(this.Equals(Direction.Up))
            {
                return "↑";
            }
            else if(this.Equals(Direction.Down))
            {
                return "↓";
            }
            else if(this.Equals(Direction.Left))
            {
                return "←";
            }
            else if(this.Equals(Direction.Right))
            {
                return "→";
            }

            return "█";
        }

        public override bool Equals(object obj)
        {
            if (obj is Direction d)
                return d.Equals(this);
            return false;
        }

        public bool Equals(Direction other)
        {
            return other._x == _x && other._y == _y;
        }
    }

    public class Node : IEquatable<Node>
    {
        public int x;
        public int y;
        public bool isWalkable;
        public uint cost;
        private uint _fCost;
        public uint fCost
        {
            get
            {
                return _fCost;
            }
            set
            {
                _fCost = value;
                if(_fCost == NodeDefine.MaxCost)
                {
                    isWalkable = false;
                }
                else
                {
                    isWalkable = true;
                }
            }
        }
        public Direction direction;

        public Node(in int x, in int y, Direction d, in bool isWalkable = true, in uint cost = 0)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
            this.cost = cost;
            this.direction = d;
        }

        public bool Equals(Node other) => x == other.x && y == other.y;

        public override int GetHashCode() => x + 100000 * y;

        public override bool Equals(object obj)
        {
            if (obj is Node node)
                return node.Equals(this);
            return false;
        }
        public override string ToString()
        {
            string s = "[" + x + "," + y + "]";
            s += " IsWalkable: " + isWalkable;
            s += " Cost: " + cost;
            s += " Direction: " + direction;
            return s;
        }

        public string GetCostStr()
        {
            if(fCost == NodeDefine.MaxCost)
            {
                return "";
            }

            return fCost.ToString();
        }

        public uint Distance(in Node node)
        {
            var dx = x < node.x ? node.x - x : x - node.x;
            var dy = y < node.y ? node.y - y : y - node.y;
            return (uint)(dx + dy);
        }
    }
}