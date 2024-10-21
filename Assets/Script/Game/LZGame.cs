using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class LZGame:FSMObject
    {
        private bool mIsInit = false;
        public bool IsInit {  get { return mIsInit; } }

        private GameSystemMgr mSysMgr;

        public void Awake()
        {
            LZDebug.Log("初始化LZGame");
            mSysMgr = GameSystemMgr.Instance;
        }

        public IEnumerator InitASync()
        {
            yield return mSysMgr.InitASync();

            mIsInit = true;

            AddState<LoginState>(0);
            AddState<MainState>();
        }

        public override void Update(float deltaTime)
        {
            if (mIsInit)
            {
                base.Update(deltaTime);
                mSysMgr.Update(deltaTime);
            }
        }

        public void OnApplicationPause()
        {

        }

        public void Destroy()
        {

        }
    }
}
