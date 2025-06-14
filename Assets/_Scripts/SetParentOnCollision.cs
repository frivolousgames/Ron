using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 20)
        {
            transform.parent = other.gameObject.transform;
        }
    }
    private void Update()
    {
        Debug.Log("Transform Parent: " + transform.parent.name);
    }
}
