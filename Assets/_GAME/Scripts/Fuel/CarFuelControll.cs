using _GAME.Scripts.Car;
using UnityEngine;

namespace _GAME.Scripts.Fuel
{
    public class CarFuelControll : MonoBehaviour
    {
        [SerializeField] private PointView characterFuelPoint;
        [SerializeField] private Transform fuelPoint;
        private PrometeoCarController _carController;

        public PointView GetCarMovePoint()
        {
            return characterFuelPoint;
        }

        public void CarStop()
        {
            _carController.BlockMovement(true);
        }

        public void CarMove()
        {
            _carController.StartMoving();
        }

        public void InitComponent(PrometeoCarController carController)
        {
            _carController = carController;
        }

        public Transform GetGasPoint()
        {
            return fuelPoint;
        }
    }
}