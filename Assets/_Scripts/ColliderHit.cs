using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderHit : MonoBehaviour
{
    public UnityEvent hit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            hit.Invoke();
            //Debug.Log("Hit");
        }
    }
}
