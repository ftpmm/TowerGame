using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace lzengine
{
    public class PlayerCtrlMono : MonoBehaviour
    {
        public class ActionName
        {
            public const string Move = "Move";
        }

        private PlayerInput mInput;

        public Vector2 moveDelta = Vector2.zero;

        private void Awake()
        {
            mInput = GetComponent<PlayerInput>();
            if(mInput != null)
            {
                mInput.camera = Camera.main;
                mInput.onDeviceLost += OnDeviceLost;
                mInput.onDeviceRegained += OnDeviceLost;
                mInput.onControlsChanged += OnControlsChanged;
                mInput.onActionTriggered += OnActionTrigger;
            }
        }

        private void OnDeviceLost(PlayerInput input)
        {

        }

        private void OnDeviceRegained(PlayerInput input)
        {

        }

        private void OnControlsChanged(PlayerInput input)
        {

        }

        private void OnActionTrigger(InputAction.CallbackContext content)
        {
            switch(content.action.name)
            {
                case ActionName.Move:
                    OnMove(content.action.ReadValue<Vector2>());
                    break;
            }
        }

        private void OnMove(Vector2 delta)
        {
            moveDelta = delta;
        }
    }
}


