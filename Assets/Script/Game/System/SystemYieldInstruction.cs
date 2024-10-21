using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class SystemYieldInstruction : CustomYieldInstruction
    {
        BaseSystem mGameSys;

        public override bool keepWaiting
        {
            get
            {
                return !mGameSys.isInit;
            }
        }

        public SystemYieldInstruction(BaseSystem gameSys)
        {
            mGameSys = gameSys;
        }
    }
}
