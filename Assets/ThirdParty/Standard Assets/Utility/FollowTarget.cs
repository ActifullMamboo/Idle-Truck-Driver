using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        public float enu;

        private void FixedUpdate()
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Lerp(transform.rotation,target.rotation,Time.deltaTime*enu);
        }
    }
}
