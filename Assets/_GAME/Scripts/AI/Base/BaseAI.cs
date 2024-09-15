using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Base;
using _GAME.Scripts.Player;
using _Game.Scripts.Tools;
using _Game.Scripts.View;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace _GAME.Scripts.AI.Base
{
    public enum CharacterState
    {
        Idle,
        Moving,
        Running
        
    }

    public enum CharacterType
    {
        Customer,
        Helper,
        Car
    }

    public class BaseAI : BaseView
    {
        [SerializeField] private AnimationsController _animationsController;
        [SerializeField] [Unity.Collections.ReadOnly] private VisualAIConfig ViewConfig;

        private NavMeshAgent _navMeshAgent;
        
        protected PointView TargetPoint;
        protected PointView CurrentPoint;
        private Renderer _skinnedMesh;
        
        public override void Init(params object[] list)
        {
            
            base.Init();
            _navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            if (ViewConfig.characterType==CharacterType.Customer)
            {
                _animationsController.Start();
                _skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
                _skinnedMesh.material = ViewConfig.mainMat;
                var m = _skinnedMesh as SkinnedMeshRenderer; 
                m.sharedMesh = ViewConfig.Mesh.RandomValue();
            }
            else if (ViewConfig.characterType==CharacterType.Car)
            {
                for (int i = 0; i < ViewConfig.cars.Length; i++)
                {
                    ViewConfig.cars[i].Deactivate();
                }

                var rend = GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < rend.Length; i++)
                {
                    rend[i].material = ViewConfig.mainMat;
                    
                }
                var c = ViewConfig.cars.RandomValue();
                c.Activate();

            }

            
            STATES = CharacterState.Idle;
            SetStopDistance(0.1f);
        }

        private void FreePoints()
        {
            if (CurrentPoint != null) CurrentPoint.FreePoint();
            if (TargetPoint != null) TargetPoint.FreePoint();
        }
        public override void Reset()
        {
            _callback = null;
            FreePoints();
            TargetPoint = null;
            CurrentPoint = null;
            Stop();
           

            if (ViewConfig.characterType == CharacterType.Car)
            {
                var rend = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < rend.Length; i++)
                {
                    rend[i].material.DOFade(1, .01f);
                    
                }
            }
            else
            {
                _skinnedMesh.material.DOFade(1, 0.01f);

            }

            base.Reset();
        }
        #region Moving

        protected float _stoppingDistance = 0.01f;
        private const float _rotationSpeed = 3600f;
        private Action _callback;
        private CharacterState STATES;
        private float _speed;

        public void SetStopDistance(float stopDistance, bool defaultValue = false)
        {
            if (defaultValue)
            {
                _stoppingDistance = 0.01f;
                return;
            }

            _stoppingDistance = stopDistance;
        }

        public virtual void GoTo(Action callback, PointView targetPoint, CharacterState state = CharacterState.Moving)
        {
            if (targetPoint == null)
            {
                Debug.LogError("Target is null");
                return;
            }
            SetState(state);
            
            _callback = callback;
            TargetPoint = targetPoint;
            TargetPoint.SetReserved(true);

            SetNavMeshAgentFlag(true);
            SetNavMeshDestination(targetPoint.transform.position);
            _navMeshAgent.speed = _speed;

        }

        public virtual void Stop()
        {
            SetState(CharacterState.Idle);
            if (_navMeshAgent.enabled) _navMeshAgent.ResetPath();
            SetNavMeshAgentFlag(false);

            if (TargetPoint)
            {
                TargetPoint.SetItem(this);
                CurrentPoint = TargetPoint;
                SetRotation(CurrentPoint.transform.rotation);
                if (_animationsController)
                {
                    _animationsController.Animate(TargetPoint.AnimationHash);

                }
            }

            SetPosition(_navMeshAgent.transform.position);

            var action = _callback;
            _callback = null;
            _navMeshAgent.speed = 0;
            action?.Invoke(); 

        }

        public void ForceMoveTo(Transform target = default)
        {
            var navAgentEnabled = _navMeshAgent.enabled;
            SetNavMeshAgentFlag(false);
            if (target != null) transform.SetPositionAndRotation(target.position, target.rotation);
            SetNavMeshAgentFlag(navAgentEnabled);
        }
        public virtual void RunNextStep()
        {
        }

        protected void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void ChangePosition(Vector3 offset)
        {
            transform.position += offset;
        }

        protected void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetNavMeshAgentFlag(bool flag)
        {
            _navMeshAgent.enabled = flag;
        }

        protected void SetNavMeshDestination(Vector3 point)
        {
            _navMeshAgent.SetDestination(point);
        }

        private void SetState(CharacterState state)
        {
            STATES = state;
            var id = (int)state;
            if (ViewConfig.characterType!=CharacterType.Car)
            {
                _animationsController.SetSpeed(id);
            }
            
        }

        public void SetTarget(Transform t)
        {
            _navMeshAgent.SetDestination(t.position);
        }
        public virtual void UpdateMovement(float deltaTime)
        {
            if (STATES == CharacterState.Idle) return;
            var navMeh = _navMeshAgent;
            var currentDistance = Vector3.Distance(navMeh.transform.position, navMeh.destination);
            if (currentDistance < _stoppingDistance)
            {
                Stop();
            }

            var offset = (navMeh.steeringTarget - transform.position).normalized * (navMeh.speed * deltaTime);
            if (offset.magnitude >= currentDistance) return;

            ChangePosition(offset);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
            _navMeshAgent.speed = speed;
            if (speed==0)
            {
                if (ViewConfig.characterType != CharacterType.Car)
                {
                    _animationsController.SetSpeed(0);
                }
            }
            else
            {
                if (ViewConfig.characterType != CharacterType.Car)
                {
                    _animationsController.SetSpeed(1);
                }
            }

        }
        public void UpdateRotation(float deltaTime)
        {
            if (STATES == CharacterState.Idle) return;
            var navMeshAgent = _navMeshAgent;
            if (navMeshAgent.enabled == false) return;
            if (Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.steeringTarget) > 0.01f)
            {
                var target = navMeshAgent.steeringTarget - navMeshAgent.transform.position;
                navMeshAgent.transform.rotation = Quaternion.Slerp(
                    navMeshAgent.transform.rotation,
                    Quaternion.LookRotation(target),
                    _rotationSpeed * deltaTime);
            }
        }

        #endregion
        public void SmoothHide(float f)
        {
            if (ViewConfig.characterType==CharacterType.Car)
            {
                var rend = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < rend.Length; i++)
                {
                    rend[i].material = ViewConfig.hideMat;
                    rend[i].material.DOFade(0, f);
                    
                }
            }
            else
            {
                _skinnedMesh.material = ViewConfig.hideMat;
                _skinnedMesh.material.DOFade(0, f);
            }
          
        }
    }

    [Serializable]
    public class VisualAIConfig
    {
        public Material mainMat;
        public Material hideMat;
        public CharacterType characterType;
        [ShowIf("characterType", CharacterType.Customer)]
        public Mesh[] Mesh;

        [ShowIf("characterType", CharacterType.Car)]
        public GameObject[] cars;

    }
    
}