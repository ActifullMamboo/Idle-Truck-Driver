using System;
using _Game.Scripts.Ui.Base;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.UI.WorldSpace
{
    public class CapasityVisualizer : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI _capacityText;
        public Vector3 _offset;
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
            Anchor anchor = new()
            {
                Scale = Vector3.one,
                EulerAngles = Vector3.zero,
                Position = Vector3.zero
            };

            if (target.TryGetComponent(out WorldSpacePanelOffset worldSpacePanelOffset))
                anchor = worldSpacePanelOffset.Anchor;

            var transform1 = transform;
            _offset =  anchor.Position;
            transform1.eulerAngles = Camera.main.transform.eulerAngles;
            transform1.localScale = anchor.Scale;
            
        }

        private void LateUpdate()
        {
            transform.position = _target.position + _offset;
        }

        public void Show(int current, int max)
        {
            _capacityText.text = current + "/" + max;
        }
        
    }
}
