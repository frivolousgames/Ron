using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollider : MonoBehaviour
{
    Collider playerPhysCol;
    [SerializeField]
    Collider toiletPhysCol;

    bool ignoring;

    private void Start()
    {
        playerPhysCol = GameObject.FindGameObjectWithTag("PlayerPhysCol").GetComponent<Collider>();
    }

    private void Update()
    {
        Debug.Log("Ignoring: " + ignoring);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 18)
        {
            Ignore(true);
            ignoring = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 18)
        {
            Ignore(true);
            ignoring = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 18)
        {
            Ignore(false);
            ignoring = false;
        }
    }

    void Ignore(bool tf)
    {
        Physics.IgnoreCollision(playerPhysCol, toiletPhysCol, tf);
    }
}
