using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class UIManager:Singleton<UIManager>
    {
        /// <summary>
        /// 打开的UI字典
        /// </summary>
        private Dictionary<string, UIBase> uiDict = new Dictionary<string, UIBase>();

        private UIRoot mUIRoot;

        public Camera UICamear
        {
            get
            {
                return mUIRoot.mUICam;
            }
        }

        private const int defaultOrderInLayer = 1000;

        /// <summary>
        /// 记录每层sortintLayer当前最大的order in layer
        /// </summary>
        private Dictionary<int, int> uiLayerTopOderDict = new Dictionary<int, int>();

        public void InitRoot(UIRoot root)
        {
            mUIRoot = root;
        }

        public void OpenWaitUI()
        {
            OpenUIASync<UIWait>(null, 1, false);
        }

        public void CloseWaitUI()
        {
            CloseUI<UIWait>();
        }

        public void OpenUIASync<T>(Action<T> callBack, int sortingLayerId = 0, bool isShowWaitUI = true) where T : UIBase
        {
            var uiType = typeof(T);
            var uiTypeName = uiType.Name;

            UIBase ui = null;
            uiDict.TryGetValue(uiTypeName, out ui);
            if (ui == null)
            {
                ui = Activator.CreateInstance<T>();
                ui.Awake();
            }

            ui.OnLoadBefore();

            if (ui.mRoot == null)
            {
                if(string.IsNullOrEmpty(ui.AssetPath))
                {
                    LZDebug.LogError(string.Format("{0}的AssetPath 为空，加载UI失败!", uiTypeName));
                    return;
                }

                if(isShowWaitUI)
                {
                    OpenWaitUI();
                }

                AssetsManager.Instance.LoadAssetAsync<GameObject>(ui.AssetPath, (uiPrefab) =>
                {
                    if(isShowWaitUI)
                    {
                        CloseWaitUI();
                    }

                    if (uiPrefab == null)
                    {
                        LZDebug.LogError(string.Format("初始化{0}的Prefab失败 path={1}", uiTypeName, ui.AssetPath));
                        return;
                    }

                    var uiGo = GameObject.Instantiate(uiPrefab, mUIRoot.transform);
                    ui.SetRoot(uiGo);
                    ui.OnLoad();

                    AddUI(uiTypeName, ui, sortingLayerId);
                    ui.OnLoadAfter();

                    if (callBack != null)
                    {
                        callBack((T)ui);
                    }
                });
            }
            else
            {
                AddUI(uiTypeName, ui, sortingLayerId);
                ui.OnLoadAfter();

                if (callBack != null)
                {
                    callBack((T)ui);
                }
            }

        }

        private void AddUI(string uiName, UIBase ui, int sortingLayerId)
        {
            if(ui == null || ui.mRoot == null)
            {
                return;
            }
            SortUIOrder(ui.mRoot, sortingLayerId);
            uiDict[uiName] = ui;
        }

        private void SortUIOrder(GameObject go, int sortingLayerId)
        {
            var canvas = go.GetComponent<Canvas>();
            int layerId = 0;
            if (canvas != null)
            {
                canvas.overrideSorting = true;
                if(sortingLayerId >= SortingLayer.layers.Length)
                {
                    LZDebug.LogError(string.Format("{0}设置的SortingLayerId 在 SortingLayer中不存在，请先在SortingLayer中设置值 id = {1}", go.name, sortingLayerId));
                    sortingLayerId = 0;
                }
                canvas.sortingLayerID = SortingLayer.layers[sortingLayerId].id;
                layerId = canvas.sortingLayerID;
            }
            else
            {
                LZDebug.LogError(string.Format("{0}最上层必须有Canvas组件", go.name));
            }

            int topOrder = 0;
            uiLayerTopOderDict.TryGetValue(layerId, out topOrder);
            if (topOrder <= defaultOrderInLayer)
            {
                topOrder = defaultOrderInLayer;
            }

            if(canvas != null && topOrder != canvas.sortingOrder)
            {
                canvas.sortingOrder = topOrder + 100;
            }
        }

        public T GetUI<T>()where T : UIBase
        {
            var uiType = typeof(T);
            var uiTypeName = uiType.Name;

            UIBase ui = null;
            uiDict.TryGetValue(uiTypeName, out ui);
            if (ui != null)
            {
                return (T)ui;
            }

            return default(T);
        }

        public void CloseUI<T>()
        {
            var uiType = typeof(T);
            var uiTypeName = uiType.Name;

            UIBase ui = null;
            uiDict.TryGetValue(uiTypeName, out ui);
            if(ui != null)
            {
                CloseUI(ui);
            }
        }

        public void CloseUI(UIBase ui)
        {
            if(ui == null)
            {
                return;
            }

            ui.BeforeDestroy();

            if (ui.mRoot != null)
            {
                var canvas = ui.mRoot.GetComponent<Canvas>();

                int topOrderId = 0;
                uiLayerTopOderDict.TryGetValue(canvas.sortingLayerID, out topOrderId);

                if (topOrderId == canvas.sortingOrder)
                {
                    topOrderId = topOrderId - 100;
                }
                topOrderId = topOrderId - 100;
                uiLayerTopOderDict[canvas.sortingLayerID] = topOrderId;
            }

            ui.Destroy();
            ui.IsActive = false;

            if (ui.IsDestroyWhenClose)
            {
                var uiType = ui.GetType();
                uiDict.Remove(uiType.Name);   

                AssetsManager.Instance.DestroyGameObject(ui.mRoot);
                ui.mRoot = null;
            }

            ui.AfterDestroy();
        }
    }
}
