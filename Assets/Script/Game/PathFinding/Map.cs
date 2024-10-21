using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace lzengine
{
    public class Map
    {
        private int _rows;
        private int _columns;
        private Vector2Int _target;
        private Graph _graph;

        public Graph Graph => _graph;
        public int Rows => _rows;
        public Vector2Int Target => _target;
        public int Columns => _columns;

        void Start()
        {
            _graph = null;
        }

        public void GenerateMap(Vector2Int mapSize, Vector2Int target)
        {
            _columns = mapSize.x;
            _rows = mapSize.y;
            _target = target;
            Graph.TryCreate(_rows, _columns, out _graph);
            _graph.SetTarget(_target.x, _target.y);
            _graph.GenerateFlowField(_target.x, _target.y);
        }

        public void SetTarget(Vector2Int target)
        {
            _target = target;
            if (_graph != null)
            {
                _graph.SetTarget(_target.x, _target.y);
                _graph.GenerateFlowField(_target.x, _target.y);
            }
            else
            {
                LZDebug.LogError("Please GenerateMap First !!!!");
            }
        }

        public void SetWalkable(Vector2Int pos, int sizeX, int sizeY, bool isWalkable)
        {
            for(int x = 0; x < sizeX; x++)
            {
                for(int y = 0; y < sizeY; y++)
                {
                    int tmpx = pos.x + x;
                    int tmpy = pos.y + y;
                    int cellindex = _graph.GetIndex(tmpx, tmpy);
                    _graph.ToggleWalkability(tmpx, tmpy, isWalkable);
                }
            }

            _graph.SetTarget(_target.x, _target.y);
            _graph.GenerateFlowField(_target.x, _target.y);
        }

        public Vector2Int GetNextNode(int col, int row)
        {
            _graph.TryGet(col, row, out Node node);
            if (node == null)
            {
                return new Vector2Int(-1, -1);
            }
            int offx = 0;
            int offy = 0;
            bool hasNext = false;
            var dir = node.direction;
            if (dir.Equals(Direction.Down))
            {
                offy = 1;
                hasNext = true;
            }
            else if (dir.Equals(Direction.Up))
            {
                offy = -1;
                hasNext = true;
            }
            else if (dir.Equals(Direction.Right))
            {
                offx = 1;
                hasNext = true;
            }
            else if (dir.Equals(Direction.Left))
            {
                offx = -1;
                hasNext = true;
            }

            if (!hasNext)
            {
                return new Vector2Int(-1, -1);
            }

            return new Vector2Int(col + offx, row + offy);
        }

        public bool IsWalkable(int col, int row)
        {
            _graph.TryGet(col, row, out Node node);
            if (node == null)
            {
                return false;
            }

            if (node.fCost == NodeDefine.MaxCost)
            {
                return false;
            }

            return node.isWalkable;
        }

        public bool IsTargetWalkable()
        {
            _graph.TryGet(_target.x, _target.y, out Node node);
            if(node == null || !node.isWalkable)
            {
                return false;
            }
            return true;
        }

        public bool IsPlacable(Vector2Int pos, int sizeX, int sizeY)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    _graph.TryGet(pos.x + x, pos.y + y, out Node node);
                    if (node == null || !node.isWalkable)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<Node> tmpList = new List<Node>();
        public List<Node> GetNearWalkableNodes(int col, int row)
        {
            tmpList.Clear();
            Node node = null;
            for (int c = -2; c <= 2; c++)
            {
                for (int r = -2; r <= 2; r++)
                {
                    _graph.TryGet(col + c, row + r, out node);
                    if (node != null && node.isWalkable)
                    {
                        tmpList.Add(node);
                    }
                }
            }
            return tmpList;
        }

        public void CloneFrom(Map _map)
        {
            _columns = _map._columns;
            _rows = _map._rows;
            _target = _map._target;
            if(_graph == null || _graph.Columns != _columns || _graph.Rows != _rows)
            {
                Graph.TryCreate(_rows, _columns, out _graph);
            }
            _graph.CloneFrom(_map.Graph);
        }
    }
}