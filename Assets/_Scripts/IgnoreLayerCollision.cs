using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreLayerCollision : MonoBehaviour
{
    [SerializeField]
    LayerMask otherLayer;

    Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        //if()
        //Physics.IgnoreCollision(col, otherLayer);
    }
}
