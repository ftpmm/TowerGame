using System.Collections;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace lzengine
{
    public class AddSceneState : BaseState
    {
        public AddSceneState(FSMObject owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            var actor = mOwner as SceneActor;
            actor.AddScene();
            mRunState = EStateExecute.Finish;
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            return base.OnExecute(deltaTime);
        }

        public override void OnExit()
        {
            mMachine.TransStateByPriority();
        }
    }
}