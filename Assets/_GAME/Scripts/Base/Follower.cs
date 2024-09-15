using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform t;

    public Vector3 offset;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = t.position+offset;
        transform.rotation = t.rotation;
    }
}
