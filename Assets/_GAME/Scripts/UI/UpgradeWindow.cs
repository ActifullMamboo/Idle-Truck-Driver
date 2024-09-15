using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using _GAME.Scripts.Garage;
using _Game.Scripts.Systems;
using TMPro;
using _Game.Scripts.Ui.Base;
using _GAME.Scripts.Upgrades;
using UnityEngine;
namespace _GAME.Scripts.UI
{
    public class UpgradeWindow : BaseWindow, ICurrencyComponent, ISoundPlayer
    {
        public UpgradeType upgradeType;
        [SerializeField] private UpgradableItem upgradableItem;
        [SerializeField] private UpgradeButton upgradeButton;
        [SerializeField] private TextMeshProUGUI nameText;

        private List<UpgradeItemUI> _upgradeItemUis;
        
        private CurrensySystem _currensySystem;
        private SoundSystem _soundSystem;
        private AdsManager _adsManager;
        private int _id = 0;
        private GarageAccessoryPicker _picker;

        public void SetCurrencySystem(CurrensySystem currensySystem)
        {
            _currensySystem = currensySystem;
        }

        public void InitSound(SoundSystem soundSystem)
        {
            _soundSystem = soundSystem;
        }
        
        public override void Init()
        {
            _upgradeItemUis = GetComponentsInChildren<UpgradeItemUI>(true).ToList();
            _picker = FindObjectOfType<GarageAccessoryPicker>(true);
            _adsManager = FindObjectOfType<AdsManager>();

            for (int i = 0; i < _upgradeItemUis.Count; i++)
            {
                var conf = upgradableItem;
                _upgradeItemUis[i].SetupUpgradeItem(i,conf.itemsToUpgrade[i]);
                _upgradeItemUis[i].OnClick += ChangeView;
            }


            base.Init();
            nameText.text = upgradableItem.upgradeName;

        }

        private void ChangeView(int id)
        {
            Redraw(id);
            _picker.OnChangeView.Invoke(upgradableItem.upgradeType,id);
        }
        public override void Open(params object[] list)
        {
            upgradeButton.SetCallback(OnBuyButtonClicked);
            base.Open(list);
            Redraw(-1);
        }

        private void Redraw(int i)
        {
            foreach (var upgradeItemUi in _upgradeItemUis) upgradeItemUi.Redraw();

            if (i<0)
                return;
            _id = i;
            RedrawBuyButton();

        }

        private void OnDestroy()
        {
            foreach (var upgradeItemUi in _upgradeItemUis) upgradeItemUi.OnClick -= Redraw;
        }

        private void RedrawBuyButton()
        {
            var itemToUpgrade = upgradableItem.itemsToUpgrade[_id];
            var price = itemToUpgrade.price;
            var text = "";

            if (price == 999)
            {
                upgradeButton.SetInteractable(true);
                text = "<sprite name=Ad>";
            }
            else if (price == 0)
            {
                if (upgradableItem.activeItem == _id)
                {
                    text = "EQUIPPED";
                    upgradeButton.SetInteractable(false);
                }
                else
                {
                    text = "EQUIP";
                    upgradeButton.SetInteractable(true);
                }
            }
            else
            {
                var b = _currensySystem.IsEnoughCurrency(CurrencyType.Soft, price);
                upgradeButton.SetInteractable(b);

                text = "<sprite name=Soft>" + price;
            }

            upgradeButton.SetText(text);
        }

        private void OnBuyButtonClicked()
        {
            var itemToUpgrade = upgradableItem.itemsToUpgrade[_id];
            var price = itemToUpgrade.price;
            _soundSystem.PlaySound(GameSoundType.ButtonClick, transform);

            if (price == 999)
            {
                var placementName = upgradableItem.upgradeName;
                _adsManager.ShowRewardAd(placementName, Upgrade);
            }
            else if (price == 0)
            {
                Upgrade();
                //AdsManager.ActionDone();
            }
            else
            {
                if (_currensySystem.IsEnoughCurrency(CurrencyType.Soft, price))
                {
                    _currensySystem.SpendCurrency(CurrencyType.Soft, price);
                    Upgrade();
                    //AdsManager.ActionDone();
                }
            }
        }

        private void Upgrade()
        {
            upgradableItem.Upgrade(_id);
            RedrawBuyButton();

        }


    }
}
