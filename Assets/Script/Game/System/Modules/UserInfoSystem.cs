using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class UserInfoSystem:BaseSystem
    {
        public override IEnumerator InitASync()
        {
            isInit = true;

            yield return initYield;
        }
    }
}
