using System;
using UnityEngine;

namespace _GAME.Scripts.PathSimulator
{
    public class TruckPathProjectile : MonoBehaviour
    {
        [SerializeField] private Transform forwardPoint;

        public void Simulate(SimpleInput.AxisInput arg1, RectTransform arg2)
        {
            var x = arg1.value * 3f;
            x = Mathf.Clamp(x, -3, 3);
            var localPosition = forwardPoint.localPosition;
            localPosition = new Vector3(x, localPosition.y, localPosition.z);
            forwardPoint.localPosition = localPosition;
        }
    }
}
