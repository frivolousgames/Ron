using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCollision : MonoBehaviour
{
    [SerializeField]
    Collider playerCol;

    Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }
    private void FixedUpdate()
    {
        Physics.IgnoreCollision(col, playerCol);
    }
}
