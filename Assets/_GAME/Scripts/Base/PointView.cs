using _Game.Scripts.Tools;
using _Game.Scripts.View;
using System;
using System.Collections.Generic;
using _GAME.Scripts.Base;
using DG.Tweening;
using Sirenix.Utilities;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public enum PointType
{
    None,
CharacterSpawnPoint,
CharacterEndPoint,
CarSpawnPoint,
CarEndPoint
}
public class PointView : BaseView
{
    public Action<PointView> OnFree;
    public Action<PointView> OnBusy;

    [SerializeField] private PointType _type;
    [SerializeField] private List<string> _animationKeys;
    [SerializeField, ReadOnly] private bool _isAvailable = true;
    [SerializeField, ReadOnly] private BaseView _item;

    private List<int> _animationHashes;

    public PointType Type => _type;
    public BaseView Item => _item;
    public bool IsFree => _item == null && !IsReserved;

    public bool IsAvailable => _isAvailable;
    public bool IsReserved { get; private set; }

    public int AnimationHash => _animationHashes.IsNullOrEmpty() ? 0 : _animationHashes.RandomValue();

    public override void Init(params object[] list)
    {
        _animationHashes = new List<int>();
        foreach (var key in _animationKeys)
        {
            _animationHashes.Add(Animator.StringToHash(key));
        }

        base.Init();
    }

    public void SetItem(BaseView item)
    {
        _item = item;
        if (_item != null) OnBusy?.Invoke(this);
    }

    public void FreePoint()
    {
        _item = null;
        IsReserved = false; 
        OnFree?.Invoke(this);
    }

    public void SetAvailable(bool flag)
    {
        _isAvailable = flag;
        if (flag) OnFree?.Invoke(this);
    }

    public void SetReserved(bool flag)
    {
        IsReserved = flag;
    }

    public override void Reset()
    {
        OnFree = null;
        OnBusy = null;
        _item = null;
    }

    public void VisualRotate()
    {
        float delay = Random.Range(0.3f, 3f);
        transform.DOLocalMoveY(0.2f, 1f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
        transform.DOLocalRotate(Vector3.up * 360, 6f).SetRelative(true).SetEase(Ease.Linear).SetDelay(delay).SetLoops(-1, LoopType.Restart);
    }
}
