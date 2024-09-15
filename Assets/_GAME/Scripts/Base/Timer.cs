using System;
using _Game.Scripts.Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _GAME.Scripts.Base
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private ProceduralImage filler;
        [SerializeField] private TextMeshProUGUI timerText;
        private float _startTime;

        public void StartTimer(float seconds)
        {
            _startTime = seconds;
            filler.fillAmount = 0;

            gameObject.Activate();
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        public void UpdateTime(float seconds)
        {
            if (seconds<=0)
            {
                transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    gameObject.Deactivate();
                });
                return;
            }

            var fill = (_startTime-seconds )/ _startTime;
            filler.fillAmount = fill;
            timerText.text = ConvertFloatToTimeText(seconds);
        }

        private string ConvertFloatToTimeText(float time)
        {
            var timeSpan = TimeSpan.FromSeconds(time);

            var timeText = string.Format("{0:D2}:{1:D2}",
                 timeSpan.Minutes, timeSpan.Seconds);

            return timeText;
        }
    }
}