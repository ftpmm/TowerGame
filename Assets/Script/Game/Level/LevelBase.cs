using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace lzengine
{
    public class LevelBase : MonoBehaviour
    {
        /// <summary>
        /// x方向有多少格
        /// </summary>
        public int sizeX;
        /// <summary>
        /// y方向有多少格
        /// </summary>
        public int sizeY;

        /// <summary>
        /// 可放置的范围
        /// </summary>
        public int placeStartX;
        public int placeStartY;
        public int placeEndX;
        public int placeEndY;

        public List<Vector2Int> monsterSpawnPos = new List<Vector2Int>();

        /// <summary>
        /// 地图实际尺寸
        /// </summary>
        public float mapStartX;
        public float mapStartY;
        public float mapEndX;
        public float mapEndY;

        private float cellSizeX = 1;
        private float cellSizeY = 1;

        public Map mMap;
        public Map mMapPreview;

        //放置格子的颜色控制
        private Color colorNormal = new Color(1, 1, 1, 0.1f);
        private Color colorRed = new Color(1, 0, 0, 0.1f);
        private Color colorNormal_NoWalkable = new Color(1, 1, 1, 0.06f);
        private Color colorGreen = new Color(0, 1, 0, 0.2f);
        private List<Vector2Int> mLastSetGeZiList = new List<Vector2Int>();
        private Dictionary<int,GameObject> mGeZiDict = new Dictionary<int, GameObject>();

        private void Start()
        {
            cellSizeX = (mapEndX - mapStartX) / sizeX;
            cellSizeY = cellSizeX;
            sizeY = (int)(Mathf.Abs(mapEndY - mapStartY) / cellSizeX) + 1;

            if(mMap == null)
            {
                mMap = new Map();
            }
            if (mMapPreview == null)
            {
                mMapPreview = new Map();
            }

            monsterSpawnPos.Clear();
            var monsterSpawnMonos = GameObject.FindObjectsByType<EnemySpawnMono>(FindObjectsSortMode.None);
            for (int i = 0; i < monsterSpawnMonos.Length; i++) 
            {
                Vector2Int spawnPos = GetLevelCellByPos(monsterSpawnMonos[i].transform.position);
                monsterSpawnPos.Add(spawnPos);
            }

            Debug.Log("cellSize = " + cellSizeX + ", xNum = " + sizeX + ", yNum = " + sizeY);

            InitGeZi();

            DoStart();
        }

        private void InitGeZi()
        {
            AssetsManager.Instance.LoadAssetAsync<GameObject>("prefab/level/level_gezi", (prefab) => { 
                if(prefab == null)
                {
                    return;
                }

                for (int x = placeStartX; x <= placeEndX; x++)
                {
                    for(int y = placeStartY; y <= placeEndY; y++)
                    {
                        Vector2 pos = GetLevelMapPos(new Vector2Int(x, y));
                        var geziGo = GameObject.Instantiate(prefab);
                        geziGo.transform.position = pos;
                        int cellIndex = y * sizeX + x;
                        mGeZiDict[cellIndex] = geziGo;
                    }
                }
            });
        }

        private void Update()
        {
            float time = Time.deltaTime;
            DoUpdate(time);
        }

        public virtual void DoStart() { }
        public virtual void DoUpdate(float deltaTime) { }
        public virtual void InitLevel(int levelId) { }

        public Vector2Int GetLevelCellByPos(Vector3 pos)
        {
            float offX = Mathf.Max(pos.x - mapStartX, 0);
            float offY = Mathf.Max(mapStartY - pos.y, 0);

            int cellX = (int)(offX / cellSizeX);
            int cellY = (int)(offY / cellSizeY);

            return new Vector2Int(cellX, cellY);
        }

        public Vector2 GetLevelMapPos(Vector2Int cell)
        {
            float mx = mapStartX + cell.x * cellSizeX;
            float my = mapStartY - cell.y * cellSizeY;

            return new Vector2(mx, my);
        }

        public Vector2 GetLevelMapCenterPos(Vector2Int cell)
        {
            float mx = mapStartX + cell.x * cellSizeX + cellSizeX * 0.5f;
            float my = mapStartY - cell.y * cellSizeY - cellSizeY * 0.5f;

            return new Vector2(mx, my);
        }

        public Vector2Int TryGetNextMapPos(Vector2 pos, out Vector2 retPos)
        {
            retPos = Vector2.zero;
            Vector2Int cellPos = GetLevelCellByPos(pos);
            Vector2 mapCenterPos = GetLevelMapCenterPos(cellPos);
            float disSqr = (mapCenterPos - pos).sqrMagnitude;
            if (disSqr < 0.009f)
            {
                cellPos = mMap.GetNextNode(cellPos.x, cellPos.y);
                if (cellPos.x != -1)
                {
                    mapCenterPos = GetLevelMapCenterPos(cellPos);
                }
            }

            retPos = mapCenterPos;

            return cellPos;
        }

        public bool IsWalkable(Vector2 pos)
        {
            Vector2Int cellPos = GetLevelCellByPos(pos);
            return mMap.IsWalkable(cellPos.x, cellPos.y);
        }

        public Vector2 GetWalkablePos(Vector2 pos)
        {
            Vector2Int cellPos = GetLevelCellByPos(pos);
            var tmpList = mMap.GetNearWalkableNodes(cellPos.x, cellPos.y);
            Node closeNode = null;
            float closeDis = 0.0f;
            for (int i = 0; i < tmpList.Count; ++i)
            {
                var node = tmpList[i];
                if (closeNode == null)
                {
                    closeNode = node;
                    var centerPos = GetLevelMapCenterPos(new Vector2Int(node.x, node.y));
                    var dis1 = (centerPos - pos).sqrMagnitude;
                    closeDis = dis1;
                }
                else
                {
                    var centerPos = GetLevelMapCenterPos(new Vector2Int(node.x, node.y));
                    var dis1 = (centerPos - pos).sqrMagnitude;
                    if (closeDis > dis1)
                    {
                        closeNode = node;
                        closeDis = dis1;
                    }
                }
            }
            Vector2 retPos = pos;
            if (closeNode != null)
            {
                retPos = GetLevelMapCenterPos(new Vector2Int(closeNode.x, closeNode.y));
            }
            return retPos;
        }

        public bool IsTowerPlacable(Vector2 pos, Vector2Int size)
        {
            Vector2Int cellPos = GetLevelCellByPos(pos);
            if(cellPos.y > placeEndY || cellPos.y < placeStartY)
            {
                return false;
            }
            if (cellPos.x < placeStartX || cellPos.x > placeEndX) 
            {
                return false;
            }
            
            bool isPlacable = mMapPreview.IsPlacable(cellPos, size.x, size.y);
            if (!isPlacable) 
            {
                return false;
            }

            //预测放置后地图的怪物的可行走区域
            bool isWalkable = true;
            mMapPreview.SetWalkable(cellPos, size.x, size.y, false);
            for (int i = 0; i < monsterSpawnPos.Count; i++)
            {
                isWalkable &= mMapPreview.IsWalkable(monsterSpawnPos[i].x, monsterSpawnPos[i].y);
                if(!isWalkable)
                {
                    break;
                }
            }
            isWalkable &= mMapPreview.IsTargetWalkable();

            //还原地图
            mMapPreview.CloneFrom(mMap);

            return isWalkable;
        }

        public void SetMapWalkable(Vector2Int cellPos, Vector2Int size, bool isWalkable)
        {
            mMap.SetWalkable(cellPos, size.x, size.y, isWalkable);
            mMapPreview.CloneFrom(mMap);
            
            for(int x = cellPos.x; x < cellPos.x + size.x; x++)
            {
                for(int y = cellPos.y; y < cellPos.y + size.y; y++)
                {

                    int index = y * sizeX + x;
                    mGeZiDict.TryGetValue(index, out GameObject go);
                    if(go != null)
                    {
                       var sp = go.GetComponent<SpriteRenderer>();
                        if(sp != null)
                        {
                            sp.color = colorNormal_NoWalkable;
                        }
                    }
                }
            }
        }

        public void SetPlacableState(Vector2 pos, Vector2Int size, bool isPlacable)
        {
            if(mLastSetGeZiList.Count > 0)
            {
                for (int i = 0; i < mLastSetGeZiList.Count; i++)
                {
                    var cPos = mLastSetGeZiList[i];
                    int cellIndex = cPos.y * sizeX + cPos.x;
                    bool isWalkable = mMap.IsWalkable(cPos.x, cPos.y);
                    if (isWalkable)
                    {
                        SetGeZiState(cellIndex, colorNormal);
                    }
                    else
                    {
                        SetGeZiState(cellIndex, colorNormal_NoWalkable);
                    }
                }
                mLastSetGeZiList.Clear();
            }
            

            Vector2Int cellPos = GetLevelCellByPos(pos);
            for (int x = cellPos.x; x < cellPos.x + size.x; x++)
            {
                for (int y = cellPos.y; y < cellPos.y + size.y; y++)
                {
                    Vector2Int tmpCellPos = new Vector2Int(x, y);
                    mLastSetGeZiList.Add(tmpCellPos);

                    int index = y * sizeX + x;
                    if(isPlacable)
                    {
                        SetGeZiState(index, colorGreen);
                    }
                    else
                    {
                        SetGeZiState(index, colorRed);
                    }
                }
            }
        }

        private void SetGeZiState(int index, Color color)
        {
            mGeZiDict.TryGetValue(index, out GameObject go);
            if (go != null)
            {
                var sp = go.GetComponent<SpriteRenderer>();
                if (sp != null)
                {
                    sp.color = color;
                }
            }
        }
    }
}

