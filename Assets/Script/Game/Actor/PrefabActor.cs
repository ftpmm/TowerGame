using System;
using System.Collections.Generic;
using UnityEngine;

namespace lzengine
{
    public class PrefabActor:SceneActor
    {
        public override Vector3 Position
        {
            get
            {
                if(mRootTrans)
                {
                    return mRootTrans.position;
                }

                return _position;
            }
            set
            {
                if (mRootTrans)
                {
                    mRootTrans.position = value;
                }

                _position = value;
            }
        }

        public override Vector3 Forward
        { 
            get
            {
                return _forward;
            }
            set
            {
                _forward = value;
                if(mRootTrans)
                {
                    mScale.x = _forward.x;
                    mRootTrans.localScale = mScale;
                }
            }
        }

        public virtual string AssetPath {  get; set; }

        public bool mIsPrefabLoaded = false;

        public GameObject mRootGo;
        public Transform mRootTrans;

        private Vector3 mScale = Vector3.one;

        public PrefabActor():base()
        { 
            AddState<LoadPrefabState>(80);
        }

        public override void OnInit()
        {
            base.OnInit();
        }

        public void LoadPrefab()
        {
            AssetsManager.Instance.LoadAssetAsync<GameObject>(AssetPath, PrefabLoadCallBack);
        }

        private void PrefabLoadCallBack(GameObject prefab)
        {
            mIsPrefabLoaded=true;

            if(prefab == null)
            {
                mRootGo = null;
                mScale = Vector3.one;
                OnLoadPrefab(null);
                return;
            }

            mRootGo = GameObject.Instantiate(prefab);
            mRootTrans = mRootGo.transform;
            mScale = mRootGo.transform.localScale;

            Position = _position;
            Forward = _forward;

            OnLoadPrefab(mRootGo);
        }

        public virtual void OnLoadPrefab(GameObject go) { }
    }
}
