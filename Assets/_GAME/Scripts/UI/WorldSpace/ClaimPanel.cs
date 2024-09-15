using System;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base;
using _GAME.Scripts.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME.Scripts.UI.WorldSpace
{
    public class ClaimPanel : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private Image _rewardImage;
        private BaseButton button;

        private Action _callback;
        private Vector3 point;

        public override void Init()
        {
            button = GetComponentInChildren<BaseButton>();
            button.SetCallback(OnClick);
            base.Init();
        }

        /*public void SetupReward(Reward taskConfigReward)
        {
            if (taskConfigReward.RewardSprite == null)
            {
                _rewardImage.Deactivate();
            }
            else
            {
                _rewardImage.Activate();
                _rewardImage.sprite = taskConfigReward.RewardSprite;
            }

            _rewardText.text = taskConfigReward.RewardText;
            if (taskConfigReward.DescriptionText.IsNullOrEmpty())
            {
                _descriptionText.Deactivate();
            }
            else
            {
                _descriptionText.text = taskConfigReward.DescriptionText;
            }
        }*/

        public void SetCallback(Action action)
        {
            _callback = action;
        }

        private void OnClick()
        {
            Close();
            _callback?.Invoke();
            _callback = null;
        }

        public void Show()
        {
            /*Anchor anchor = new()
            {
                Scale = Vector3.one,
                EulerAngles = Vector3.zero,
                Position = Vector3.zero
            };

            if (target.TryGetComponent(out WorldSpacePanelOffset worldSpacePanelOffset))
                anchor = worldSpacePanelOffset.Anchor;

            point = target.position + anchor.Position;
            transform.position = Camera.main.WorldToScreenPoint(point);*/
           
            base.Open();
        }
    }
}