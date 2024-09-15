using System;
using System.Collections.Generic;
using _GAME.Scripts.Base;
using UnityEngine;

namespace _GAME.Scripts.Tasks
{
    [Serializable]
    public class Task
    {
        public Action OnTaskComplete;
        
        
        public int TaskID;
        public int MapID;
        
        public int requireLevel;
        public float distance;
        
        public List<Reward> Rewards;
    }
    
    [Serializable]
    public class Reward
    {
        public int RewardAmount;
        public string RewardText;
        public Sprite RewardSprite;
    }
}