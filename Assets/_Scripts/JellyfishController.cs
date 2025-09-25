using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishController : SwimmingBackgroundAnimals
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pooler = new ObjectPooler();
    }
    private void Start()
    {
        if (FishRagdollArrayList.creatureRagdolls.TryGetValue(ragdollKey, out ragdolls))
        {
            ragdolls = FishRagdollArrayList.creatureRagdolls[ragdollKey];
            Debug.Log(ragdolls);
        }
    }
    private void OnEnable()
    {
        SetInactiveByTime();

        height = Random.Range(minHeight, maxHeight);
        heightSpeed = Random.Range(heightSpeedMin, heightSpeedMax);
    }

    private void OnDisable()
    {
        if (setInactiveWait != null)
        {
            StopCoroutine(setInactiveWait);
        }
    }

    private void FixedUpdate()
    {
        currentSpeed = Random.Range(speedMin, speedMax);
        rb.velocity = new Vector3(Mathf.SmoothStep(-height, height, Mathf.PingPong(Time.time * heightSpeed, 1)), transform.up.y * currentSpeed * Time.deltaTime, 0f);
    }
}
