using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class Enemy : PrefabActor
    {
        private Animator mAnime;

        public Vector3 mCurSpeed = Vector3.zero;

        public Rigidbody2D mRigidbody;

        private EnemyAnimeMono mMono;

        public float MoveSpeed
        {
            get
            {
                return mMono.param.moveSpeed;
            }
        }

        public Enemy() : base()
        {
            AddState<EnemyIdleState>(100);
            AddState<EnemyMoveState>(110);
            AddState<EnemyStunState>(120);
        }

        public override void OnLoadPrefab(GameObject go)
        {
            mMono = go.GetComponent<EnemyAnimeMono>();
            mRigidbody = go.GetComponent<Rigidbody2D>();
            mAnime = go.GetComponentInChildren<Animator>();
        }

        public override void OnInit()
        {
            base.OnInit();
        }

        public void PlayAnime(string animeName)
        {
            if (mAnime == null)
            {
                mAnime = mRootGo.GetComponent<Animator>();
            }
            if (mAnime != null) 
            {
                mAnime.Play(animeName);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public void Move(Vector3 distance)
        {
            mRootTrans.transform.position += distance;
            //mRigidbody.MovePosition(mRootGo.transform.position + distance);
            _position = mRootTrans.position;
        }
    }
}

