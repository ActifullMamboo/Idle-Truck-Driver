using System;
using _GAME.Scripts.Components;
using _GAME.Scripts.Upgrades;
using UnityEngine;

namespace _GAME.Scripts
{
    public enum CurrencyType
    {
        Soft,
        Hard
    }
    public class CurrensySystem : MonoBehaviour
    {

        public Action<int, int> UpdateCurrency;

        private int _softCurrency;

        private int _hardCurrency;
        private int _incrementer = 1;

        public void Initialize()
        {
            _softCurrency = SaveSystem.GetCurrency(CurrencyType.Soft);
        }

      

        public int GetCurrency(CurrencyType type)
        {
            switch (type)
            {
                case CurrencyType.Soft:
                    return _softCurrency;
                case CurrencyType.Hard:
                    return _hardCurrency;
                default: return 0;
            }
        }

        public void AddCurrency(CurrencyType type, int count)
        {
            switch (type)
            {
                case CurrencyType.Soft:
                    _softCurrency += count*_incrementer;
                    SaveSystem.SaveCurrency(type, _softCurrency);

                    break;
                case CurrencyType.Hard:
                    _hardCurrency += count;
                    SaveSystem.SaveCurrency(type, _hardCurrency);

                    break;
            }

            UpdateCurrency?.Invoke(_softCurrency, _hardCurrency);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                AddCurrency(CurrencyType.Soft, 1000);
            else if (Input.GetKeyDown(KeyCode.H)) AddCurrency(CurrencyType.Hard, 1000);
        }

        public void SpendCurrency(CurrencyType type, int price)
        {
            switch (type)
            {
                case CurrencyType.Soft:
                    _softCurrency -= price;
                    SaveSystem.SaveCurrency(type, _softCurrency);

                    break;
                case CurrencyType.Hard:
                    _hardCurrency -= price;
                    SaveSystem.SaveCurrency(type, _hardCurrency);

                    break;
            }

            UpdateCurrency?.Invoke(_softCurrency, _hardCurrency);
        }


        public bool IsEnoughCurrency(CurrencyType type, int price)
        {
            var curr = type switch
            {
                CurrencyType.Soft => _softCurrency,
                CurrencyType.Hard => _hardCurrency,
                _ => 0
            };

            return price <= curr;
        }

        public string GetSoft()
        {
            return _softCurrency.ToString();
        }

        public void ShowCurrency(CurrencyType soft, int i)
        {
            var curr = _softCurrency + i;
            UpdateCurrency?.Invoke(curr, _hardCurrency);
        }
    }

    public interface ICurrencyComponent
    {
        public void SetCurrencySystem(CurrensySystem currensySystem);
    }
}