using _GAME.Scripts.Car;
using _GAME.Scripts.PathSimulator;
using _Game.Scripts.Tools;
using SimpleInputNamespace;
using UnityEngine;

namespace _GAME.Scripts.UI.CarControlls
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private SteeringWheel steeringWheel;
        [SerializeField] private TransmissionController transmissionController;
        [SerializeField] private TruckPathProjectile forwardPath;
        [SerializeField] private TruckPathProjectile backwardPath;
        [SerializeField] private FuelController fuelController;

        public void SetupControlsForCar(TruckBehaviour beh)
        {
            steeringWheel.Init();
            transmissionController.Init();
            fuelController.Initialize(beh);
            steeringWheel.OnPress += beh.MoveDirection;
            steeringWheel.OnMove += beh.Rotate;
            steeringWheel.OnMove += fuelController.DecreaseFuel;
            steeringWheel.OnPress += fuelController.StartDecrease;
            steeringWheel.OnRelease += fuelController.StopDecrease;
            steeringWheel.OnMove += forwardPath.Simulate;
            steeringWheel.OnMove += backwardPath.Simulate;
            steeringWheel.OnRelease += beh.Release;
            fuelController.OnEmpty += beh.BlockMovement;
            transmissionController.OnPress += beh.ChangeControls;
            PathSimulator(beh);
        }

        private void PathSimulator(TruckBehaviour beh)
        {
            forwardPath.SetParent(beh.GetCarParent());
            forwardPath.transform.localPosition = new Vector3(0, 0.45f, 2);
            backwardPath.SetParent(beh.GetCarParent());
            backwardPath.Deactivate();
            transmissionController.OnPress += SwitchProjectile;
        }

        private void SwitchProjectile(bool obj)
        {
            forwardPath.SetActive(obj);
            backwardPath.SetActive(!obj);
        }
    }
}
