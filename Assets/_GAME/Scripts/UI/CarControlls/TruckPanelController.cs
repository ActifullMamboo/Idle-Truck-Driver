using System;
using _GAME.Scripts.Car;
using UnityEngine;

namespace _GAME.Scripts.UI.CarControlls
{
   public class TruckPanelController : MonoBehaviour
   {
      [SerializeField] private WheelRotation wheelRotation;
      [SerializeField] private Transmission transmission;

      public void WheelRotate(RectTransform rectTransform)
      {
         wheelRotation.SetRotation(rectTransform);
      }

      public void ChangeControls(bool b)
      {
         transmission.ChangeControls(b);
      }
   }
}
