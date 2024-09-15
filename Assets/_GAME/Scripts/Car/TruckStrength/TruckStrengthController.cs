using System;
using _GAME.Scripts.Configs;
using _Game.Scripts.View;
using UnityEngine;
namespace _GAME.Scripts.Car.TruckStrength
{
    public class TruckStrengthController : CollisionListener
    {
        public Action<TruckStrengthController> OnDie;
        private float _startStrength;
        private float _currentStrength;
        private StrengthVisualizer _visualizer;
        private bool _died = false;
        [SerializeField] private ParticleSystem smokeFx;

        public void Initialize(CarConfig config, StrengthVisualizer visualizer)
        {
            var strength = config.carStats.stats.Find(x => x.name == "Strength");
            _startStrength = strength.parameter;
            _currentStrength = _startStrength;
            OnCollided += CheckStrength;
            _visualizer = visualizer;
            smokeFx.transform.SetParent(transform);
        }
        
        private void CheckStrength(Collision collision)
        {
            if (collision.collider != null)
            {
                float collisionForce = collision.impulse.magnitude;
                var impulse = collisionForce * 0.01f;
                _currentStrength -= impulse;
                if (_died)
                {
                    return;
                }
                if (_currentStrength<=0)
                {
                    OnDie?.Invoke(this);
                    _died = true;
                    return;
                }
                var damage = _currentStrength / _startStrength;
                if (damage<0.5f)
                {
                    smokeFx.Play();
                }
                _visualizer.ShowDamage(damage);
                
            }
        }
    }
}
