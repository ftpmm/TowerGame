using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Map : MonoBehaviour
{
    [SerializeField, Range(1, 50)]
    private int _rows;
    [SerializeField, Range(1, 50)]
    private int _columns;
    [SerializeField, Range(0, 50)]
    private int _obstaclesNum;
    [SerializeField]
    private Vector2Int target;

    [SerializeField]
    private bool _addObstacle = false;


    Graph _graph;
    HashSet<Vector2Int> _obstacles = new();

    public Graph Graph => _graph;
    public int Rows => _rows;
    public int Columns => _columns;
    public HashSet<Vector2Int> Obstacles => _obstacles;

    private bool _isInit = false;

    public float offX = 0;
    public float offY = 0;
    public float cellSizeX = 50;
    public float cellSizeY = 50;

    void Start()
    {
        _graph = null;
    }

    public void InitMap(int rows, int cols)
    {
        _rows = rows;
        _columns = cols;
        _obstaclesNum = 20;
        Graph.TryCreate(_rows, _columns, out _graph);
        GenerateFlowField();
        _isInit = true;
    }

    public void SetTarget(int targetX, int targetY)
    {
        target = new Vector2Int(targetX, targetY);
    }

    private void GenerateFlowField()
    {
        GenerateObstacles(_obstaclesNum);

        _graph.SetTarget(target.x, target.y);
        _graph.GenerateFlowField(target.x, target.y);
    }

    public void GenerateObstacles(int total)
    {
        int leftObjNum = total - _obstacles.Count;
        for (int i = 0; i < leftObjNum; i++)
        {
            int x = UnityEngine.Random.Range(0, _columns);
            int y = UnityEngine.Random.Range(0, _rows);
            if (x == target.x && y == target.y)
            {
                i -= 1;
                continue;
            }

            _graph.ToggleWalkability(x, y, false);
            if (_obstacles.Contains(new(x, y)))
                i -= 1;
            else
                _obstacles.Add(new(x, y));
        }
    }

    private void Update()
    {
        if (_addObstacle)
        {
            _addObstacle = false;
            _obstaclesNum++;
            GenerateFlowField();
        }
    }

    public Vector2Int GetNextNode(int col, int row)
    {
        _graph.TryGet(col, row, out Node node);
        if(node == null)
        {
            return new Vector2Int(-1, -1);
        }
        int offx = 0;
        int offy = 0;
        bool hasNext = false;
        var dir = node.direction;
        if(dir.Equals(Direction.Down))
        {
            offy = 1;
            hasNext = true;
        }
        else if(dir.Equals(Direction.Up))
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
       
        if(!hasNext)
        {
            return new Vector2Int(-1, -1);
        }

        return new Vector2Int(col + offx, row + offy);
    }

    public bool IsWalkable(int col, int row)
    {
        _graph.TryGet(col, row, out Node node);
        if(node == null)
        {
            return false;
        }

        if(node.fCost == NodeDefine.MaxCost)
        {
            return false;
        }

        return node.isWalkable;
    }

    private List<Node> tmpList = new List<Node>();
    public List<Node> GetNearWalkableNodes(int col, int row)
    {
        tmpList.Clear();
        Node node = null;
        for(int c = -2; c <= 2; c++)
        {
            for(int r = -2; r <= 2; r++)
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

    //private void OnGUI()
    //{
    //    if (_graph != null)
    //    {
    //        GUIStyle style = new GUIStyle();
    //        style.fontSize = 24;
    //        var nodes = _graph.Nodes;
    //        for (int i = 0; i < nodes.Length; i++)
    //        {
    //            var node = nodes[i];
    //            GUI.Label(new Rect(new Vector2(node.x * cellSizeX + offX, node.y * cellSizeY + offY), new Vector2(cellSizeX, cellSizeY)), node.direction.ToString(), style);
    //        }
    //    }
    //}
}
