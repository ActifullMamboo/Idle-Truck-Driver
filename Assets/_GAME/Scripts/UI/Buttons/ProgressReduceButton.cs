using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Ui.Buttons
{
    public class ProgressReduceButton :BaseUIView
    {
        [SerializeField] private BaseButton _adWatchButton;
        [SerializeField] private BaseButton _hardButton;
        [SerializeField] private TextMeshProUGUI _timeText;

        // public AdWatchButton AdWatchButton => _adWatchButton;
        // public BaseButton HardButton => _hardButton;
        //
        // public void RedrawButtons(string timeLeft, int tickets, int hardPrice, float reduceTime, bool adsRemoved)
        // {
        //     var text = $"{"REDUCE".ToLocalized()} \n{reduceTime / 60}{"MIN".ToLocalized()}.";
        //     if (tickets > 0)
        //     {
        //         _adWatchButton.ShowTicketsButton(tickets, text);
        //     }
        //     else
        //     {
        //         _adWatchButton.ShowDefaultButton(text, adsRemoved);
        //     }
        //     
        //     _hardButton.SetText("SPEEDUP".ToLocalized());
        //     _hardButton.SetText(1, $"<sprite name=Hard>{hardPrice}");
        //
        //     _timeText.text = $"<sprite name=CycleDelay>{timeLeft}";
        // }
        //
        // public void UpdateProgress(string timeLeft, int hardPrice)
        // {
        //     _hardButton.SetText("SPEEDUP".ToLocalized());
        //     _hardButton.SetText(1, $"<sprite name=Hard>{hardPrice}");
        //     _timeText.text = $"<sprite name=CycleDelay>{timeLeft}";
        // }
    }
}