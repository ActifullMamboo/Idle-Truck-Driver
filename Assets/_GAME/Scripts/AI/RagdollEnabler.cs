using System;
using _Game.Scripts.View;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.AI
{
    public class RagdollEnabler : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;
        private Collider[] colliders;
        private bool ticked = false;
        void Start()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = true;
            }
        }
        private float CalculateCollisionForce(Vector3 velocity, float mass)
        {
            // Get the magnitude of the velocity
            float speed = velocity.magnitude;

            // Calculate the collision force using the formula: force = mass * speed
            float collisionForce = mass * speed;

            return collisionForce;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CollisionListener>())
            {
                if (!ticked)
                {
                    Rigidbody otherRigidbody = other.attachedRigidbody;
                    if (otherRigidbody != null)
                    {
                        float collisionForce = CalculateCollisionForce(otherRigidbody.velocity, otherRigidbody.mass);
            
                       // Debug.Log(otherRigidbody);
                        //Debug.Log("Collision Force: " + collisionForce);
                        if (collisionForce>100)
                        {
                            ticked = true;

                            for (int i = 0; i < colliders.Length; i++)
                            {
                                colliders[i].isTrigger = false;
                            }
                            GetComponent<CapsuleCollider>().enabled = false;
                            var anim = GetComponent<Animator>();
                            anim.enabled = false;
                            var cityzenBehaviour = GetComponentInParent<CityzenBehaviour>();
                            cityzenBehaviour.BreaksMovement();
                            DOVirtual.DelayedCall(5f,delegate { Disable(cityzenBehaviour); });
                            for (int i = 0; i < _rigidbodies.Length; i++)
                            {
                                _rigidbodies[i].isKinematic = false;
                            }
                        }
                    }
                }
             
            }
            
        }

        private void Disable(CityzenBehaviour cityzenBehaviour)
        {
            
            cityzenBehaviour.Reset();
        }
    }
}
