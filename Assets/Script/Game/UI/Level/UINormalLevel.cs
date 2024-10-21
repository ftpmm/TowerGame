using UnityEngine;
using System;
using UnityEngine.UI;

namespace lzengine
{
    public class UINormalLevel : UIBase
    {
        public override string AssetPath => "prefab/ui/UINormalLevel";

        private UINormalLevelMono mMono;

        private TowerForUI mCurDragItem;

        public override void OnLoadAfter()
        {
            mMono = GetMono<UINormalLevelMono>();

            AddTowerItem(1);
            AddTowerItem(2);
        }

        public void AddTowerItem(int towerId)
        {
            var towerPrefab = mMono.m_goTowerItem;
            var towerGo = GameObject.Instantiate(towerPrefab, mMono.m_goTowerItem.transform.parent);
            towerGo.transform.localScale = Vector3.one;
            towerGo.SetActive(true);
            var itemCom = towerGo.GetComponent<SubTowerItem>();
            if(itemCom == null)
            {
                itemCom = towerGo.AddComponent<SubTowerItem>();
            }
            itemCom.TowerId = towerId;
            itemCom.AddDragEvent(OnItemDragStart, OnItemDrag, OnItemDragEnd);
        }

        private void OnItemDragStart(SubTowerItem item, Vector2 sPos)
        { 
            if(mCurDragItem == null)
            {
                var actorSys = GameSystemMgr.GetSystem<ActorSystem>();
                mCurDragItem = actorSys.CreateActor<TowerForUI>("prefab/tower/Tower" + item.TowerId);
            }
        }

        private void OnItemDrag(SubTowerItem item, Vector2 sPos)
        {
            if (mCurDragItem == null)
            {
                return;
            }
            var wPos = Camera.main.ScreenToWorldPoint(sPos);
            wPos.z = 0;
            mCurDragItem.Position = wPos;
            GameLevelManager.Instance.SetPlacableState(mCurDragItem.Position, mCurDragItem.TowerSize, mCurDragItem.IsPlacable);
        }

        private void OnItemDragEnd(SubTowerItem item, Vector2 sPos)
        {
            if (mCurDragItem == null)
            {
                return;
            }
            if(!mCurDragItem.IsPlacable)
            {
                mCurDragItem.Destroy();
                LZDebug.LogError("放置失败！！！");
            }
            else
            {
                GameLevelManager.Instance.AddFightTower(mCurDragItem.TowerId, mCurDragItem.Position, mCurDragItem.TowerSize);
                mCurDragItem.Destroy();
            }
            mCurDragItem = null;
        }
    }
}