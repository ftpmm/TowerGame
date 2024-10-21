using Google.FlatBuffers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lzengine
{
    public interface ITableObject:IFlatbufferObject
    {
        public ITableObject InitTable(ByteBuffer buffer);
    }

    public class TableManager:Singleton<TableManager>
    {
        private bool isInit = false;

        private Dictionary<string, TextAsset> configAssetDict = new Dictionary<string, TextAsset>();
        private Dictionary<string, ITableObject> configDict = new Dictionary<string, ITableObject>();


        public bool IsInit
        {
            get
            {
                return isInit;
            }
        }

        public IEnumerator InitASync()
        {
            isInit = false;
            var handler = AssetsManager.Instance.LoadAllAssetsAsync<TextAsset>("data/table/hero.bytes");
            yield return handler;
            if(handler.IsDone)
            {
                configAssetDict.Clear();
                var objs = handler.AllAssetObjects;
                if(objs != null)
                {
                    for(int i = 0; i < objs.Length; i++)
                    {
                        TextAsset asset = objs[i] as TextAsset;
                        configAssetDict[asset.name] = asset;
                    }
                }

                isInit = true;
            }
        }

        public static T GetTable<T>(string tableName) where T : ITableObject
        {
            ITableObject tmpT = null;
            bool ret = Instance.configDict.TryGetValue(tableName, out tmpT);
            if (ret)
            {
                return (T)tmpT;
            }

            var buffer = GetByteBuffer(tableName);
            tmpT = Activator.CreateInstance<T>();
            tmpT = tmpT.InitTable(buffer);

            Instance.configDict[tableName] = tmpT;

            return (T)tmpT;
        }

        private static ByteBuffer GetByteBuffer(string tableName)
        {
            TextAsset asset = null;
            Instance.configAssetDict.TryGetValue(tableName, out asset);
            if (asset != null)
            {
                return new ByteBuffer(asset.bytes);
            }

            return null;
        }
    }
}
