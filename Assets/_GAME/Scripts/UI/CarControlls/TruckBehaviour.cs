using _GAME.Scripts.Car;
using _GAME.Scripts.Car.TruckStrength;
using _GAME.Scripts.Configs;
using _GAME.Scripts.Fuel;
using _Game.Scripts.Tools;
using _Game.Scripts.View;
using _GAME.Scripts.WorldCanvas;
using UnityEngine;

namespace _GAME.Scripts.UI.CarControlls
{
    public class TruckBehaviour : MonoBehaviour
    {
        [SerializeField] private PrometeoCarController carController;
        [SerializeField] private TruckPanelController panelController;
        [SerializeField] private Transform navigationHolder;
        [SerializeField] private CarConfig carConfig;
        public CarConfig CarConfig => carConfig;
        private TruckStrengthController _truckStrengthController;
        private ScreenSpace _screenSpace;

        public Transform GetCarParent()
        {
            return carController.transform;
        }
        public void Init( params object[] objects)
        {
            var claimer = objects[0] as Transform;
            _screenSpace = objects[1] as ScreenSpace;
            var visualizer = _screenSpace.GetWindow<StrengthVisualizer>() as StrengthVisualizer;
            var fuelWindow = _screenSpace.GetWindow<FuelController>() as FuelController;

            claimer.SetParent(navigationHolder);
            claimer.localPosition=Vector3.zero;
            gameObject.Activate();
            panelController.Activate();
            carController.Init();
            _truckStrengthController = GetComponentInChildren<TruckStrengthController>();
            _truckStrengthController.Initialize(CarConfig, visualizer);
            _truckStrengthController.OnDie += OnDie;

            var fuelController = GetComponentInChildren<CarFuelControll>();
            fuelController.InitComponent(carController);
            fuelWindow.OnEmpty += OnEmpty;
        }

        private void OnEmpty(FuelController obj)
        {
            obj.OnEmpty -= OnEmpty;
            var window = _screenSpace.GetWindow<LooseWindow>();
            window.Open(0);
        }

        private void OnDie(TruckStrengthController obj)
        {
            obj.OnDie -= OnDie;
            carController.BlockMovement();
            var window = _screenSpace.GetWindow<LooseWindow>();
            window.Open(1);
        }

        public void MoveDirection()
        {
            carController.MoveForward();
        }

        public void Release()
        {
            carController.Stop();
        }

        public void ChangeControls(bool forward)
        {
            carController.ChangeControlls(forward);
            panelController.ChangeControls(forward);
        }

        public void Rotate(SimpleInput.AxisInput arg1, RectTransform arg2)
        {
            carController.Turn(arg1.value);
            panelController.WheelRotate(arg2);
        }

        public void BlockMovement(FuelController obj)
        {
            obj.OnEmpty -= BlockMovement;
            carController.BlockMovement();
        }
    }
}
