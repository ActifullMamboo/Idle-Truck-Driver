using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Tasks;
using _Game.Scripts.Ui.Base;
using UnityEngine;

namespace _GAME.Scripts.UI.WorldSpace
{
    public class TaskPanel : BaseWindow
    {
        private List<TaskItem> _taskItems = new();
        private TasksController _tasksController;
        private List<Task> _requireTasks = new();
        [SerializeField] private BaseButton button;

        public override void Init()
        {
            base.Init();
            _taskItems = GetComponentsInChildren<TaskItem>(true).ToList();
            _tasksController = FindObjectOfType<TasksController>();
            foreach (var item in _taskItems)
                item.Init();

            button.SetCallback(()=>Open());
        }

        public override void Open(params object[] list)
        {
            base.Open(list);
            _requireTasks = _tasksController.GetRequireTasks();
            var carLevel = _tasksController.GetCurrentCarLevel();

            for (var i = 0; i < _requireTasks.Count; i++)
            {
                _taskItems[i].OnTaskConfirmed += _tasksController.StartTask;
                _taskItems[i].SetupTask(_requireTasks[i], carLevel);
            }
        }
    }
}