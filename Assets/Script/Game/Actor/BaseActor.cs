using lzengine;
using System;

namespace lzengine
{
    public class BaseActor : FSMObject
    {
        private static int UUID_NEXT = 1;

        public int uuid;

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        public bool isInit = false;
        /// <summary>
        /// 是否已经销毁
        /// </summary>
        public bool isDestroy = false;

        public BaseActor():base()
        {
            uuid = UUID_NEXT++;
            AddState<ActorInitState>(100);
        }

        /// <summary>
        /// 初始化actor
        /// </summary>
        public void Init()
        {
            OnInit();
        }

        /// <summary>
        /// 更新actor
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            if (isInit)
            {
                OnUpdate(deltaTime);
            }
        }

        /// <summary>
        /// 销毁actor
        /// </summary>
        public override void Destroy()
        {
            OnDestroy();
            base.Destroy();
            isDestroy = true;
        }

        //***************************子类覆写的方法***************************
        public virtual void OnInit() { isInit = true; }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnDestroy() { }
    }
}

