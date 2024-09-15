using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCom : MonoBehaviour
{
    public Vector3 COM= new Vector3(0, 2.6f, 0.27f);
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.centerOfMass = COM;
    }
}
