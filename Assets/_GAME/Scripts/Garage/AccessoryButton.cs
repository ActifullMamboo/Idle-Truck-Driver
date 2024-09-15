using System;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;

namespace _GAME.Scripts.Garage
{
    public class AccessoryButton : BaseButton
    {
        private GarageAccessoryZone _zone;
        public void Init()
        {
            _zone = FindObjectOfType<GarageAccessoryZone>(true);
            Callback += OpenZone;
        }

        private void OpenZone()
        {
            _zone.Activate();
        }
    }
}
