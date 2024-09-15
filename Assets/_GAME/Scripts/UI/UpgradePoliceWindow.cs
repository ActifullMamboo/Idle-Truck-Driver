using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Garage;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using _GAME.Scripts.Upgrades;
using UnityEngine;

namespace _GAME.Scripts.UI
{
    public class UpgradePoliceWindow : BaseWindow
    {
        [SerializeField] private List<UpgradeWindow> windows;
        [SerializeField] private BaseButton openedButton;
        [SerializeField] private BaseButton closeButton;
        [SerializeField] private GameObject carWindow;

        private List<ChooseWindowButton> _buttons = new();
        private int _roomId;


        public override void Init()
        {
            base.Init();
            _buttons = GetComponentsInChildren<ChooseWindowButton>().ToList();
            var picker = FindObjectOfType<GarageAccessoryPicker>(true);
            var zone = FindObjectOfType<GarageAccessoryZone>(true);
            for (var i = 0; i < _buttons.Count; i++)
            {
                var i1 = i;
                _buttons[i].SetCallback(()=>
                {
                    OpenWindowViaButton(_buttons[i1]);
                    picker.ResetView();
                });
            }

            openedButton.SetCallback(() =>
            {
                Open(UpgradeType.SteeringWheel);
                openedButton.Deactivate();
                carWindow.Deactivate();
            });
            closeButton.SetCallback(() =>
            {
                Close();
                openedButton.Activate();
                zone.Deactivate(); 
                carWindow.Activate();
            });
            var spB = (AccessoryButton)openedButton;
            spB.Init();
        }

        private void OpenWindowViaButton(ChooseWindowButton button)
        {
            RedrawAllButtons();
            for (var i = 0; i < _buttons.Count; i++) _buttons[i].ScaleOut();
            button.ScaleIn();
            for (var i = 0; i < windows.Count; i++) windows[i].Close();
            var window = windows.FirstOrDefault(item => item.upgradeType == button.upgradeType);
            if (window == null) return;
            window.Open();
        }

        private void RedrawAllButtons()
        {
            for (var i = 0; i < _buttons.Count; i++) _buttons[i].SetInteractable(true);
        }

        public override void Open(params object[] list)
        {
            DeactivateAllWindow();

            var upgradeType = (UpgradeType)list[0];
            var window = windows.FirstOrDefault(item => item.upgradeType == upgradeType);
            var b = _buttons.Find(x => x.upgradeType == upgradeType);
            if (b != null) b.SetInteractable(false);
            if (window == null) return;
            window.Open();
            base.Open(list);
        }

        public override void Close()
        {
            DeactivateAllWindow();
            RedrawAllButtons();
            base.Close();
        }


        private void DeactivateAllWindow()
        {
            foreach (var windowConfig in windows) windowConfig.Deactivate();
        }
    }
}