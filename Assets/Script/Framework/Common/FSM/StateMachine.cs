using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

namespace lzengine
{
    public class StateMachine
    {
        public BaseState mCurState;

        public BaseState mNextState;

        private int mMinPriority = 999;

        public Dictionary<int, BaseState> mStatesDict = new Dictionary<int, BaseState>();

        public List<BaseState> mSortedStatesList = new List<BaseState>();

        private FSMObject mOwner;

        public StateMachine(FSMObject owner)
        {
            mOwner = owner;
        }

        public void Register(BaseState newState, int priority = StateDefine.DEFAULT_STATE_PRIORITY)
        {
            if(newState == null)
            {
                LZDebug.LogError("NewState is null!!");
                return;
            }

            BaseState tmpState = null;
            mStatesDict.TryGetValue(newState.mStateId, out tmpState);
            if(tmpState != null)
            {
                LZDebug.LogError("State is exist oldState = " + tmpState + ", newState = " + newState);
                return;
            }

            mStatesDict[newState.mStateId] = newState;
            newState.BindMachine(this);
            newState.mPriority = priority;

            if (mMinPriority > priority)
            {
                mNextState = newState;
                mMinPriority = priority;
            }

            mSortedStatesList.Add(newState);

            mSortedStatesList.Sort((a, b) => { return a.mPriority.CompareTo(b.mPriority); });
        }

        public void TransState<T>()where T : BaseState
        {
            T retState = null;
            foreach(var state in mStatesDict.Values)
            {
                if(state is T)
                {
                    retState = (T)state;
                    break;
                }
            }

            if(retState != null)
            {
                mNextState = retState;
            }
            else
            {
                LZDebug.LogError("TransState Failed !!! " + default(T).ToString());
            }
        }

        public void TransStateByPriority()
        {
            if(mCurState == null)
            {
                LZDebug.LogError("当前状态为空，无法通过优先级转换到下一个状态");
                return;
            }

            int nextStateIndex = -1;
            for(int i = 0; i < mSortedStatesList.Count; i++)
            {
                if (mSortedStatesList[i].mStateId == mCurState.mStateId)
                {
                    nextStateIndex = i + 1;
                }
            }

            if(nextStateIndex >= mSortedStatesList.Count)
            {
                LZDebug.LogError("状态已切换完，不存在下一个可切换的状态");
                return;
            }

            mNextState = mSortedStatesList[nextStateIndex];
        }

        public void Update(float deltaTime)
        {
            if(mCurState == null)
            {
                if(mNextState != null)
                {
                    mCurState = mNextState;
                    mCurState.OnEnter();
                    mNextState = null;
                }
                
                if(mCurState == null)
                {
                    return;
                }
            }

            var runState = mCurState.OnExecute(deltaTime);
            if(runState == EStateExecute.Finish)
            {
                mCurState.OnExit();
                if(mNextState != null)
                {
                    mCurState = mNextState;
                    mCurState.OnEnter();
                    mNextState = null;
                }
                else
                {
                    mCurState = null;
                }
            }
        }

        public void Destroy()
        {
            mCurState = null;
            mNextState = null;
            mStatesDict.Clear();
        }
    }
}
