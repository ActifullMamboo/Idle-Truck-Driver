using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHeadFollower : MonoBehaviour
{
    [SerializeField] private Rigidbody lateRb;
    [SerializeField] private Rigidbody thisRb;

    // Update is called once per frame
    void FixedUpdate()
    {
        thisRb.position = lateRb.position;
        thisRb.rotation = lateRb.rotation;

        thisRb.velocity = lateRb.velocity;
        thisRb.angularVelocity = lateRb.angularVelocity;
    }
}
