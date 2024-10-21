using UnityEngine;
using System;
using System.Collections;
using System.IO;

namespace lzengine
{
    public class HotUpdateMgr:FSMObject
    {
        private bool _isInit = false;
        public bool IsInit
        {
            get { return _isInit; }
        }

        private bool _isFinishUpdate = false;
        public bool IsFinishUpdate
        {
            get
            {
                return _isFinishUpdate;
            }
        }

        public IEnumerator InitAsync()
        {
            _isInit = false;
            _isFinishUpdate = false;

            InitMachine();

            yield return null;
        }

        private void InitMachine()
        {
            AddState<UpdateBundleState>(0);
            AddState<UpdateLocalizationState>();
            AddState<UpdateAOTMetaState>();
            AddState<UpdateHotfixState>();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public void FinishUpdate()
        {
            _isFinishUpdate = true;
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
