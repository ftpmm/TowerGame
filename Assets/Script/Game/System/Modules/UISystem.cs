using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class UISystem:BaseSystem
    {
        private List<Canvas> tmpCanvas = new List<Canvas>();

        public override IEnumerator InitASync()
        {
            UIManager.Instance.InitRoot(UIRoot.Instance);

            //加载等待UI
            UIManager.Instance.OpenUIASync<UIWait>((ui) => {
                UIManager.Instance.CloseWaitUI();
                isInit = true;

            }, 1, false);


            yield return initYield;
        }
    }
}
