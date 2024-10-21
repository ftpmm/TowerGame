using Framework;
using System;
using UnityEngine;

namespace lzengine
{
    public class LZEngineMono:MonoBehaviour
    {
        private LZEngine engine;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            engine = new LZEngine(this);
        }

        private void Update()
        {
            if (engine != null)
            {
                engine.Update(Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            if (engine != null) 
            {
                engine.Destroy();
            }
            engine = null;
        }
    }
}
