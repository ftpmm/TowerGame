using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class UIWait : UIBase
    {
        public override string AssetPath { get { return "prefab/ui/UIWait.prefab"; } }
        public override bool IsDestroyWhenClose{ get { return false; } }
    }
}
