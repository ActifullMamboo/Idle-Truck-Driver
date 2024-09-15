using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Configs;
using _GAME.Scripts.Load;
using UnityEngine;

namespace _GAME.Scripts.Tasks
{
    public class TasksController : MonoBehaviour
    {
        [SerializeField] private List<Task> tasks;
        [SerializeField] private List<CarConfig> carConfigs;

        public static Task activeTask;
        public List<Task> GetRequireTasks()
        {
            if (activeTask==null)
            {
                return tasks.GetRange(0, 4);
            }
            var tsks = tasks.FindAll(x => x != activeTask);
            return tsks.GetRange(0, 4);
        }

        public int GetCurrentCarLevel()
        {
            var carId = SaveSystem.GetActiveCar();
            return carConfigs[carId].currentLevel;
        }

        public void StartTask(Task task)
        {
            activeTask = task;
            LoadingManager.LoadScene(LoadingType.Game);
        }
    }
}
