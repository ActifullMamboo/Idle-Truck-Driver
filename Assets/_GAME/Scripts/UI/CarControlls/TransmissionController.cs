using System;
using SimpleInputNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _GAME.Scripts.UI.CarControlls
{
    public class TransmissionController : MonoBehaviour,ISimpleInputDraggable
    {

        private bool _forward = true;
        public Action<bool> OnPress;
        public void Init()
        {
            SimpleInputDragListener eventReceiver = gameObject.AddComponent<SimpleInputDragListener>();
            eventReceiver.Listener = this;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _forward = !_forward;
            OnPress?.Invoke(_forward);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}
