using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCasingController : MonoBehaviour
{
    Rigidbody rb;

    public float velocityY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(velocityY - 2f, velocityY + 2f), 0f),  ForceMode.Impulse);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
}
