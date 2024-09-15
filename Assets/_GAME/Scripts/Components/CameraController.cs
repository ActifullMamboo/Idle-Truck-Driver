using _Game.Scripts.Tools;
using Cinemachine;
using UnityEngine;

namespace _GAME.Scripts.Components
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera fuelCamera;

        public void SetActiveFuelCamera(bool state)
        {
            fuelCamera.SetActive(state);
        }
    }
}
