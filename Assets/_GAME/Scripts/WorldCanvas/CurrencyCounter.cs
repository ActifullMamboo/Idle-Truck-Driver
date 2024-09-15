using _Game.Scripts.Ui.Base;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.WorldCanvas
{
    public class CurrencyCounter : BaseWindow, ICurrencyComponent
    {
        [SerializeField] private TextMeshProUGUI text;
        protected TextMeshProUGUI Text => text;

        private void SetText(string txt, float progress = 0)
        {
            text.text = txt;
        }
        public override void Init()
        {
            base.Init();
            base.Open();
        }
        
        public void SetCurrencySystem(CurrensySystem currensySystem)
        {
            currensySystem.UpdateCurrency += UpdateCurency;
            SetText(currensySystem.GetSoft());
        }
        
        private void UpdateCurency(int arg1, int arg2)
        {
            SetText(arg1.ToString());
        }
    }
}
