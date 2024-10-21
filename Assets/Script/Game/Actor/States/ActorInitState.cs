using System.Collections;
using UnityEngine;

namespace lzengine
{
    public class ActorInitState : BaseState
    {
        private BaseActor actor;

        public ActorInitState(FSMObject owner) : base(owner)
        {
            actor = owner as BaseActor;
        }

        public override void OnEnter()
        {
            actor.Init();
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            if(actor.isInit)
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