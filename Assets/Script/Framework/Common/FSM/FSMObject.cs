using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class FSMObject
    {
        public StateMachine mMachine;

        public FSMObject()
        {
            mMachine = new StateMachine(this);
        }

        public void AddState<T>(int priority = 100) where T:BaseState
        {
            var newState = Activator.CreateInstance(typeof(T), this);

            if(newState == null)
            {
                return;
            }

            mMachine.Register(newState as BaseState, priority);
        }

        public void TransState<T>() where T : BaseState
        {
            mMachine.TransState<T>();
        }

        public void AddStateWithParam<T>(BaseStateParam param, int priority = 100)
        {
            var newState = Activator.CreateInstance(typeof(T), this, param);

            if (newState == null)
            {
                return;
            }

            mMachine.Register(newState as BaseState, priority);
        }

        public virtual void Update(float deltaTime)
        {
            if(mMachine != null)
            {
                mMachine.Update(deltaTime);
            }
        }

        public virtual void Destroy()
        {
            mMachine.Destroy();
        }
    }
}
