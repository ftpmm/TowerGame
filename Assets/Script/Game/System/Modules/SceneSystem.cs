using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lzengine
{
    public class SceneGrid
    {
        private Vector2Int mPos;
        private Dictionary<int, SceneActor> mActorDict = new Dictionary<int, SceneActor>();

        public void AddActor(SceneActor actor)
        {
            if (actor == null) return;

            mActorDict[actor.uuid] = actor;
        }

        public void RemoveActor(SceneActor actor) 
        { 
            if(actor == null) return;

#if UNITY_EDITOR
            if(!mActorDict.ContainsKey(actor.uuid))
            {
                LZDebug.LogErrorFormat("单位不包含在该单元格内 gridPos={0}, uuid={1}", mPos, actor.uuid);
            }
#endif

            mActorDict.Remove(actor.uuid);
        }

        public IEnumerator GetEnumerator()
        {
            return mActorDict.GetEnumerator();
        }

        public void Clear()
        {
            mActorDict.Clear();
        }
    }

    public class SceneSystem : BaseSystem
    {
        /// <summary>
        /// 场景格子尺寸
        /// </summary>
        public Vector2Int GridSize = new Vector2Int(5,5);

        Dictionary<Vector2Int, SceneGrid> mGridsDict = new Dictionary<Vector2Int, SceneGrid>();

        public override void Awake()
        {
            base.Awake();
        }

        public override IEnumerator InitASync()
        {
            isInit = true;

            return base.InitASync();
        }

        public void AddActor(SceneActor actor)
        {
            if(actor == null) return;

            Vector2Int gridPos = PosToGridPos(actor.Position);
            SceneGrid grid = null;
            mGridsDict.TryGetValue(gridPos, out grid);
            if(grid == null)
            {
                grid = new SceneGrid();
                mGridsDict[gridPos] = grid;
            }
            grid.AddActor(actor);
        }

        public void RemoveActor(SceneActor actor)
        {
            if(actor == null) return;

            Vector2Int gridPos = PosToGridPos(actor.Position);
            SceneGrid grid = null;
            mGridsDict.TryGetValue(gridPos, out grid);
            if (grid == null)
            {
                LZDebug.LogError("单位没加入到SceneSystem中！！uuid = " + actor.uuid);
                return;
            }
            grid.RemoveActor(actor);
        }

        public Vector2Int PosToGridPos(Vector3 pos)
        {
            return new Vector2Int((int)pos.x / GridSize.x, (int)pos.y / GridSize.y);
        }
    }
}