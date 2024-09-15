using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using UnityEngine;

namespace _Game.Scripts.Ui.Buttons
{
    public class ProgressButton : BaseButton
    {
        [SerializeField] private GameObject _progressBack;
        [SerializeField] private Color _hardFrontColor;
        [SerializeField] private Color _hardBackColor;

        public void SetHardColor()
        {
            SetColorFront(_hardFrontColor);
            SetColorBack(_hardBackColor);
        }

        public void ShowProgressBack()
        {
            _progressBack.Activate();
            if (_texts.Length > 1) _texts[2].Activate();
        }
        
        public void HideProgressBack()
        {
            _progressBack.Deactivate();
            if (_texts.Length > 1) _texts[2].Deactivate();
        }
    }
}