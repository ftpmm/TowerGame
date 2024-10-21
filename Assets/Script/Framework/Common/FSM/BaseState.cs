using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class BaseState
    {
        public readonly int mStateId;

        public BaseStateParam mParam;

        public StateMachine mMachine;

        protected FSMObject mOwner;

        protected EStateExecute mRunState;

        public int mPriority = StateDefine.DEFAULT_STATE_PRIORITY;

        public BaseState(FSMObject owner)
        {
            mOwner = owner;
            mStateId = StateDefine.STATE_ID_GEN++;
        }

        public BaseState(FSMObject owner, BaseStateParam param)
        {
            mOwner = owner;
            mParam = param;
            mStateId = StateDefine.STATE_ID_GEN++;
        }

        public void SetParam(BaseStateParam param)
        {
            mParam = param;
        }

        public void BindMachine(StateMachine machine)
        {
            mMachine = machine;
        }

        public virtual void OnEnter()
        {
            mRunState = EStateExecute.Finish;
        }

        public virtual EStateExecute OnExecute(float deltaTime)
        {
            return mRunState;
        }

        public virtual void OnExit()
        {
        }
    }
}
