using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace lzengine
{
    public class GameSystemMgr : Singleton<GameSystemMgr>
    {
        /// <summary>
        /// 游戏所有的系统列表
        /// </summary>
        private Dictionary<Type, BaseSystem> mSysDict = new Dictionary<Type, BaseSystem>();

        public GameSystemMgr()
        {
            var baseType = typeof(BaseSystem);
            var assembly = baseType.Assembly;
            var allSysType = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

            foreach (var subType in allSysType)
            {
                var subIns = Activator.CreateInstance(subType);
                mSysDict[subType] = subIns as BaseSystem;
                mSysDict[subType].Awake();
            }
        }

        public IEnumerator InitASync()
        {
            LZDebug.Log("初始化各个System");
            foreach (var sys in mSysDict.Values)
            {
                yield return sys.InitASync();
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var sys in mSysDict.Values)
            {
                sys.Update(deltaTime);
            }
        }

        private T GetSystem_Internal<T>() where T : BaseSystem
        {
            BaseSystem ret = null;
            mSysDict.TryGetValue(typeof(T), out ret);
            return ret as T;
        }

        public static T GetSystem<T>() where T : BaseSystem
        {
            return Instance.GetSystem_Internal<T>();
        }

    }
}
