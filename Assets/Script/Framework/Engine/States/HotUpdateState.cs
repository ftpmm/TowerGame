using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace lzengine
{
    public class HotUpdateState : BaseState
    {
        private HotUpdateMgr mHotUpdateMgr;

        public HotUpdateState(FSMObject owner) : base(owner)
        {
            mHotUpdateMgr = new HotUpdateMgr();
        }

        public override void OnEnter()
        {
            CoroutineRunner.Run(InitRes());
        }

        private IEnumerator InitRes()
        {
            yield return mHotUpdateMgr.InitAsync();
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            EStateExecute runState = EStateExecute.Running;

            mHotUpdateMgr.Update(deltaTime);

            if(mHotUpdateMgr.IsFinishUpdate)
            {
                runState = EStateExecute.Finish;
            }

            return runState;
        }

        public override void OnExit()
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 90);
            mMachine.TransState<GameState>();
        }
    }
}
