using _GAME.Scripts.Base;
using _GAME.Scripts.Pools;
using _GAME.Scripts.UI.MoveTargets;
using _Game.Scripts.View;
using _GAME.Scripts.WorldCanvas;
using UnityEngine;

namespace _GAME.Scripts.Progress
{
    public class ProgressParticleController : BaseView, IScreenSpaceClaimer, IPoolClaimer
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.Particle[] _particles;
        private Transform _moveObject;
        private PoolHandler _poolHandler;
        public UITargetType targetType;
        public bool UI = false;

        public override void Init(params object[] list)
        {
            _particleSystem = GetComponent<ParticleSystem>();
           
            _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        }

        private void Update()
        {
            var numParticlesAlive = _particleSystem.GetParticles(_particles);
            if (numParticlesAlive <= 0) return;
           
           
            for (var i = 0; i < numParticlesAlive; i++)
                if (_particles[i].remainingLifetime <= 0.02f)
                {
                    var particlePosition = transform.TransformPoint(_particles[i].position);
                    var p = _poolHandler.GetPool<UIVisualItemsPool>() as UIVisualItemsPool;
                    if (p != null)
                    {
                        var star = p.GetItem(targetType);
                        var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, particlePosition);


                        if ( UI)
                        {

                            var thisPos = transform.position;
                            var reset = thisPos - _particles[i].position;
                            var needPos = reset * 450;
                            screenPosition = thisPos - needPos;

                        }


                        if (star != null)
                        {
                            star.SetPosition(screenPosition);
                           // star.SetRotation(_particles[i].rotation);
                            star.Move(_moveObject, () => p.DeSpawn(star));
                        }
                    }
                }
        }

        public void Play()
        {
            _particleSystem.Play();
        }

        private ScreenSpace _screenSpace;

        public void ClaimScreenSpaceCanvas(ScreenSpace screenSpace)
        {
            _screenSpace = screenSpace;
            _moveObject = _screenSpace.GetMoveParentForParticles(targetType);
        }

        public void GetPool(PoolHandler pool)
        {
            _poolHandler = pool;
        }

        public float GetLifeTime()
        {
            return _particleSystem.main.duration;
        }
    }
}