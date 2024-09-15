using System;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME.Scripts.Configs
{
    [CreateAssetMenu(fileName = "Data", menuName = "CarConfig", order = 1)]
   
    public class CarConfig : ScriptableObject
    {
        
        public Action OnUpgrade;

        [Header("STATS"), Space] public CarStats carStats;

        [Header("PRICE"), Space]
        public int price;
        
        
        [Header("Progress"), Space]       
        public int currentLevel;
        [Space]
        public List<int> pointsToNextLevel;
        [HideInInspector] public int CurrentPointsToNextLevel => pointsToNextLevel[currentLevel];

        
        public void Init()
        {
            currentLevel = PlayerPrefs.GetInt(name, 0);
        }

        public void Upgrade()
        {
            currentLevel++;
            PlayerPrefs.SetInt(name,currentLevel);
            OnUpgrade?.Invoke();
        }
    }
    [Serializable]
    public class CarStats
    {
        public List<Stats> stats;
    }
    [Serializable]
    public class Stats
    {
        public string name;
        public int parameter;
        [Range(0,1)]
        public float value;
    }
}
