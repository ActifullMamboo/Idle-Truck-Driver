using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Ui.Base.Animations
{
    public class AlphaGuiAnimation : BaseUIAnimation
    {
        private Image _image;
        private float _defaultAlpha;

        public override void Init()
        {
            base.Init();
            _image = GetComponent<Image>();
            _defaultAlpha = _image.color.a;
        }

        public override void PlayOpenAnimation(RectTransform windowRect, Action callback = null)
        {
            var color = _image.color;
            color.a = 0;
            _image.color = color;
            color.a = _defaultAlpha;
            
            Tween = _image.DOColor(color, DurationShow)
                .SetEase(CurveShow)
                .SetUpdate(true).SetDelay(delay)
                .OnComplete(() => callback?.Invoke());
        }

        public override void PlayCloseAnimation(RectTransform windowRect, Action callback = null)
        {
            var color = _image.color;
            color.a = 0;
            Tween = _image.DOColor(color, DurationShow)
                .SetEase(CurveShow)
                .SetUpdate(true).SetDelay(delay)
                .OnComplete(() => callback?.Invoke());
        }
    }
}