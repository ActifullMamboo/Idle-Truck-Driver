using UnityEngine;

namespace _GAME.Scripts.Accessories
{
    public class Accessory : BaseAccessory
    {

        public override void StartSetup()
        {
            base.StartSetup();

            transform.parent.SetParent(null);
        }
    }
}