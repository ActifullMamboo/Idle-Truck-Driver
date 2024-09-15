using UnityEngine;

namespace _GAME.Scripts.Accessories
{
    public class PanelAccessory : BaseAccessory
    {
        public bool isAnimated = false;
        [SerializeField] private Rigidbody parentRigid;
        [SerializeField] private Transform unParentObject;
        [SerializeField] private ConfigurableJoint joint;
        public override void StartSetup()
        {
            base.StartSetup();
            if (!isAnimated)
            {
               return;
            }
            
            joint.connectedBody = parentRigid;
            unParentObject.SetParent(null);
        }
    }
}
