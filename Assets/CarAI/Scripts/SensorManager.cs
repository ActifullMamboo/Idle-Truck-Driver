using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    public CarAI carAI;
    public string tagName;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(tagName))
        {
            carAI.move = false;
        }
    }



    private void OnTriggerEnter(Collider car)
    {
        if (car.gameObject.CompareTag(tagName))
        {
            carAI.move = false;
        }
    }

    private void OnTriggerExit(Collider car)
    {
        if (car.gameObject.CompareTag(tagName))
        {
            carAI.MovesInDelay();
        }
    }
}
