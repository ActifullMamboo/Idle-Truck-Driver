using System;
using System.Collections.Generic;
using _GAME.Scripts.Components;
using _Game.Scripts.Tools;
using InsaneSystems.RoadNavigator;
using UnityEngine;

namespace _GAME.Scripts.Tasks
{
    public class TaskClaimer : MonoBehaviour, ILevelStartComponents
    {
        [SerializeField] private List<TaskOnLevel> tasksOnLevel;
        [SerializeField] private Navigator calculatePath;

        public Task _task;
        public void Initialize()
        {
            //_task = TasksController.activeTask;
            var current = tasksOnLevel.Find(x => x.id == _task.TaskID);
            for (int i = 0; i < tasksOnLevel.Count; i++)
            {
                tasksOnLevel[i].destination.Deactivate();
            }

            current.destination.Activate();
            calculatePath.SetTarget(current.destination);
        }
       
    }
    [Serializable]
    public class TaskOnLevel
    {
        public int id;
        public Transform destination;
    }
}
