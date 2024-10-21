using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace lzengine
{
    public class GameState : BaseState
    {
        public GameState(FSMObject owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            EventManager.Instance.AddEventListener<bool>(LZEngineEventDefine.Game_Init, OnGameInitCallBack);
            EventManager.Instance.Dispatch<bool>(LZEngineEventDefine.Game_Init, false);

            mRunState = EStateExecute.Running;
            var handler = AssetsManager.Instance.LoadAssetAsync<GameObject>("prefab/base/game");
            handler.Completed += OnLoadComplete;
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            return base.OnExecute(deltaTime);
        }

        public override void OnExit()
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 100);
        }

        private void OnLoadComplete(AssetHandle handler)
        {
            GameObject prefab = handler.AssetObject as GameObject;
            if (prefab != null)
            {
                //实例化游戏prefab实例,到此开始执行热更的代码
                GameObject go = GameObject.Instantiate(prefab);
            }
            else
            {
                LZDebug.LogError("Load game prefab failed!!! " + handler.LastError);
            }
        }

        private void OnGameInitCallBack(bool ret)
        {
            if(ret)
            {
                mRunState = EStateExecute.Finish;
            }
        }
    }
}
