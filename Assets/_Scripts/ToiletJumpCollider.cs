using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToiletJumpCollider : MonoBehaviour
{
    [SerializeField]
    UnityEvent landed;
    [SerializeField]
    UnityEvent inAir;

    bool inTheAir;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            landed.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            inAir.Invoke();
        }
    }
}
