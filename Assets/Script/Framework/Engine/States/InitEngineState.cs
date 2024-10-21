using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class InitEngineState : BaseState
    {
        public InitEngineState(FSMObject owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 10);

            //初始化本地化设置
            LocalizationMgr.Instance.Init();
            LocalizationMgr.Instance.SetLocalization(ELocalization.Zhs);
            //初始化资源管理器
#if UNITY_EDITOR
            CoroutineRunner.Run(AssetsManager.Instance.InitAsync(EAssetServiceMode.Editor));
#else
            CoroutineRunner.Run(AssetsManager.Instance.InitAsync(EAssetServiceMode.Remote));
#endif
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            if(AssetsManager.Instance.IsInit)
            {
                mRunState = EStateExecute.Finish;
            }

            return base.OnExecute(deltaTime);
        }

        public override void OnExit()
        {
            mMachine.TransState<HotUpdateState>();
        }
    }
}
