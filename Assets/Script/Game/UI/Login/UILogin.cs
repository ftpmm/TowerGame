using lzengine;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace lzengine
{
    public class UILogin:UIBase
    {
        public override string AssetPath => "prefab/ui/UILogin";

        public override void OnLoad()
        {
            UILoginMono mono = GetMono<UILoginMono>();
            var btn = mono.m_txtTip.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClickEnterGame);
        }

        private void OnClickEnterGame()
        {
            EventManager.Instance.Dispatch(GameEventDefine.Enter_Game);
        }
    }
}
