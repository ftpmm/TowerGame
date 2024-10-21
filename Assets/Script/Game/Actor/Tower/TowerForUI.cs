using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class TowerForUI : PrefabActor
    {
        private TowerMono mMono;

        public Vector2Int TowerSize
        {
            get 
            { 
                return new Vector2Int(mMono.sizeX, mMono.sizeY);
            }
        }

        public int TowerId
        {
            get
            {
                return mMono.towerId;
            }
        }

        public bool IsPlacable = false;
        public Vector2Int lastCellPos = new Vector2Int(-1,-1);

        public TowerForUI() : base()
        {
            
        }

        public override void OnLoadPrefab(GameObject go)
        {
            mMono = go.GetComponent<TowerMono>();

            var sprites = go.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].sortingLayerName = "UITop";
            }
            UIUtils.ChangeLayer(go.transform, LayerDefine.UI);
            go.transform.localScale = Vector3.one * 0.7f;
        }

        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnUpdate(float deltaTime)
        {
            var ins = GameLevelManager.Instance;

            Vector2Int curCellPos = ins.GetLevelCellPos(_position);
            if (curCellPos.x != lastCellPos.x || curCellPos.y != lastCellPos.y) 
            {
                IsPlacable = ins.IsTowerPlacable(this);
                lastCellPos = curCellPos;
            }
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}

