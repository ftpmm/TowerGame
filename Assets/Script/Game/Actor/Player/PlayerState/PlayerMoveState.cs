using System;
using UnityEngine;

namespace lzengine
{
    public class PlayerMoveState : BaseState
    {
        private Player mPlayer;

        public PlayerMoveState(FSMObject mOwner):base(mOwner)
        {
            mPlayer = mOwner as Player;
        }

        public override void OnEnter()
        {
            mPlayer.PlayAnime("Run");
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            var curSpeed = mPlayer.mCurSpeed;
            float speed = mPlayer.MoveSpeed;
            if(curSpeed == Vector3.zero)
            {
                mMachine.TransState<PlayerIdleState>();
                return EStateExecute.Finish;
            }

            Vector3 direct = curSpeed.normalized;
            Vector3 delDistance = direct * speed * Time.deltaTime;

            if (curSpeed.x > 0)
            {
                mPlayer.Forward = (Vector3.one);
            }
            else if (curSpeed.x < 0)
            {
                mPlayer.Forward = (Vector3.one * -1);
            }

            mPlayer.Move(delDistance);

            return EStateExecute.Running;
        }
    }
}
