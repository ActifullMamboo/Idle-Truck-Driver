using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Ui.Base;

namespace _GAME.Scripts.WorldCanvas
{
    public class GamePanel : BaseWindow
    {
        private List<CurrencyCounter> _counters = new List<CurrencyCounter>();
        public override void Init()
        {
            _counters = transform.parent.GetComponentsInChildren<CurrencyCounter>().ToList();
            for (int i = 0; i < _counters.Count; i++)
            {
                _counters[i].Init();
            }
            base.Init();
            Open();
        }
    }
}
