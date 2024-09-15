using System;
using _GAME.Scripts.AI.Base;
using _GAME.Scripts.Car;
using _GAME.Scripts.Components;
using _GAME.Scripts.Player;
using _GAME.Scripts.UI;
using _Game.Scripts.Ui.Base;
using _Game.Scripts.View;
using _GAME.Scripts.WorldCanvas;
using DG.Tweening;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace _GAME.Scripts.Fuel
{
    public class GasStation : MonoBehaviour, IScreenSpaceClaimer, IComponentInitializer
    {
        [SerializeField] private Transform dynamicPoint;
        [SerializeField] private Transform dynamicPointStartPosition;
        [SerializeField] private PointView standPoint;
        [SerializeField] private BaseAI character;
    
        [SerializeField] private Transform characterHandPoint;
        [SerializeField] private CollisionListener pickPoint;
    
        private CarFuelControll _carFuelController;
        private CameraController _cameraController;
        private ScreenSpace _screenSpace;
        private FuelController _fuelController;
        private Transform _carFuelPoint;
        private CharacterHandsController _handsController;
        private CalculatePath _navigator;
        private PointView _target;
        public void ClaimScreenSpaceCanvas(ScreenSpace screenSpace)
        {
            _screenSpace = screenSpace;
            _fuelController = _screenSpace.GetComponentInChildren<FuelController>(true);
        }

        public void Initialize()
        {
            _cameraController = FindObjectOfType<CameraController>();
            dynamicPoint.SetParent(dynamicPointStartPosition);
            dynamicPoint.localPosition = Vector3.zero;
            pickPoint.OnEnter += StartFuelRoutine;
            _handsController = character.GetComponent<CharacterHandsController>();
            _handsController.InitializePlayerComponent();
            _navigator = FindObjectOfType<CalculatePath>(true);
            character.Init();
            character.SetStopDistance(1f);
            character.ForceMoveTo(standPoint.transform); 

        }

        private void Update()
        {
            if (_target==null)
            {
                return;
            }
            character.SetTarget(_target.transform);
            character.UpdateMovement(Time.deltaTime);

        }

        private void StartFuelRoutine(Collider obj)
        {
            if (obj.TryGetComponent(out CarFuelControll fuelController))
            {
                _carFuelController = fuelController;
                fuelController.CarStop();

                _navigator.DisableNavigation();
                DOVirtual.DelayedCall(1f, delegate { StartWorking(fuelController); });
            }
        }

        private void StartWorking(CarFuelControll fuelController)
        {
            _handsController.HandsUp();

            SetDynamicPointPosition(characterHandPoint);

            _carFuelPoint = fuelController.GetGasPoint();
            var point = fuelController.GetCarMovePoint();
            _cameraController.SetActiveFuelCamera(true);
            _target = point;
            character.GoTo(OnEndMove,point);
            character.SetSpeed(1);

        }

        private void OnEndMove()
        {
            character.SetSpeed(0);
            _target = null;
            _handsController.HandsDown();
            SetDynamicPointPosition(_carFuelPoint);
            var w = _screenSpace.GetWindow<FuelWindow>() as FuelWindow;
            if (w == null) return;
            
            w.Open(_fuelController);
            w.Closed += CharacterBackMove;
        }

        private void CharacterBackMove(BaseWindow window)
        {
            window.Closed -= CharacterBackMove;
            _handsController.HandsUp();

            SetDynamicPointPosition(characterHandPoint);
            _target = standPoint;
            character.GoTo(OnEndProcess,standPoint);
            character.SetSpeed(1);

            _cameraController.SetActiveFuelCamera(false);
            _carFuelController.CarMove();
            _navigator.ShowNavigation();
        }

        private void OnEndProcess()
        {
            character.SetSpeed(0);
            _target = null;

            _handsController.HandsDown();
            SetDynamicPointPosition(dynamicPointStartPosition);

        }

        private void SetDynamicPointPosition(Transform parent)
        {
            dynamicPoint.SetParent(parent);
            dynamicPoint.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.InSine);
        }
    }
}
