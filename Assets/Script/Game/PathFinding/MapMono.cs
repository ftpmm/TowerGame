using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lzengine
{
    public class MapMono : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool IsPreviewMap;

        private Map _map;

        private void OnGUI()
        {
            if(_map == null)
            {
                if(IsPreviewMap)
                {
                    _map = GameLevelManager.Instance.GetPreviewMap();
                }
                else
                {
                    _map = GameLevelManager.Instance.GetMap();
                }
                
            }

            if (_map != null)
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 24;
                var nodes = _map.Graph.Nodes;
                for (int i = 0; i < nodes.Length; i++)
                {
                    var node = nodes[i];
                    GUI.Label(new Rect(new Vector2(node.x * 50 + 10, node.y * 50 + 10), new Vector2(50, 50)), node.direction.ToString(), style);
                }
            }
        }
#endif

    }
}

