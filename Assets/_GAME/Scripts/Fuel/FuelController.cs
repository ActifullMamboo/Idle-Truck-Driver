using System;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using _GAME.Scripts.UI.CarControlls;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts.Car
{
    public class FuelController : BaseWindow
    {
        public Action<FuelController> OnEmpty;
        [SerializeField] private ProceduralImage filler;
        [SerializeField] private TextMeshProUGUI percentage;
        [SerializeField,Space(5)] private Color lowColor;
        [SerializeField] private TextMeshProUGUI lowFuelText;
        private bool _lowFuel = false;
        private bool _pressed = false;
        private bool _empty = false;
        private float _startFuel = 0;
        private float _currentFuel = 0;
        private Tween _fillTween;
        public override void Init()
        {
            base.Init();
            Open();
        }

        public void Initialize(TruckBehaviour beh)
        {
            var stats = beh.CarConfig.carStats.stats;
            var fuelParam = stats.Find(x => x.name == "Fuel");
            _startFuel = fuelParam.parameter;
            _currentFuel = _startFuel;
            SetupFuelVisual();

        }
        public void DecreaseFuel(SimpleInput.AxisInput arg1, RectTransform arg2)
        {
            if (_empty)
            {
                return;
            }

            if (!_pressed)
            {
                return;
            }
            _currentFuel -= Time.deltaTime;
            var fill = SetupFuelVisual();
            if (fill<=0)
            {
                _empty = true;
                OnEmpty?.Invoke(this);
                percentage.text = "EMPTY";

            }
            if (_lowFuel)
            {
                return;
            }
            if (fill<0.25f)
            {
                _lowFuel = true;
                filler.DOColor(lowColor, 0.5f);
                lowFuelText.Activate();
                lowFuelText.DOFade(0.3f, 0.5f);
                lowFuelText.DOScale(1.3f, 2f).SetEase(Ease.Linear).SetDelay(0.5f).SetLoops(4, LoopType.Yoyo);
                lowFuelText.DOFade(1f, 2f).SetEase(Ease.Linear).SetDelay(0.5f).SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        lowFuelText.DOFade(0f, 0.5f).OnComplete(() =>
                        {
                            lowFuelText.Deactivate();
                        });

                    });
            }
        }

        private float SetupFuelVisual()
        {
            float fill = _currentFuel / _startFuel;
            percentage.text = (fill * 100).ToString("F1") + "%";
            filler.fillAmount = fill;
            return fill;
        }

        private void FloatFill()
        {
            float fill = _currentFuel / _startFuel;
            percentage.text = (fill * 100).ToString("F1") + "%";
            _fillTween?.Kill();
            _fillTween = filler.DOFillAmount(fill, 0.3f).SetEase(Ease.InSine);
        }

        public void StartDecrease()
        {
            _pressed = true;
        }
        public void StopDecrease()
        {
            _pressed = false;
        }

        public float GetCurrentAmount()
        {
            return _currentFuel;
        }
        public float GetMaxFuel()
        {
            return _startFuel;
        }

        public void AddFuel(float fuel)
        {
            _currentFuel += fuel;
            FloatFill();
        }
    }
}
