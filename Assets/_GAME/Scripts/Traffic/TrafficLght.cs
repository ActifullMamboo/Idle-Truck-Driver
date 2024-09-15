using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Tools;
using _Game.Scripts.View;
using UnityEngine;

public class TrafficLght : MonoBehaviour
{
    [SerializeField] private CollisionListener Listener;
    [SerializeField] private GameObject carStoper;
    [SerializeField] private GameObject humanStoper;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject yellow;
    [SerializeField] private GameObject green;

    public bool needScenario = false;

    private void Start()
    {
        if (needScenario)
        {
            Listener.OnEnter += StartTimer;

        }
        else
        {
            StartCoroutine(TrafficRoutine());

        }
    }

    private void StartTimer(Collider obj)
    {
        if (obj.GetComponent<PrometeoCarController>())
        {
            StartCoroutine(TrafficRoutine());
        }
    }

    private IEnumerator TrafficRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            red.Activate();
            yellow.Deactivate();
            yield return new WaitForSeconds(2f);

            yellow.Activate();
            green.Deactivate();
            humanStoper.Activate();
            carStoper.Deactivate();
            yield return new WaitForSeconds(8f);

            for (int i = 0; i < 4; i++)
            {
                green.Activate();
                yield return new WaitForSeconds(0.4f);
                green.Deactivate();
                yield return new WaitForSeconds(0.2f);
            }

            green.Activate();
            yellow.Deactivate();
            yield return new WaitForSeconds(2f);
            yellow.Activate();
            red.Deactivate();
            humanStoper.Deactivate();
            carStoper.Activate();

            yield return new WaitForSeconds(8f);
        }
    }
}
