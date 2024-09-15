using _Game.Scripts.Ui.Base;
using _GAME.Scripts.Upgrades;
using DG.Tweening;

namespace _GAME.Scripts.UI
{
    public class ChooseWindowButton : BaseButton
    {
        public UpgradeType upgradeType;
        private Tween _tween;
        public void ScaleOut()
        {
            _tween?.Kill();
            _tween = transform.DOScale(1f, 0.2f).SetEase(Ease.OutSine);
        }

        public void ScaleIn()
        {
            _tween?.Kill();
            _tween = transform.DOScale(1.2f, 0.2f).SetEase(Ease.InSine);

        }
    }
}
