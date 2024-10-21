using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class ActorSystem:BaseSystem
    {
        private Dictionary<int,  BaseActor> mActorsDict = new Dictionary<int, BaseActor>();

        public override IEnumerator InitASync()
        {
            isInit = true;

            return initYield;
        }

        public T CreateActor<T>(string assetPath = null) where T:BaseActor
        {
            T ins = Activator.CreateInstance<T>();
            mActorsDict[ins.uuid] = ins;

            if(assetPath != null)
            {
                PrefabActor prefabActor = ins as PrefabActor;
                if(prefabActor != null)
                {
                    prefabActor.AssetPath = assetPath;
                }
            }

            return ins;
        }

        public override void Update(float deltaTime)
        {
            foreach(var actor in mActorsDict.Values)
            {
                actor.Update(deltaTime);
            }
        }
    }
}
