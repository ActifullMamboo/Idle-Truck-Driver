using System;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME.Scripts.Upgrades
{
    [CreateAssetMenu(fileName = "Data", menuName = "Upgrade", order = 2)]

    public class UpgradableItem : ScriptableObject
    {
        public UpgradeType upgradeType;
        public string upgradeName;

        public List<ItemToUpgrade> itemsToUpgrade;

        public int activeItem = 0;

        public void Init()
        {
            activeItem = PlayerPrefs.GetInt(upgradeType.ToString(), -1);
           var ids =  SaveSystem.LoadCustomizations(upgradeType);
           for (int i = 0; i < ids.Count; i++)
           {
               for (int j = 0; j < itemsToUpgrade.Count; j++)
               {
                   if (ids[i] == itemsToUpgrade[j].ID)
                   {
                       itemsToUpgrade[j].price = 0;
                   }
               }
           }
        }

        public void Upgrade(int num)
        {
            activeItem = num;
            PlayerPrefs.SetInt(upgradeType.ToString(),num);
            itemsToUpgrade[num].price = 0;
            var id = itemsToUpgrade[num].ID;
            SaveSystem.SaveCustomization(id, upgradeType);

        }

        [Serializable]
        public class ItemToUpgrade
        {
            public int ID;
            public int price;
            public Sprite sprite;
        }
    }
    public enum UpgradeType
    {
        SteeringWheel,
        BottomAccessory,
        TopAccessory
    }
}
