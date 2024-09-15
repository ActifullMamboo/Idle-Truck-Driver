using System.Collections;
using _GAME.Scripts.Configs;
using _Game.Scripts.Ui.Base;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using _GAME.Scripts.Load;
using _GAME.Scripts.Progress;
using _GAME.Scripts.Tasks;
using _GAME.Scripts.UI.WorldSpace;

namespace _GAME.Scripts.UI
{
    public class WinWindow : BaseWindow,ICurrencyComponent
    {
        [SerializeField] private List<CarConfig> configs;
        [SerializeField] private List<RewardItem> items;
        [SerializeField] private BaseButton claimButton;
        [SerializeField] private BaseButton x2Button;
        [SerializeField] private ParticleSystem cashFx;
        private int _moneyCount;
        private int _expCount;
        private int _configNumber;
        
        private CurrensySystem _currensySystem;

        public override void Init()
        {
            base.Init();
            cashFx.GetComponent<ProgressParticleController>().Init();
            claimButton.SetCallback(ClaimAndMoveNext);
            x2Button.SetCallback(ClaimX2);
        }

        private void ClaimX2()
        {
            _expCount *= 2;
            _moneyCount *= 2;
            ClaimAndMoveNext();
        }

        private void ClaimAndMoveNext()
        {
            x2Button.SetInteractable(false);
            claimButton.SetInteractable(false);
            _currensySystem.AddCurrency(CurrencyType.Soft,_moneyCount);
            var activeConfig = configs[_configNumber];
            SaveSystem.SaveCarLevelPoints(activeConfig.name,_expCount);
            cashFx.Play();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetInNull();
            }

            StartCoroutine(WaitR());
        }

        IEnumerator WaitR()
        {
            yield return new WaitForSeconds(2f);
            LoadingManager.LoadScene(LoadingType.Garage);
            Close();
            yield return new WaitForSeconds(.3f);

        }

        public override void Open(params object[] list)
        {
            x2Button.SetInteractable(true);
            claimButton.SetInteractable(true);
            base.Open(list);
            var task = TasksController.activeTask;
            var reward = task.Rewards;
            _moneyCount = reward[1].RewardAmount;
            _expCount = reward[0].RewardAmount;
            for (int i = 0; i < reward.Count; i++)
            {
                items[i].SetupReward(reward[i]);
            }
            _configNumber = SaveSystem.GetActiveCar();

            for (int i = 0; i < _windowAnimations.Count; i++)
            {
                _windowAnimations[i].SetDelay(0.1f*i);
            }
        }

        public void SetCurrencySystem(CurrensySystem currensySystem)
        {
            _currensySystem = currensySystem;
        }
    }
}
