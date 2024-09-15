using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Other;
using _GAME.Scripts.Upgrades;
using UnityEngine;

namespace _GAME.Scripts
{
    public class SaveSystem : MonoBehaviour
    {
        private const string CurrencyKey = "Currency";
        private const string PickedCar = "PickedCar";
        private const string BoughtCars = "BoughtCars";
        private const string CarLevel = "CARLEVEL";
        private const string Customization = "Customization";
        
        

        public static int GetCurrency(CurrencyType type)
        {
            var key = CurrencyKey + type;
            return ES3.Load(key,  0);
        }

        public static void SaveCurrency(CurrencyType type, int count)
        {
            var key = CurrencyKey + type;
            ES3.Save(key, count);
        }

        public static int GetActiveCar()
        {
            var id = ES3.Load(PickedCar, 0);
            return id;
        }

        public static void SaveActiveCar(int id)
        {
            ES3.Save(PickedCar, id);
        }

        public static void SaveBoughtCar(int id)
        {
            var key = BoughtCars + id;
            ES3.Save(key,id);
        }

        public static int GetBoughtCar(int id)
        {
            var key = BoughtCars + id;

            return ES3.Load(key, 0);
        }
        

        public static float GetCarLevelPoints(string activeConfigName)
        {
            var key = CarLevel + activeConfigName;
            return ES3.Load<float>(key, 0);
        }

        public static void SaveCarLevelPoints(string activeConfigName, float points)
        {
            var key = CarLevel + activeConfigName;
            var last = ES3.Load<float>(key,0);
            var newPoints = last + points;

            ES3.Save(key, newPoints);
        }

        public static void SaveCustomization(int num, UpgradeType upgradeType)
        {
            var key = Customization + upgradeType;
            var newList = new List<int>();
            var arrayOfUpgrades = ES3.Load(key, newList);
            if (arrayOfUpgrades.Exists(x=>x != num))
            {
                arrayOfUpgrades.Add(num);

            }

            ES3.Save(key, arrayOfUpgrades);

        }

        public static List<int> LoadCustomizations(UpgradeType upgradeType)
        {
            var key = Customization + upgradeType;
            var newList = new List<int>();
            var arrayOfUpgrades = ES3.Load(key, newList);
            return arrayOfUpgrades;
        }
    }
}