using System;
using System.Collections.Generic;
using UnityEngine;

namespace lzengine
{
    public class UIRoot:MonoBehaviour
    {
        public GameObject mUIRoot;
        public Camera mUICam;

        public static UIRoot Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
