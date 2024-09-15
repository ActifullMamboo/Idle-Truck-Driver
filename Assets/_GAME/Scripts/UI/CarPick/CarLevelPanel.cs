using _Game.Scripts.Ui.Base;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts.UI.CarPick
{
    public class CarLevelPanel : BasePanel
    {
        [SerializeField] private TextMeshProUGUI levelName;
        [SerializeField] private ProceduralImage levelProgress;

        public override void SetupPanel(params object[] list)
        {
            levelName.DOText("lvl " +( (int)list[0] +1), 0.33f);
            levelProgress.DOFillAmount((float)list[1], 0.1f);
        }
    }
}
