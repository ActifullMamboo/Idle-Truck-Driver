using _GAME.Scripts.Load;
using _GAME.Scripts.Other;
using _Game.Scripts.Ui.Base;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.UI
{
    public class LooseWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI looseText;
        [SerializeField] private BaseButton button;
        [SerializeField] private BaseButton mainManu;
        public override void Init()
        {
            base.Init();
            button.SetCallback(Restart);
            mainManu.SetCallback(MainMenu);
        }

        private void MainMenu()
        {
            LoadingManager.LoadScene(LoadingType.Garage);
        }

        private void Restart()
        {
            LoadingManager.LoadScene(LoadingType.Game);
        }

        public override void Open(params object[] list)
        {
            base.Open(list);
            int dieId = (int)list[0];
            if (dieId==0)
            {
                looseText.text = "Out of fuel";
            }
            else
            {
                looseText.text = "Truck is broken";
            }
        }
    }
}
