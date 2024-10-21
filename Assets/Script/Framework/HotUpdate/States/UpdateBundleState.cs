using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class UpdateBundleState : BaseState
    {
        public UpdateBundleState(FSMObject owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 20);
            CoroutineRunner.Run(AssetsManager.Instance.StartUpdate());
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            if(AssetsManager.Instance.IsUpdateFinish)
            {
                mRunState = EStateExecute.Finish;
            }

            return base.OnExecute(deltaTime);
        }

        public override void OnExit()
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 50);
            mMachine.TransState<UpdateLocalizationState>();
        }
    }
}
