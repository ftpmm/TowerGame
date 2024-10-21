using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace lzengine
{
    public class GameLevelManager
    {
        private static GameLevelManager _instance;

        public static GameLevelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameLevelManager();
                }

                return _instance;
            }
        }

        private LevelBase curLevel;

        public Map GetPreviewMap()
        {
            if (curLevel != null)
            {
                return curLevel.mMapPreview;
            }
            return null;
        }

        public Map GetMap()
        {
            if (curLevel != null)
            {
                return curLevel.mMap;
            }
            return null;
        }

        public void Init()
        {

        }

        public void StartLevel(int levelId)
        {
            var normalLevel = GameObject.FindAnyObjectByType<LevelNormal>();
            normalLevel.InitLevel(levelId);
            curLevel = normalLevel;
        }

        public Vector2Int TryGetNextMapPos(Vector3 pos, out Vector2 retPos)
        {
            return curLevel.TryGetNextMapPos(pos, out retPos);
        }

        public Vector2 GetWalkablePos(Vector2 pos)
        {
            return curLevel.GetWalkablePos(pos);
        }

        public Vector2Int GetLevelCellPos(Vector2 pos)
        {
            if(curLevel == null)
            {
                return Vector2Int.zero;
            }

            return curLevel.GetLevelCellByPos(pos);
        }

        public bool IsWalkable(Vector3 pos)
        {
            return curLevel.IsWalkable(pos);
        }

        public bool IsTowerPlacable(TowerForUI tower)
        {
            return curLevel.IsTowerPlacable(tower.Position, tower.TowerSize);
        }

        public void AddFightTower(int towerId, Vector2 pos, Vector2Int towerSize)
        {
            var cellPos = curLevel.GetLevelCellByPos(pos);
            var mapPos = curLevel.GetLevelMapPos(cellPos);
            var actorSys = GameSystemMgr.GetSystem<ActorSystem>();
            var fightTower = actorSys.CreateActor<TowerForFight>("prefab/tower/Tower" + towerId);
            fightTower.Position = mapPos;
            curLevel.SetMapWalkable(cellPos, towerSize, false);
        }
    }
}
