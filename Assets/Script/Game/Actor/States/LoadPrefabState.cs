using lzengine;
using System.Collections;
using UnityEngine;

namespace lzengine
{
    public class LoadPrefabState : BaseState
    {
        PrefabActor actor;

        public LoadPrefabState(FSMObject owner) : base(owner)
        {
            actor = mOwner as PrefabActor;
        }

        public override void OnEnter()
        {
            actor.LoadPrefab();
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            if(actor.mIsPrefabLoaded)
            {
                mRunState = EStateExecute.Finish;
            }

            return base.OnExecute(deltaTime);
        }

        public override void OnExit()
        {
            mMachine.TransStateByPriority();
        }
    }
}