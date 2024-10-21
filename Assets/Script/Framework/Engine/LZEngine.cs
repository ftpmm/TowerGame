using lzengine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework
{
    public class LZEngine : FSMObject
    {
        public LZEngine(MonoBehaviour root):base()
        {
            CoroutineRunner.Init(root);

            AddState<InitEngineState>(0);
            AddState<HotUpdateState>();
            AddState<GameState>();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Destroy()
        {
            base.Destroy();
            CoroutineRunner.StopAll();
        }
    }
}


