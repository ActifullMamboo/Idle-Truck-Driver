using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.UI.WorldSpace.Max
{
    public class MaxPanel : BaseWindow
    {
        private Tween _delayTween;

        private bool _opened;

        public void Show(Transform target, float delay = 1f)
        {
            if (_opened)
                return;

            _opened = true;

            gameObject.Activate();

            transform.DOScale(1f, 1f).SetEase(Ease.InSine);
            Anchor anchor = new()
            {
                Scale = Vector3.one,
                EulerAngles = Vector3.zero,
                Position = Vector3.zero
            };

            if (target.TryGetComponent(out WorldSpacePanelOffset worldSpacePanelOffset))
                anchor = worldSpacePanelOffset.Anchor;

            var transform1 = transform;
            transform1.position = target.position + anchor.Position;
            transform1.eulerAngles = anchor.EulerAngles;
            transform1.localScale = anchor.Scale;
            base.Open();

            DOVirtual.Float(0, 1, delay, value => transform1.position = target.position + anchor.Position);
            _delayTween.Kill();
            _delayTween = DOVirtual.DelayedCall(delay, Close);
        }

        public override void Close()
        {
            transform.DOScale(0, 0.3f).SetEase(Ease.InSine).OnComplete(() =>
            {
                _opened = false;
                gameObject.Deactivate();
            });
            base.Close();
        }
    }
}