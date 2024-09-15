using System;
using _Game.Scripts.Ui.Base;
using UnityEngine;

namespace _GAME.Scripts.UI.WorldSpace
{
    public class WinPanel : BaseWindow
    {
        
        private BaseButton button;
        private Action _callback;

        public override void Init()
        {
            button = GetComponentInChildren<BaseButton>();
            button.SetCallback(OnClick);
            Close();
            base.Init();
        }
        
        private void OnClick()
        {
            _callback?.Invoke();
            _callback = null;
            Close();
        }
        public void SetCallback(Action action)
        {
            _callback = action;
        }
    }
}
