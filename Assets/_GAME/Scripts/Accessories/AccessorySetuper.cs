using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Components;
using _Game.Scripts.Tools;
using _GAME.Scripts.Upgrades;
using UnityEngine;

namespace _GAME.Scripts.Accessories
{
    public class AccessorySetuper : MonoBehaviour,IComponentInitializer
    {
        [SerializeField] private UpgradableItem item;
        private List<BaseAccessory> _accessories = new List<BaseAccessory>();
        public void Initialize()
        {
            _accessories = GetComponentsInChildren<BaseAccessory>(true).ToList();

            var activeItem = item.activeItem;
            if (activeItem<0)
            {
                return;
            }
            if (item.upgradeType == UpgradeType.SteeringWheel)
            {
                _accessories[activeItem].Deactivate();
                activeItem += 1;
            }
            _accessories[activeItem].StartSetup();
        }

        public void ResetAll()
        {
            for (int i = 0; i < _accessories.Count; i++)
            {
                _accessories[i].Deactivate();
            }
            var activeItem = item.activeItem;

            if (item.upgradeType == UpgradeType.SteeringWheel)
            {
                activeItem += 1;
            }
            if (activeItem<0)
            {
                return;
            }
            _accessories[activeItem].StartSetup();

        }


        public void SetView(UpgradeType type, int id)
        {
            if (item.upgradeType!=type)
            {
                return;
            }
            
            if (item.upgradeType == UpgradeType.SteeringWheel)
            {
                id += 1;
            }

            for (int i = 0; i < _accessories.Count; i++)
            {
                _accessories[i].Deactivate();
            }

            _accessories[id].Activate();
        }
    }
}
