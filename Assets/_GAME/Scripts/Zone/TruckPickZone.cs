using System.Collections.Generic;
using _Game.Scripts.Tools;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.Zone
{
    public class TruckPickZone : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private GameObject zone;
        public List<WheelCollider> wheelColliders;
        


        public void ClaimTrailer(Rigidbody rb, Vector3 point)
        {
            rigid.transform.position = point;
            rigid.isKinematic = false;
            //AddJoint(rb);
            zone.Deactivate();
            gameObject.Deactivate();

        }

        private void AddJoint(Rigidbody rb)
        {
            var rig = rigid;
            AddJoint(rb, rig);
        }

        private static void AddJoint(Rigidbody rb, Rigidbody rig)
        {
            var joint = rig.gameObject.AddComponent<HingeJoint>();
            // joint.autoConfigureConnectedAnchor = false;
            //joint.connectedAnchor = new Vector3(0, 0.8f, -0.75f);
            joint.connectedBody = rb;
            joint.axis = new Vector3(0, 1, 0);
            //joint.useSpring = true;
            JointSpring spring = new JointSpring();
            spring.spring = 35000;
            spring.damper = 4500;
            joint.spring = spring;
            joint.useLimits = true;
            JointLimits limits = new JointLimits();
            limits.min = -90;
            limits.max = 90;
            joint.limits = limits;
        }

        public void Claim(Rigidbody rb,Vector3 transformPosition)
        {
            rigid.transform.position = transformPosition;
            AddJoint(rb);

        }
    }
}
