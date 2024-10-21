using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class UpdateLocalizationState : BaseState
    {
        public UpdateLocalizationState(FSMObject owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            CoroutineRunner.Run(TableManager.Instance.InitASync());
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            if(TableManager.Instance.IsInit)
            {
                mRunState = EStateExecute.Finish;
            }

            return base.OnExecute(deltaTime);
        }

        public override void OnExit() 
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 60);
            mMachine.TransState<UpdateAOTMetaState>();
        }
    }
}
