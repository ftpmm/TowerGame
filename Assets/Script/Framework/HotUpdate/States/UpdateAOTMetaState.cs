using HybridCLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    /// <summary>
    /// 代码热更
    /// </summary>
    public class UpdateAOTMetaState : BaseState
    {

        bool isLoadFinish = false;

        public UpdateAOTMetaState(FSMObject owner) : base(owner)
        {
            isLoadFinish = false;
        }

        public override void OnEnter()
        {
            var handler = AssetsManager.Instance.LoadAllAssetsAsync<TextAsset>("hotfix/meta/mscorlib.dll.bytes");
            handler.Completed += (files) => {
                if (files != null && files.AllAssetObjects != null)
                {
                    var objs = files.AllAssetObjects;
                    for (int i = 0; i < objs.Length; i++)
                    {
                        TextAsset asset = objs[i] as TextAsset;
                        RuntimeApi.LoadMetadataForAOTAssembly(asset.bytes, HomologousImageMode.SuperSet);
                    }
                    OnLoadMetaCallback(true);
                }
                else
                {
                    OnLoadMetaCallback(false);
                }
            };
        }

        private void OnLoadMetaCallback(bool ret)
        {
            isLoadFinish = true;
            if (!ret)
            {
                LZDebug.LogError("Load Meta Fail");
            }
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            if (isLoadFinish)
            {
                return EStateExecute.Finish;
            }

            return EStateExecute.Running;
        }

        public override void OnExit()
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 70);
            mMachine.TransState<UpdateHotfixState>();
        }
    }
}
