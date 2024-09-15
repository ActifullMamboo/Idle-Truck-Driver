using _GAME.Scripts.Configs;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts.UI.CarPick
{
    public class StatsPanel : BasePanel
    {
        [SerializeField] private TextMeshProUGUI statsName;
        [SerializeField] private TextMeshProUGUI statsNumber;
        [SerializeField] private ProceduralImage statsAmount;

        public override void SetupPanel(params object[] list)
        {
            if (list[0] is Stats stats)
            {
                statsName.DOText(stats.name, 0.33f);
                statsNumber.DOText(stats.parameter.ToString(), 0.33f, true, ScrambleMode.Numerals);
                statsAmount.DOFillAmount(stats.value, 0.1f);
            }
        }
    }
}