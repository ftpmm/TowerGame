using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace lzengine
{
    public class EnemyMoveState : BaseState
    {
        private Enemy mEnemy;

        private Vector2 mTargetPos;
        private bool hasTarget = false;

        public EnemyMoveState(FSMObject mOwner) : base(mOwner)
        {
            mEnemy = mOwner as Enemy;
        }

        public override void OnEnter()
        {
            //mEnemy.PlayAnime("Run");
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            //获取下一个格子的位置
            Vector2 curPos = mEnemy.Position;

            //检查当前格子是否可走
            bool isWalkable = GameLevelManager.Instance.IsWalkable(curPos);
            if(!isWalkable)
            {
                Vector2 walkablePos = GameLevelManager.Instance.GetWalkablePos(curPos);
                mEnemy.Position = walkablePos;
                curPos = mEnemy.Position;
                hasTarget = false;
            }

            if(hasTarget)
            {
                bool isTargetWalkable = GameLevelManager.Instance.IsWalkable(mTargetPos);
                if (!isTargetWalkable)
                {
                    hasTarget = false;
                    mTargetPos = curPos;
                }
            }

            if (!hasTarget) 
            {
                Vector2 nextPos = Vector2.zero;
                Vector2Int cellPos = GameLevelManager.Instance.TryGetNextMapPos(mEnemy.Position, out nextPos);
                if(cellPos.x != -1)
                {
                    hasTarget = true;
                    mTargetPos = nextPos;
                }
                else
                {
                    mTargetPos = curPos;
                }
            }
            else
            {
                
                float sqr = (mTargetPos - curPos).sqrMagnitude;
                if(sqr < 0.01f)
                {
                    hasTarget = false;
                }
            }

            Vector2 offset = mTargetPos -  curPos;
            Vector2 dir = offset.normalized;

            float speed = mEnemy.MoveSpeed;
            if (dir == Vector2.zero)
            {
                mMachine.TransState<EnemyIdleState>();
                return EStateExecute.Finish;
            }

            Vector3 delDistance = dir * speed * Time.deltaTime;
            if(delDistance.sqrMagnitude > offset.sqrMagnitude)
            {
                delDistance = offset;
            }

            if (dir.x < 0)
            {
                mEnemy.Forward = (Vector3.one);
            }
            else if (dir.x > 0)
            {
                mEnemy.Forward = (Vector3.one * -1);
            }

            mEnemy.Move(delDistance);

            return EStateExecute.Running;
        }
    }
}
