using System.Collections;
using UnityEngine;

namespace lzengine
{
    public class  UIUtils
    {
        /// <summary>
        /// 动态更改所有子物体Layer
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="targetLayer"></param>
        public static void ChangeLayer(Transform trans, int targetLayer)
        {
            //遍历更改所有子物体layer
            trans.gameObject.layer = targetLayer;
            foreach (Transform child in trans)
            {
                ChangeLayer(child, targetLayer);
            }
        }
    }
}