using System;
using System.Collections.Generic;
using _Game.Scripts.Tools;
using _GAME.Scripts.UI.CarPick;
using _GAME.Scripts.WorldCanvas;
using UnityEngine;

namespace _GAME.Scripts.Car
{
    public class GaragePickZone : MonoBehaviour, IScreenSpaceClaimer
    {
        [SerializeField] private List<Transform> cars;
        [SerializeField] private List<Rigidbody> carsB;
        public float force;
        public void ClaimScreenSpaceCanvas(ScreenSpace screenSpace)
        {
            var carStatsWindow = screenSpace.GetWindow<CarStatsWindow>() as CarStatsWindow;
            if (carStatsWindow != null) carStatsWindow.OnListed += SetupCarView;
        }

        private void SetupCarView(int obj)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].Deactivate();
            }

            cars[obj].Activate();
            carsB[obj].AddForce(Vector3.up*force);

        }
    }
}
