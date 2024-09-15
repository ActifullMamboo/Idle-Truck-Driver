using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.AI;
using _Game.Scripts.View;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CarStuntHelper : MonoBehaviour
{
    private bool inited = false;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider);
        if (collision.collider.GetComponent<CollisionListener>())
        {
            /*if (!inited)
            {
                inited = true;
                float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

                if (collisionForce<5000)
                    return;
            
                var cityzenBehaviour = transform.parent.parent.GetComponentInParent<CityzenBehaviour>();
                cityzenBehaviour.SetNavMeshAgentFlag(false);
                DOVirtual.DelayedCall(10f,delegate { Disable(cityzenBehaviour); });

                var rb = transform.parent.AddComponent<Rigidbody>();
                rb.mass = 1000;
                Vector3 oppositeForceDirection = collision.relativeVelocity.normalized;

                rb.AddRelativeForce(oppositeForceDirection*collisionForce, ForceMode.Force);
            }*/
           

        }
           
    }
    private void Disable(CityzenBehaviour cityzenBehaviour)
    {
        cityzenBehaviour.Reset();
    }
}
