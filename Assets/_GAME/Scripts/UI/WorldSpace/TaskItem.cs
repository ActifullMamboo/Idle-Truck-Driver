using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Tasks;
using _Game.Scripts.Ui.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts.UI.WorldSpace
{
    public class TaskItem : BaseUIView
    {
        public Action<Task> OnTaskConfirmed;

        [SerializeField] private TextMeshProUGUI requireLevelText;
        [SerializeField] private TextMeshProUGUI Tasktext;
        [SerializeField] private TextMeshProUGUI distanceText;

        [SerializeField] private BaseButton claimButton;
        private Task _task;
        private List<RewardItem> _rewardItems = new();

        public void Init()
        {
            claimButton.SetCallback(() =>
                OnTaskConfirmed?.Invoke(_task));
            _rewardItems = GetComponentsInChildren<RewardItem>().ToList();
        }
        
        public void SetupTask(Task requireTask, int carLevel)
        {
            _task = requireTask;
            var dist ="Distance: " +$"<color=#FFCF69>{requireTask.distance}</color> KM";
            var id = transform.GetSiblingIndex();
            var taskName ="Task "+$"{id+1}";
            var lvl = carLevel + 1;
            distanceText.SetText(dist);
            Tasktext.SetText(taskName);
            
            if (lvl < requireTask.requireLevel)
            {
               // requireLevelText.text = $"Require level:  <color=red>{requireTask.requireLevel}</color>";
                claimButton.SetInteractable(false);
            }
            else
            {
               // requireLevelText.text = $"Require level:  <color=green>{requireTask.requireLevel}</color>";
            }

            for (var i = 0; i < _rewardItems.Count; i++) _rewardItems[i].SetupReward(requireTask.Rewards[i]);
        }
    }
}