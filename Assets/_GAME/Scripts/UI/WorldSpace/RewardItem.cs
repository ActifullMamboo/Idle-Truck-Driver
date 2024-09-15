using _GAME.Scripts.Tasks;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace _GAME.Scripts.UI.WorldSpace
{
    public class RewardItem : MonoBehaviour
    {

        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;
        private int _amount;
        public void SetupReward(Reward requireTaskReward)
        {
            rewardImage.sprite = requireTaskReward.RewardSprite;
            rewardText.text = requireTaskReward.RewardText;
            _amount = requireTaskReward.RewardAmount;
        }

        public void SetInNull()
        {
            DOVirtual.Int(_amount, 0, 0.5f, x => rewardText.text = x.ToString());
        }
    }
}
