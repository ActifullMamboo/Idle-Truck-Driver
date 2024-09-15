using System;
using System.Collections.Generic;
using _Game.Scripts.Tools;
using _Game.Scripts.View;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

namespace _GAME.Scripts.Base
{
    public enum ItemType
    {
        none,
        Tree,
        Wheat,
        Wool,
        Stone,
        Coal,
        Iron,
        Bones,
        IronSlitok
    }
    public class BaseItem : BaseTriggerItem
    {
        [SerializeField, ReadOnly] List<ViewConfig> ViewConfigs;
        [SerializeField, ReadOnly] List<ItemConfig> ItemConfigs;

        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        public ItemConfig ItemConfig { get; private set; }
        private PointView _currentPoint;

        public bool InGardener { get; set; }

        public override void Init(params object[] list)
        {
            base.Init();

            _meshFilter = GetComponentInChildren<MeshFilter>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public void SetupItemView(ItemType itemType)
        {
            var viewConf = ViewConfigs.Find(c => c.ItemType == itemType);
            _meshRenderer.material = viewConf.Material;
            _meshFilter.mesh = viewConf.Mesh;
            SetupItemConfig(itemType);
        }

        private void SetupItemConfig(ItemType itemType)
        {
            ItemConfig = ItemConfigs.Find(c => c.ItemType == itemType);
        }

        #region  MoveRegion

        

        public void MoveToFreePoint(PointView freePoint, Action callback, Transform parent)
        {
            PrepareForMove(freePoint, parent);

            transform.DOLocalJump(Vector3.zero, 3f, 1, 0.66f).SetEase(Ease.InSine)
                .OnComplete(()=>callback?.Invoke());
        }

        public void MoveToGardener(PointView freePoint, Action callback, Transform parent)
        {
            InGardener = true;
            PrepareForMove(freePoint, parent);

            transform.DOLocalJump(Vector3.zero, 3f, 1, 0.66f).SetEase(Ease.InSine)
                .OnComplete(()=>callback?.Invoke());
        }
        public void ForceMoveToPoint(PointView freePoint, Action callback, Transform parent)
        {
            PrepareForMove(freePoint, parent);
            var transform1 = transform;
            transform1.localPosition = Vector3.zero;
            transform1.localEulerAngles = Vector3.zero;
            callback?.Invoke();
        }
        public void MoveToFreePoint(PointView freePoint, Action callback, float delay)
        {
            PrepareForMove(freePoint, freePoint.transform);

            transform.DOLocalJump(Vector3.zero, 3f, 1, 0.66f).SetEase(Ease.Linear)
                .SetDelay(delay).OnComplete(() =>
                {
                    OnDestinationComplete();
                    callback?.Invoke();
                });
        }
        public void MoveToFreePointOnDelay(PointView freePoint, Action callback, float delay)
        {
            if (_currentPoint != null)
            {
                _currentPoint.FreePoint();
            }
            _currentPoint = freePoint;
            _currentPoint.SetReserved(true);
            gameObject.Activate();
            transform.SetParent(_currentPoint.transform);
            
            transform.DOLocalJump(Vector3.zero, 3f, 1, 0.66f).SetEase(Ease.Linear)
                .SetDelay(delay).OnComplete(() =>
                {
                    OnDestinationComplete();
                    callback?.Invoke();
                    _currentPoint.SetItem(this);

                });
        }
        public void MoveToFreePoint(PointView freePoint, Action callback, float delay, Transform parent, bool needDeactivate = false)
        {
            PrepareForMove(freePoint, parent);
            transform.DOLocalJump(Vector3.zero, 3f, 1, 0.66f).SetEase(Ease.Linear)
                .SetDelay(delay).OnComplete(() =>
                {
                    if (needDeactivate)
                    {
                        gameObject.Deactivate();
                    }
                    callback?.Invoke();
                });
        }

        public void OnDestinationComplete()
        {
            transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.OutBounce);
            transform.DOPunchScale(Vector3.one*0.4f, 0.66f, 5, 1).SetEase(Ease.OutSine);
            transform.DOScale(1f, 0.33f).SetEase(Ease.OutSine).SetDelay(0.66f);
        }
        private void PrepareForMove(PointView freePoint, Transform parent)
        {
            if (_currentPoint != null)
            {
                _currentPoint.FreePoint();
            }
            _currentPoint = freePoint;
            _currentPoint.SetItem(this);

            gameObject.Activate();
            transform.SetParent(parent);
        }
        #endregion
    }

    [Serializable]
    public class ViewConfig
    {
        public ItemType ItemType;
        public Mesh Mesh;
        public Material Material;
    }

    [Serializable]
    public class ItemConfig
    {
        public ItemType ItemType;
        public int ItemID;
        public float RecoveryTime;
        public int HitsToCollect;
        public ParticleSystem HitFX;
    }
}

