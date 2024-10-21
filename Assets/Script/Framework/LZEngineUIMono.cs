using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace lzengine
{
    public class LZEngineUIMono:MonoBehaviour
    {
        public Image progressBar;
        public float speed = 4.0f;

        private float progress = 0.0f;

        private void Awake()
        {
            progressBar.fillAmount = 0.0f;
        }

        private void OnEnable()
        {
            EventManager.Instance.AddEventListener<float>(LZEngineEventDefine.Engine_Init, OnProgressChange);
            EventManager.Instance.AddEventListener<bool>(LZEngineEventDefine.Game_Init, OnGameInitCallback);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveEventListener<float>(LZEngineEventDefine.Engine_Init, OnProgressChange);
            EventManager.Instance.RemoveEventListener<bool>(LZEngineEventDefine.Game_Init, OnGameInitCallback);
        }

        private void Update()
        {
            float curValue = progressBar.fillAmount;
            if(curValue == 1.0f)
            {
                return;
            }

            float offset = Mathf.Abs(progress - curValue);
            if(curValue < 0.99f)
            {
                float delta = Time.deltaTime * speed;
                if (delta > offset)
                {
                    delta = offset;
                }

                progressBar.fillAmount += delta;
            }
            else
            {
                progressBar.fillAmount = 1;
            }
            
        }

        private void OnProgressChange(float v)
        {
            v = Mathf.Min(v, 100f);
            progress = v / 100.0f;
        }

        private void OnGameInitCallback(bool ret)
        {
            if(ret)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
