using lzengine;
using System;
using System.Collections.Generic;

namespace lzengine
{
    public class Graph
    {
        public const uint MaxCost = NodeDefine.MaxCost;
        int _rows;
        int _columns;
        Node[] _graph;

        public int Rows => _rows;
        public int Columns => _columns;

        public Node[] Nodes => _graph;

        private Graph(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _graph = new Node[rows * columns];
            Initialize();
        }

        private void Initialize()
        {
            for (int q = 0; q < _columns; q++)
            {
                for (int r = 0; r < _rows; r++)
                {
                    int index = GetIndex(q, r);
                    _graph[index] = new Node(q, r, Direction.None);
                }
            }
        }

        public void ToggleWalkability(int column, int row, bool isWalkable)
        {
            if (!TryGet(column, row, out Node node, out int index))
                return;
            node.isWalkable = isWalkable;
        }
        public void SetCost(int column, int row, uint cost)
        {
            if (!TryGet(column, row, out Node node, out int index))
                return;
            node.cost = cost;
        }

        public void SetFCost(int column, int row, uint fCost)
        {
            if (!TryGet(column, row, out Node node, out int index))
                return;
            node.fCost = fCost;
        }

        public void SetTarget(int targetColumn, int targetRow)
        {
            if (!TryGet(targetColumn, targetRow, out Node target, out int index))
                return;

            for(int i = 0; i < _graph.Length; i++)
            {
                var node = _graph[i];
                node.cost = node.isWalkable ? 10 : MaxCost;
                node.fCost = MaxCost;
            }

            target.cost = 0;
            target.fCost = 0;
            target.direction = Direction.None;

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(target);

            Node[] neighbours = new Node[4];
            while (queue.Count > 0)
            {
                var curNode = queue.Dequeue();
                GetNeighbour(curNode.x, curNode.y, neighbours);
                for(int n = 0; n < neighbours.Length; n++)
                {
                    if (neighbours[n] == null) continue;
                    var nNode = neighbours[n];
                    if (nNode.cost == MaxCost) continue;
                    nNode.cost = CalcCost(nNode, curNode);
                    if(nNode.cost + curNode.fCost < nNode.fCost)
                    {
                        nNode.fCost = nNode.cost + curNode.fCost;
                        queue.Enqueue(nNode);
                    }
                }
            }
        }

        public void GenerateFlowField(int targetColumn, int targetRow)
        {
            if (!TryGet(targetColumn, targetRow, out Node target, out int index))
                return;

            Node[] neighbours = new Node[4];
            for (int i = 0; i < _graph.Length; i++)
            {
                var node = _graph[i];
                GetNeighbour(node.x, node.y, neighbours);
                uint fCost = node.fCost;
                Node tmpNode = null;
                for(int n=0; n < neighbours.Length; n++)
                {
                    if(neighbours[n] == null) continue;
                    var nNode = neighbours[n];
                    if(nNode.fCost < fCost)
                    {
                        tmpNode = nNode;
                        fCost = nNode.fCost;
                        node.direction = new Direction(nNode.x - node.x, nNode.y - node.y);
                    }
                    else if(nNode.fCost == fCost && tmpNode != null)
                    {
                        if(CalcCost(nNode, target) < CalcCost(tmpNode, target))
                        {
                            tmpNode = nNode;
                            fCost = nNode.fCost;
                            node.direction = new Direction(nNode.x - node.x, nNode.y - node.y);
                        }
                    }
                }

                if(node.fCost == MaxCost)
                {
                    node.direction = Direction.None;
                }
            }
        }

        private void GetNeighbour(int column, int row, Node[] neighbours)
        {
            TryGet(column + 1, row, out Node eastNode, out int eastIndex);
            TryGet(column - 1, row, out Node westNode, out int westIndex);
            TryGet(column, row + 1, out Node northNode, out int northIndex);
            TryGet(column, row - 1, out Node southNode, out int southIndex);
            neighbours[0] = eastNode;
            neighbours[1] = westNode;
            neighbours[2] = northNode;
            neighbours[3] = southNode;
        }

        private uint CalcCost(Node node1, Node node2)
        {
            int deltaX = node1.x - node2.x;
            if (deltaX < 0) deltaX = -deltaX;
            int deltaY = node1.y - node2.y;
            if (deltaY < 0) deltaY = -deltaY;
            int delta = deltaX - deltaY;
            if (delta < 0) delta = -delta;
            //每向上、下、左、右方向移动一个节点代价增加10
            //每斜向移动一个节点代价增加14（勾股定理，精确来说是近似14.14~）
            return (uint)(14 * (deltaX > deltaY ? deltaY : deltaX) + 10 * delta);
        }

        public bool TryGet(int column, int row, out Node node, out int index)
        {
            index = GetIndex(column, row);
            if (column < 0 || row < 0 || column > Columns - 1 || row > Rows - 1 || index < 0 || index > _graph.Length - 1)
            {
                node = null;
                return false;
            }

            node = _graph[index];
            return true;
        }
        public bool TryGet(int column, int row, out Node node) => TryGet(column, row, out node, out _);
        public int GetIndex(int column, int row) => row * _columns + column;
        public void GetColumnRow(int index, out int column, out int row)
        {
            if (index < 0 || index > _graph.Length - 1)
                throw new Exception("Index " + index + " is out of range for 0.." + _graph.Length);
            row = index / _columns;
            column = index % _columns;
        }

        public static bool TryCreate(int rows, int columns, out Graph graph)
        {
            graph = null;
            if (rows < 1 || columns < 1)
                return false;

            graph = new Graph(rows, columns);
            return true;
        }

        public void CloneFrom(Graph graph)
        {
            for(int i = 0; i < graph._graph.Length; i++)
            {
                var srcNode = graph._graph[i];
                var tarNode = _graph[i];
#if UNITY_EDITOR
                if(srcNode.x != tarNode.x || srcNode.y != tarNode.y)
                {
                    LZDebug.LogError("Graph is Diff Err!!!!!!!!!!!!");
                }
#endif
                tarNode.fCost = srcNode.fCost;
                tarNode.cost = srcNode.cost;
                tarNode.direction = srcNode.direction;
            }
        }

    }
}