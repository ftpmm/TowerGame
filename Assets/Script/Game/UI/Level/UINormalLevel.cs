﻿using UnityEngine;
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
            itemCom.AddDragEvent(OnItemDragStart, OnItemDrag, OnItemDragEnd);
        }

        private void OnItemDragStart(SubTowerItem item, Vector2 sPos)
        { 
            if(mCurDragItem == null)
            {
                var actorSys = GameSystemMgr.GetSystem<ActorSystem>();
                mCurDragItem = actorSys.CreateActor<TowerForUI>("prefab/tower/Tower1");
            }
        }

        private void OnItemDrag(SubTowerItem item, Vector2 sPos)
        {
            if(mCurDragItem == null)
            {
                return;
            }
            var wPos = Camera.main.ScreenToWorldPoint(sPos);
            mCurDragItem.Position = wPos;
        }

        private void OnItemDragEnd(SubTowerItem item, Vector2 sPos)
        {
            if (mCurDragItem == null)
            {
                return;
            }

            mCurDragItem.Destroy();
            mCurDragItem = null;
        }
    }
}