using System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Ui.Base.Animations
{
    public class BaseUIAnimation : BaseUIView
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] protected float DurationShow = 0.15f, DurationHide = 0.1f;

        [SerializeField] protected Ease CurveShow = Ease.OutSine;
        [SerializeField] protected Ease CurveHide = Ease.InSine;
        [SerializeField] protected float delay;
        private float _defaultDelay = 0;
        
        protected RectTransform CacheRect;
        protected Tweener Tween;
        public bool needDeactivate = false;

        public virtual void Init()
        {
            CacheRect = _target != null ? _target : RectTransform;
            _defaultDelay = delay;
        }

        public void ResetDelay()
        {
            delay = _defaultDelay;
        }
        public void SetDelay(float d)
        {
            delay = d;
        }
        public virtual void PlayOpenAnimation(RectTransform windowRect, Action callback = null)
        {
        }

        public virtual void PlayCloseAnimation(RectTransform windowRect, Action callback = null)
        {
        }
    }
}