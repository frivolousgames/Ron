using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneRagdollController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    Transform explosionCenter;

    [SerializeField]
    GameObject airplane;
    Rigidbody planeRb;
    Vector3 inheretedVelocity;

    [SerializeField]
    float forceMin;
    [SerializeField]
    float forceMax;
    float explosionForce;
    [SerializeField]
    float upForce;

    Vector3 rotation;
    float rotSpeed;
    [SerializeField]
    float rotMin;
    [SerializeField]
    float rotMax;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        planeRb = airplane.GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        inheretedVelocity = planeRb.velocity;
        explosionForce = Random.Range(forceMin, forceMax);
        Vector3 forceDir = -(transform.position - explosionCenter.position).normalized;
        Vector3 forceTotal = new Vector3(forceDir.x, forceDir.y + upForce, forceDir.z) + inheretedVelocity;
        rb.AddForce(forceTotal * explosionForce, ForceMode.Force);
        rotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        rotSpeed = Random.Range(rotMin, rotMax);
    }

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime * rotSpeed);
    }
}
