using System;
using _Game.Scripts.View;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.Base
{
    public class CustomCollisionVisualizer : MonoBehaviour
    {
        [SerializeField] private CollisionListener _collisionListener;
        [SerializeField] private Transform _visualizer;

        private void Start()
        {
            _collisionListener.OnEnter += AnimateIn;
            _collisionListener.OnExit += AnimateOut;
        }

        private void AnimateOut(Collider obj)
        {
            _visualizer.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        private void AnimateIn(Collider obj)
        {
            _visualizer.transform.DOScale(Vector3.one*1.2f, 0.5f).SetEase(Ease.OutBack);
        }
    }
}
