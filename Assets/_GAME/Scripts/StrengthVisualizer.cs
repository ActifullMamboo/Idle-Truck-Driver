using System.Collections;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts
{
    public class StrengthVisualizer : BaseWindow
    {
        [SerializeField] private ProceduralImage filler;
        private Tween _visualTween;

        public void ShowDamage(float need)
        {
            _visualTween?.Kill();
            if (!IsOpened)
            {
                Open();
            }

            _visualTween = filler.DOFillAmount(need, 0.3f).SetDelay(0.1f);
            StopCoroutine("CloseR");
            StartCoroutine("CloseR");
        }

        IEnumerator CloseR()
        {
            yield return new WaitForSeconds(2f);
            Close();
        }
    }
}
