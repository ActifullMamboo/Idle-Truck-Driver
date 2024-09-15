using System;
using UnityEngine;

namespace _Game.Scripts.View
{
    public class CollisionListener : BaseView
    {
        public event Action<Collider> OnEnter;
        public event Action<Collision> OnCollided;
        public event Action<Collider> OnStay;
        public event Action<Collider> OnExit;

        private void OnCollisionEnter(Collision collision)
        {
            OnCollided?.Invoke(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(other);
        }


    }
}