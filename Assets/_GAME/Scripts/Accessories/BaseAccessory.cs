using _Game.Scripts.Tools;
using _GAME.Scripts.Upgrades;
using UnityEngine;

namespace _GAME.Scripts.Accessories
{
    public class BaseAccessory : MonoBehaviour
    {
        public virtual void StartSetup()
        {
            gameObject.Activate();
        }

    }
}
