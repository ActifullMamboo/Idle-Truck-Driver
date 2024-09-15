using UnityEngine;

namespace _GAME.Scripts.Player
{
    public class AnimationsController : MonoBehaviour
    {
        private Animator _animator;
        private readonly string _speed = "Speed";
        private readonly string _attack = "Hit";
        private readonly string _base = "base";

        
        public virtual void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetSpeed(float speed)
        {
            _animator.SetFloat(_speed, speed);
        }

        public void Hit()
        {
            _animator.SetTrigger(_attack);
        }

        public void Animate(int targetPointAnimationHash)
        {
            _animator.SetFloat(_speed, 0);
        }
        public void Animate()
        {
            _animator.CrossFade(_base,0.2f);
        }

        public void Die()
        {
            //
        }
    }
}