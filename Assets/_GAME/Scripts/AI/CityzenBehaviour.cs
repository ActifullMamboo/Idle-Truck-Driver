using System;
using _GAME.Scripts.AI.Base;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME.Scripts.AI
{
    public class CityzenBehaviour : BaseAI
    {
        public Action<CityzenBehaviour> OnCycleEnd;

        private enum LogicState
        {
            None,
            Spawn,
            Moving,
            Stop
        }

        private PointView _destinationPoint;
        private PointView _spawnPoint;
        private LogicState _state;
        public float speed;
        public override void Init(params object[] list)
        {
            base.Init();
            SetSpeed(speed);

            if (list.Length == 0) return;
            _state = LogicState.Spawn;
            _spawnPoint = (PointView)list[0];
            _destinationPoint = (PointView)list[1];
            RunNextStep();
        }

        private readonly Vector3[] _directions = { Vector3.right, Vector3.left, Vector3.zero };
        public LayerMask mask;

        private void CheckObstacles()
        {
            var ray = new Ray(transform.position + Vector3.up,
                transform.forward*3 + _directions[Random.Range(0, _directions.Length)] * 0.15f);

            Debug.DrawRay(ray.origin,ray.direction*3, Color.green);
            if (Physics.Raycast(ray.origin, ray.direction,
                    out var hit, 2f, mask))
            {
                if (_state == LogicState.Stop) return;

                SetSpeed(0);
                _waitTime = 1;
                _state = LogicState.Stop;
            }
            else
            {
                if (_state != LogicState.Stop) return;
                _waitTime -= Time.deltaTime;
                if (_waitTime<=0)
                {
                    SetSpeed(speed);
                    _state = LogicState.Moving;
                }
               
            }
        }

        private float _waitTime = 1f;
        public override void UpdateMovement(float deltaTime)
        {
            base.UpdateMovement(deltaTime);

            CheckObstacles();
        }

        public override void RunNextStep()
        {
            switch (_state)
            {
                case LogicState.Spawn:

                    ForceMoveTo(_spawnPoint.transform);
                    _state = LogicState.Moving;
                    RunNextStep();          
                    break;

                case LogicState.Moving:

                    GoTo(OnDestinationPoint, _destinationPoint);

                    break;
            }
        }

        private void OnDestinationPoint()
        {
            SmoothHide(1f);
            DOVirtual.DelayedCall(1f, OnStopMoveToPoint);
        }

        private void OnStopMoveToPoint()
        {
            Reset();
            OnCycleEnd.Invoke(this);
        }

        public void BreaksMovement()
        {
            SetSpeed(0);
            SetNavMeshAgentFlag(false);
        }
    }
}