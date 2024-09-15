using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Tools;
using _GAME.Scripts.UI.MoveTargets;
using _Game.Scripts.View;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.Base
{
    public class UIVisualItem : BaseView
    {
        private RectTransform _rectTransform;
        private List<UIVisualConfig> _viewConfigs;
        public UIVisualConfig ItemConfig { get; private set; }

        public void SetupItemView(UITargetType itemType)
        {
            var viewConf = _viewConfigs.Find(c => c.TargetType == itemType);
            viewConf.Visual.Activate();
            ItemConfig = viewConf;
        }
        public override void Init(params object[] list)
        {
            base.Init();
            _rectTransform = GetComponent<RectTransform>();
            _viewConfigs = GetComponentsInChildren<UIVisualConfig>(true).ToList();

        }

        public override void Reset()
        {
            base.Reset();
            for (int i = 0; i < _viewConfigs.Count; i++)
            {
                _viewConfigs[i].Visual.Deactivate();
            }
        }

        public void SetPosition(Vector2 screenPosition)
        {
            _rectTransform.position = screenPosition;
        }

        public void Move(Transform target, Action callback)
        {
            _rectTransform.DOMove(target.position, 1f).SetEase(Ease.InSine).OnComplete(()=>callback?.Invoke());
        }

        public void SetRotation(float rotation)
        {
            _rectTransform.localEulerAngles = new Vector3(0, 0, rotation);
        }
    }
}
