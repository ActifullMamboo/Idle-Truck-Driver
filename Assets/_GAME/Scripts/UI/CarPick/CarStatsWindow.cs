using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Configs;
using _Game.Scripts.Ui.Base;
using UnityEngine;

namespace _GAME.Scripts.UI.CarPick
{
    public class CarStatsWindow : BaseWindow, ICurrencyComponent
    {
        [SerializeField] private List<CarConfig> configs;
        public Action<int> OnListed;
        private List<BasePanel> _panels = new();
        private BuyButton _buyButton;
        private CurrensySystem _currensySystem;
        private int _configNumber;

        public override void Init()
        {
            base.Init();
            _panels = GetComponentsInChildren<BasePanel>(true).ToList();
            _buyButton = GetComponentInChildren<BuyButton>(true);
            _buyButton.SetCallback(BuyACar);
            var listingB = GetComponentsInChildren<ListingButton>(true);

            for (var i = 0; i < listingB.Length; i++) listingB[i].OnListing += MoveNext;
            
            
            var activeConfig = configs[_configNumber];

            RedrawBuyButton();
            ReSetupLevelPanel(activeConfig);
            ReSetupStatsPanel(activeConfig);
            Open();

        }

        private void BuyACar()
        {
            var activeConfig = configs[_configNumber];
            var boughtCar = SaveSystem.GetBoughtCar(_configNumber);
            if (boughtCar == _configNumber)
            {
                SaveSystem.SaveActiveCar(_configNumber);

            }
            else
            {
                _currensySystem.SpendCurrency(CurrencyType.Soft, activeConfig.price);
                SaveSystem.SaveActiveCar(_configNumber);
                SaveSystem.SaveBoughtCar(_configNumber);

            }
            
            RedrawBuyButton();
        }

        private void RedrawBuyButton()
        {
            var boughtCar = SaveSystem.GetBoughtCar(_configNumber);
            if (boughtCar == _configNumber)
            {
                var carId = SaveSystem.GetActiveCar();
                if (carId == _configNumber)
                {
                    _buyButton.SetText("PICKED");
                    _buyButton.SetInteractable(false);
                }
                else
                {
                    _buyButton.SetText("PICK");
                    _buyButton.SetInteractable(true);
                } 
            }
            else
            {
                var price = configs[_configNumber].price;

                var text = "<sprite name=Soft>" + price;
                
                _buyButton.SetText(text);
                var hasMoney = _currensySystem.IsEnoughCurrency(CurrencyType.Soft, price);
                _buyButton.SetInteractable(hasMoney);
            }
            
           
        }

        private void MoveNext(int obj)
        {
            _configNumber += obj;

            if (_configNumber > configs.Count - 1)
            {
                _configNumber = 0;

            }
            else if (_configNumber < 0)
            {
                _configNumber = configs.Count - 1;
            }
            var activeConfig = configs[_configNumber];
            OnListed?.Invoke(_configNumber);
            ReSetupLevelPanel(activeConfig);
            ReSetupStatsPanel(activeConfig);
            RedrawBuyButton();

        }

        private void ReSetupLevelPanel(CarConfig activeConfig)
        {
            var carLevelP = _panels.Find(x => x.panelType == PanelType.Level);
            var currentPoints = SaveSystem.GetCarLevelPoints(activeConfig.name);
            if (currentPoints>=activeConfig.CurrentPointsToNextLevel)
            {
                currentPoints -=activeConfig.CurrentPointsToNextLevel;
                SaveSystem.SaveCarLevelPoints(activeConfig.name,currentPoints);
                activeConfig.Upgrade();
            }
            var percents = currentPoints / activeConfig.CurrentPointsToNextLevel;

            var param = new object[2] { activeConfig.currentLevel, percents };
            carLevelP.SetupPanel(param);
        }

        private void ReSetupStatsPanel(CarConfig activeConfig)
        {
            var carStatsPanel = _panels.FindAll(x => x.panelType == PanelType.Stats);
            for (var i = 0; i < carStatsPanel.Count; i++) carStatsPanel[i].SetupPanel(activeConfig.carStats.stats[i]);
        }

        public void SetCurrencySystem(CurrensySystem currensySystem)
        {
            _currensySystem = currensySystem;
        }
    }
}