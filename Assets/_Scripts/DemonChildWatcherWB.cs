using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonChildWatcherWB : MonoBehaviour
{
    float playerDistance;

    [SerializeField]
    Transform bus;

    [SerializeField]
    float threshold;

    [SerializeField]
    GameObject[] parts;


    private void Update()
    {
        playerDistance = Vector3.Distance(bus.position, transform.position);
        if(playerDistance < threshold)
        {
            foreach(var p in parts)
            {
                p.SetActive(true);
            }
        }
        else
        {
            foreach (var p in parts)
            {
                p.SetActive(false);
            }
        }
    }
}
