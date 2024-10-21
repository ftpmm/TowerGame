using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lzengine
{
    public class ImageRotate:MonoBehaviour
    {
        private RectTransform mTrans;

        public float rotateSpeed = 1.0f;

        private float angle = 0;

        private void Awake()
        {
            mTrans = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (mTrans != null)
            {
                angle += Time.deltaTime * rotateSpeed;
                mTrans.localEulerAngles = new Vector3(0, 0, angle);
            }
        }
    }
}
