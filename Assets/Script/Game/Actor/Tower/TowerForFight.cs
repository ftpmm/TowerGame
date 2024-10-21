using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class TowerForFight : PrefabActor
    {
        private TowerMono mMono;


        public TowerForFight() : base()
        {

        }

        public override void OnLoadPrefab(GameObject go)
        {
            mMono = go.GetComponent<TowerMono>();
        }

        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}

