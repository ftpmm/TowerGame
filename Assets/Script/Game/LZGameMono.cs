using System;
using System.Collections;
using UnityEngine;

namespace lzengine
{
    public class LZGameMono:MonoBehaviour
    {
        private LZGame mGame;

        private float deltaTime = 0;

        private const float FrameTime = 0.02f;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            DontDestroyOnLoad(gameObject);

            mGame = new LZGame();
            mGame.Awake();
        }

        private IEnumerator Start()
        {
            yield return mGame.InitASync();
        }

        private void Update()
        {
            deltaTime += Time.deltaTime;
            if(deltaTime >= FrameTime)
            {
                deltaTime -= FrameTime;
                mGame.Update(deltaTime);
            }
        }

        private void OnApplicationPause(bool pause)
        {
            mGame.OnApplicationPause();
        }

        private void OnDestroy()
        {
            mGame.Destroy();
        }
    }
}
