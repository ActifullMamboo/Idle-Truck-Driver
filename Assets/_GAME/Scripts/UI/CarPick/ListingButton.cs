using System;
using _Game.Scripts.Ui.Base;
using UnityEngine;

namespace _GAME.Scripts.UI.CarPick
{
    public class ListingButton : BaseButton
    {
        
        public Action<int> OnListing;

        [SerializeField] private int number;
        protected override void OnClick()
        {
            OnListing.Invoke(number);
            base.OnClick();
        }
    }
}
