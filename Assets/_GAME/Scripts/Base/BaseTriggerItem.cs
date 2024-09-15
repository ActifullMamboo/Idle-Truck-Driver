using _GAME.Scripts.Base;
using UnityEngine;
namespace _Game.Scripts.View
{
    public class BaseTriggerItem : BaseView
    {
        [SerializeField] private CollisionListener collisionListener;

        public override void Init(params object[] list)
        {
            base.Init();
            collisionListener.OnEnter += CollisionListener_OnEnter;
            collisionListener.OnExit += CollisionListener_OnExit;
        }

        public virtual void CollisionListener_OnExit(Collider collider)
        {
        }

        public virtual void CollisionListener_OnEnter(Collider col)
        {
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            collisionListener.OnEnter -= CollisionListener_OnEnter;
            collisionListener.OnExit -= CollisionListener_OnExit;
        }
    }
}
