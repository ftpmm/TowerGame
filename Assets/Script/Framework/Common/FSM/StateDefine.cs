using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public enum EStateExecute
    {
        None,
        Start,
        Running,
        Finish,
    }

    public class StateDefine
    {
        public static int STATE_ID_GEN = 1;
        public const int DEFAULT_STATE_PRIORITY = 100;
    }

    
}
