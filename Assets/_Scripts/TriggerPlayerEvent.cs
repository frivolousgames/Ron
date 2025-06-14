using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlayerEvent : MonoBehaviour
{
    public UnityEvent hit;
    [SerializeField]
    Collider[] targetCol;

    private void OnTriggerEnter(Collider other)
    {
        foreach(Collider col in targetCol)
        {
            if (other == col)
            {
                hit.Invoke();
            }
        } 
    }
}
