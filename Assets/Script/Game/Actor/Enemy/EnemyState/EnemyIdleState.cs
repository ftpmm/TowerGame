using System;
using UnityEngine;

namespace lzengine
{
    public class EnemyIdleState : BaseState
    {
        private Enemy mEnemy;

        public EnemyIdleState(FSMObject owner) : base(owner)
        {
            mEnemy = owner as Enemy;
        }

        public override void OnEnter()
        {
            
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            mMachine.TransState<EnemyMoveState>();
            return EStateExecute.Finish;
        }
    }
}
