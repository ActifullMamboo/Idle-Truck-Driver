using System;
using _Game.Scripts.Systems;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using _GAME.Scripts.Upgrades;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts.UI
{
    public class UpgradeItemUI : BaseUIView
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private ProceduralImage fade;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Color clickColor;
        private Color _singleColor;

        private int _id;

        public Action<int> OnClick;

        public void SetupUpgradeItem(int id, UpgradableItem.ItemToUpgrade item)
        {
            _singleColor = fade.color;
            itemImage.sprite = item.sprite;
            _id = id;
            var button = GetComponent<BaseButton>();
            button.SetCallback(OnChoose);

            var price = item.price;
            var text = "";
            if (price == 999)
            {
                text = "<sprite name=Ad>";
            }
            else if (price == 0)
            {
                
            }
            else
            {
                text = "<sprite name=Soft>" + price;
            }
            priceText.SetText(text);
        }

        private void OnChoose()
        {
            OnClick?.Invoke(_id);
            fade.DOColor(clickColor, 0.05f);
            RectTransform.DOScale(1.2f, 0.33f);
        }


        public void Redraw()
        {
            fade.DOColor(_singleColor, 0.05f);
            RectTransform.DOScale(1f, 0.13f).SetEase(Ease.InBack);


        }
    }
}