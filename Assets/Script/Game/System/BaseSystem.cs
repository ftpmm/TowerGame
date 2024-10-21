using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class BaseSystem
    {
        public bool isInit = false;

        public bool isDestroy = false;

        protected SystemYieldInstruction initYield;
        public BaseSystem() 
        {
            initYield = new SystemYieldInstruction(this);
        }

        public virtual void Awake()
        {
            LZDebug.LogFormat("{0} Awake", this.GetType().Name.ToString());
        }

        public virtual IEnumerator InitASync()
        {
            LZDebug.LogFormat("{0} InitASync", this.GetType().Name.ToString());
            yield return initYield;
        }

        public virtual void Update(float deltaTime)
        {

        }
    }
}
