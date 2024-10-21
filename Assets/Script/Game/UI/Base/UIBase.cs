using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class UIBase
    {
        /// <summary>
        /// 当前UI层级
        /// </summary>
        public int mOrder;

        public GameObject mRoot;

        private UIBaseMono _mMono;

        private bool mIsActive;
        public bool IsActive                                                                                                                                                                                                                                                                                                                                                                                                
        {
            get { return mIsActive; }
            set { 
                mIsActive = value;
                SetActive(value);
            }

        }


        public virtual string AssetPath
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual bool IsDestroyWhenClose
        {
            get
            {
                return true;
            }
        }

        public virtual void Awake()
        {
        }

        public void SetRoot(GameObject root)
        {
            mRoot = root;
            _mMono = mRoot.GetComponent<UIBaseMono>();
        }

        public T GetMono<T>()where T : UIBaseMono
        {
            if(_mMono == null)
            {
                return default(T);
            }

            return (T)_mMono;
        }

        public virtual void OnLoadBefore()
        {

        }

        /// <summary>
        /// 加载ui的gameobject成功后调用。
        /// </summary>
        public virtual void OnLoad()
        {

        }

        /// <summary>
        /// 加载gameobject完成后调用，可以重复调用
        /// </summary>
        public virtual void OnLoadAfter()
        {

        }

        private void SetActive(bool isActive)
        {
            if(mRoot != null)
            {
                mRoot.SetActive(isActive);
            }
        }

        public virtual void BeforeDestroy()
        {

        }

        public virtual void AfterDestroy()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public void Destroy()
        {
            OnDestroy();
        }

        public void Close()
        {
            UIManager.Instance.CloseUI(this);
        }
    }
}
