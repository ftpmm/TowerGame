using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    /// <summary>
    /// 代码热更
    /// </summary>
    public class UpdateHotfixState : BaseState
    {
        bool isLoadFinish = false;

        public UpdateHotfixState(FSMObject owner) : base(owner)
        {
            isLoadFinish = false;
        }

        public override void OnEnter()
        {
            string hotFixPath = "hotfix/dll/Game.dll.bytes";
            var handler = AssetsManager.Instance.LoadAllAssetsAsync<TextAsset>(hotFixPath);
            handler.Completed += (files) => {
                if (files != null && files.AllAssetObjects != null)
                {
                    var objs = files.AllAssetObjects;
                    //找到dll列表文件
                    List<string> hotfixFiles = new List<string>();
                    Dictionary<string, TextAsset> fileDict = new Dictionary<string, TextAsset>();
                    for (int i = 0; i < objs.Length; i++)
                    {
                        TextAsset asset = objs[i] as TextAsset;
                        if (asset.name == "dinfo")
                        {
                            StringReader reader = new StringReader(asset.text);
                            while (reader.Peek() > -1)
                            {
                                string fileName = reader.ReadLine();
                                hotfixFiles.Add(fileName);
                            }
                        }
                        else
                        {
                            fileDict[asset.name] = asset;
                        }
                    }

                    if(hotfixFiles.Count > 0)
                    {
                        //需要按顺序加载dll
                        for (int i = 0; i < hotfixFiles.Count; i++)
                        {
                            TextAsset asset = null;
                            fileDict.TryGetValue(hotfixFiles[i], out asset);
                            if (asset == null)
                            {
                                LZDebug.LogError("Load HotFix File Failed!!!! file = " + hotfixFiles[i]);
                                continue;
                            }
                            Assembly.Load(asset.bytes);
                        }
                    }
                    else
                    {
                        foreach(var asset in fileDict.Values)
                        {
                            Assembly.Load(asset.bytes);
                        }
                    }

                    hotfixFiles.Clear();
                    fileDict.Clear();

                    OnLoadFileCallback(true);
                }
                else
                {
                    OnLoadFileCallback(false);
                }
            };
        }

        private void OnLoadFileCallback(bool ret)
        {
            isLoadFinish = true;
            if(!ret)
            {
                LZDebug.LogError("load dll file failed!!!");
            }
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            if(isLoadFinish)
            {
                return EStateExecute.Finish;
            }

            return EStateExecute.Running;
        }

        public override void OnExit()
        {
            EventManager.Instance.Dispatch<float>(LZEngineEventDefine.Engine_Init, 80);
            HotUpdateMgr mgr = mOwner as HotUpdateMgr;
            if(mgr != null)
            {
                mgr.FinishUpdate();
            }
        }
    }
}
