using System;
using UnityEngine;

namespace lzengine
{
    public class EnemyStunState : BaseState
    {
        private Enemy mEnemy;

        private float stunTime = 1.0f;

        public EnemyStunState(FSMObject owner) : base(owner)
        {
            mEnemy = owner as Enemy;
        }

        public override void OnEnter()
        {
            stunTime = 1.0f;
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            stunTime -= deltaTime;
            if(stunTime <= 0)
            {
                mMachine.TransState<EnemyMoveState>();
                return EStateExecute.Finish;
            }
            return EStateExecute.Running;
        }
    }
}
