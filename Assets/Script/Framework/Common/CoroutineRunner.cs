using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class CoroutineRunner
    {
        public static MonoBehaviour mRoot;

        public static void Init(MonoBehaviour root)
        {
            mRoot = root;
        }

        public static Coroutine Run(IEnumerator  func)
        {
            return mRoot.StartCoroutine(func);
        }

        public static void Stop(Coroutine cor)
        {
            if(cor == null)
            {
                return;
            }
            mRoot.StopCoroutine(cor);
        }

        public static void StopAll()
        {
            mRoot.StopAllCoroutines();
        }
    }
}
