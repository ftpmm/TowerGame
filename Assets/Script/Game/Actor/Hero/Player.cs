using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class Player : PrefabActor
    {
        private Animator mAnime;

        public Vector3 mCurSpeed = Vector3.zero;

        public float MoveSpeed = 2.4f;

        public PlayerCtrlMono mPlayerCtrl;

        public Rigidbody2D mRigidbody;

        public Player():base() 
        {
            AddState<PlayerIdleState>(110);
            AddState<PlayerMoveState>(120);
            AddState<PlayerMoveState>(130);
        }

        public override void OnLoadPrefab(GameObject go)
        {
            mPlayerCtrl = go.GetComponent<PlayerCtrlMono>();
            mRigidbody = go.GetComponent<Rigidbody2D>();
            mAnime = go.GetComponentInChildren<Animator>();
        }

        public override void OnInit()
        {
            base.OnInit();
        }

        public void PlayAnime(string animeName)
        {
            if(mAnime == null)
            {
                mAnime = mRootGo.GetComponent<Animator>();
            }
            mAnime.Play(animeName);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            mCurSpeed = mPlayerCtrl.moveDelta;
        }

        public void Move(Vector3 distance)
        {
            mRigidbody.MovePosition(mRootGo.transform.position + distance);
            _position = mRootTrans.position;
        }
    }
}

