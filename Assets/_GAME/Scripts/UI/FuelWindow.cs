using _GAME.Scripts.Car;
using _Game.Scripts.Ui.Base;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.UI
{
    public class FuelWindow : BaseWindow ,ICurrencyComponent
    {
        [SerializeField] private BaseButton fillButton;
        [SerializeField] private BaseButton videoButton;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI amountText;
        private float _maxAmount;
        private int _price = 100;
        private FuelController _fuelController;
        private CurrensySystem _currensySystem;

        public override void Init()
        {
            base.Init();
            fillButton.SetCallback(AddGas);
            videoButton.SetCallback(FullGas);
            priceText.text = $"<sprite name=Soft>{_price}";
        }

        private void FullGas()
        {
            var fuel = GetFillFuel();
            _fuelController.AddFuel(fuel);
            fillButton.SetInteractable(false);
            videoButton.SetInteractable(false);
            DOVirtual.DelayedCall(1f, Close);
        }

        private float GetFillFuel()
        {
            var fuel = _maxAmount - _fuelController.GetCurrentAmount();
            return fuel;
        }

        private void AddGas()
        {
            var fuel = GetFillFuel();
            var max = _maxAmount * 0.1f;
            fuel = Mathf.Clamp(fuel, 0, max);
            _fuelController.AddFuel(fuel);
            _currensySystem.SpendCurrency(CurrencyType.Soft,_price);
            Redraw();
            if (_fuelController.GetCurrentAmount() >=_maxAmount)
            {
                Close();
            }
        }

        private void Redraw()
        {
            fillButton.SetInteractable(_currensySystem.IsEnoughCurrency(CurrencyType.Soft, _price));
            var fuel = GetFillFuel();
            var max = _maxAmount * 0.1f;
            fuel = Mathf.Clamp(fuel, 0, max);
            if (fuel<=0)
            {
                fillButton.SetInteractable(false);
                amountText.text = "FULL";
                return;
            }

            string txt = fuel.ToString("F1");

            amountText.text = "+" + txt;
        }

        public override void Open(params object[] list)
        {
            _fuelController = list[0] as FuelController;
            _maxAmount = _fuelController.GetMaxFuel();
            base.Open(list);
            videoButton.SetInteractable(true);
            Redraw();
        }

        public void SetCurrencySystem(CurrensySystem currensySystem)
        {
            _currensySystem = currensySystem;
        }
    }
}
