using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace lzengine
{
    public class SubTowerItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Button mBtn;

        public int TowerId;

        private Action<SubTowerItem, Vector2> mOnDragStart;
        private Action<SubTowerItem, Vector2> mOnDrag;
        private Action<SubTowerItem, Vector2> mOnDragEnd;

        public void InitItem()
        {
            
        }

        public void AddDragEvent(Action<SubTowerItem, Vector2> onDragStart, 
            Action<SubTowerItem, Vector2> onDrag, Action<SubTowerItem, Vector2> onDragEnd)
        {
            mOnDragStart = onDragStart;
            mOnDrag = onDrag;
            mOnDragEnd = onDragEnd;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (mOnDrag != null) 
            {
                mOnDrag(this, eventData.position);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            if (mOnDragStart != null) 
            {
                mOnDragStart(this, eventData.position);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");
            if (mOnDragEnd != null)
            {
                mOnDragEnd(this, eventData.position);
            }
        }
    }
}

