using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerryShovelThrow : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    Transform shovelSpawn;

    [SerializeField]
    float throwSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        transform.position = shovelSpawn.position;
        rb.AddForce(transform.forward * throwSpeed, ForceMode.Impulse);
    }
}
