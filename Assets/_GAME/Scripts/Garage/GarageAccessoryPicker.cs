using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Accessories;
using _GAME.Scripts.Components;
using _GAME.Scripts.Upgrades;
using UnityEngine;

namespace _GAME.Scripts.Garage
{
    public class GarageAccessoryPicker : MonoBehaviour, IComponentInitializer
    {
        public Action<UpgradeType, int> OnChangeView;

        private List<AccessorySetuper> _setupers = new();

        public void Initialize()
        {
            _setupers = GetComponentsInChildren<AccessorySetuper>().ToList();
            for (var i = 0; i < _setupers.Count; i++) OnChangeView += _setupers[i].SetView;
        }

        public void ResetView()
        {
            for (var i = 0; i < _setupers.Count; i++)
            {
                _setupers[i].ResetAll();
            }
        }
    }
}