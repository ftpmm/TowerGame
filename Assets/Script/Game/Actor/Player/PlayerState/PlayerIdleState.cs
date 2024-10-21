using System;
using UnityEngine;

namespace lzengine
{
    public class PlayerIdleState : BaseState
    {
        private Player mPlayer;

        public PlayerIdleState(FSMObject owner) : base(owner)
        {
            mPlayer = owner as Player;
        }

        public override void OnEnter()
        {
            mPlayer.PlayAnime("Idle");
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            var curSpeed = mPlayer.mCurSpeed;
            if (curSpeed != Vector3.zero)
            {
                mMachine.TransState<PlayerMoveState>();
                return EStateExecute.Finish;
            }

            return EStateExecute.Running;
        }
    }
}
