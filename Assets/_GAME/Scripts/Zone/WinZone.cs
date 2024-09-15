using System;
using _GAME.Scripts.UI;
using _GAME.Scripts.WorldCanvas;
using UnityEngine;

namespace _GAME.Scripts.Zone
{
    public class WinZone : MonoBehaviour
    {
        private ScreenSpace _screenSpace;

        private void Start()
        {
            _screenSpace = FindObjectOfType<ScreenSpace>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PrometeoCarController carController))
            {
                carController.Stop();
                var w = _screenSpace.GetWindow<WinWindow>();
                w.Open();
            }
        }
    }
}
