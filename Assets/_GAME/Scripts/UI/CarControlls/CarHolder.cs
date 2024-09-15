using System.Collections.Generic;
using _GAME.Scripts.Components;
using _Game.Scripts.Tools;
using _GAME.Scripts.WorldCanvas;
using UnityEngine;

namespace _GAME.Scripts.UI.CarControlls
{
   public class CarHolder : MonoBehaviour, IComponentInitializer
   {
      [SerializeField] private List<TruckBehaviour> behaviours;
      [SerializeField] private ControlPanel controlPanel;
      [SerializeField] private CalculatePath calculatePath;
      [SerializeField] private ScreenSpace screenSpace;


      public void Initialize()
      {
         int activeCar = SaveSystem.GetActiveCar();

         for (int i = 0; i < behaviours.Count; i++)
         {
            behaviours[i].Deactivate();
         }

         var beh = behaviours[activeCar];
         beh.Init(calculatePath.transform,screenSpace);
         controlPanel.SetupControlsForCar(beh);
      }
   }
}
