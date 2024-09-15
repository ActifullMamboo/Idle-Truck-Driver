using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _GAME.Scripts.Player
{
    public enum HandsState
    {
        HandsUp,
        HandsDown
    }

    public class CharacterHandsController : MonoBehaviour
    {
        [Serializable]
        public class HandRigController
        {
            public Transform HandTarget;
            public Rig Rig;
            public Rig Rotator;
            public Vector3 Position;
            public Vector3 EulerAngles;

            public Vector3 StartPosition;
            public Vector3 StartEulerAngles;
        }

        private HandsState _handsState = HandsState.HandsDown;
        [SerializeField] private HandRigController[] controllers;

        public void InitializePlayerComponent()
        {
            for (var i = 0; i < controllers.Length; i++)
            {
                controllers[i].StartPosition = controllers[i].HandTarget.localPosition;
                controllers[i].StartEulerAngles = controllers[i].HandTarget.localEulerAngles;
            }
        }
[Button]
        public void HandsUp()
        {
            if (_handsState == HandsState.HandsUp) return;
            _handsState = HandsState.HandsUp;

            var moveTime = 0.5f;
            for (var i = 0; i < controllers.Length; i++)
            {
                var i1 = i;
                controllers[i1].HandTarget.localPosition = controllers[i1].Position;
                controllers[i1].HandTarget.localEulerAngles = controllers[i1].EulerAngles;
                DOVirtual.Float(0, 1, moveTime, value => SetWeight(controllers[i1], value));
            }
        }
        [Button]

        public void HandsDown()
        {
            if (_handsState == HandsState.HandsDown) return;
            _handsState = HandsState.HandsDown;
            var moveTime = 0.5f;

            for (var i = 0; i < controllers.Length; i++)
            {
                var i1 = i;
                DOVirtual.Float(1, 0, moveTime, value => SetWeight(controllers[i1], value));
                controllers[i1].HandTarget.DOLocalMove(controllers[i1].StartPosition, moveTime);
                controllers[i1].HandTarget.DOLocalRotate(controllers[i1].StartEulerAngles, moveTime);
            }
        }

        private void SetWeight(HandRigController controller, float value)
        {
            controller.Rig.weight = value;
            controller.Rotator.weight = value;
        }
    }
}