using _Game.Scripts.Ui.Base;
using _Game.Scripts.View;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace _GAME.Scripts.UI.WorldSpace
{
    public class WorldSpace : BaseView
    {
        private List<TaskPanel> taskPanels = new();
        

        public override void Init(params object[] list)
        {
            taskPanels = GetComponentsInChildren<TaskPanel>(true).ToList();
            
            for (int i = 0; i < taskPanels.Count; i++)
            {
                taskPanels[i].Init();
            }
        
            base.Init();
        }

        public TaskPanel GetAvailableTaskPanel()
        {
            return taskPanels.FirstOrDefault(p => !p.IsOpened);
        }

    }
}
