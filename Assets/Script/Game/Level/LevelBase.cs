using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        /// <summary>
        /// 地图实际尺寸
        /// </summary>
        public float mapStartX;
        public float mapStartY;
        public float mapEndX;
        public float mapEndY;

        private float cellSizeX = 1;
        private float cellSizeY = 1;

        /// <summary>
        /// 阻挡标记
        /// </summary>
        public int[,] mBlocks;

        public Map mMap;
        public Map mMapPreview;

        private void Start()
        {
            cellSizeX = (mapEndX - mapStartX) / sizeX;
            cellSizeY = cellSizeX;
            sizeY = (int)(Mathf.Abs(mapEndY - mapStartY) / cellSizeX) + 1;

            var mapMono = GameObject.FindAnyObjectByType<MapMono>();
            if(mMap == null)
            {
                mMap = new Map();
            }
            if (mMapPreview == null)
            {
                mMapPreview = new Map();
            }

            Debug.Log("cellSize = " + cellSizeX + ", xNum = " + sizeX + ", yNum = " + sizeY);
            DoStart();
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

            mMapPreview.SetWalkable(cellPos, size.x, size.y, false);
            bool ret = mMapPreview.IsTargetWalkable();
            //还原
            mMapPreview.CloneFrom(mMap);
            return ret;
        }

        public void SetMapWalkable(Vector2Int cellPos, Vector2Int size, bool isWalkable)
        {
            mMap.SetWalkable(cellPos, size.x, size.y, isWalkable);
            mMapPreview.CloneFrom(mMap);
        }
    }
}

