using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SwimmingFishController : SwimmingBackgroundAnimals
{

    //ParticleSystem.MainModule mainModule;

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
            //Debug.Log(ragdolls);
        }
        //ragdolls = PooledObjectArrays.fishChunksArrays;
        //mainModule = ps.main;
    }
    private void OnEnable()
    {
        currentAnimSpeed = Random.Range(.9f, 1f);
        //currentSpeed = Random.Range(-100f, -200f);
        //currentSpeed = -150f;
        anim.speed = currentAnimSpeed;

        height = Random.Range(minHeight, maxHeight);
        heightSpeed = Random.Range(heightSpeedMin, heightSpeedMax);
        SetInactiveByTime();
    }
    private void OnDisable()
    {
        if(setInactiveWait != null)
        {
            StopCoroutine(setInactiveWait);
        }
    }
    private void Update()
    {
        anim.SetFloat("offset", animOffset);

    }

    private void FixedUpdate()
    {
        currentSpeed = Random.Range(speedMin, speedMax);
        rb.velocity = new Vector3(transform.right.x * currentSpeed * Time.deltaTime, Mathf.SmoothStep(-height, height, Mathf.PingPong(Time.time * heightSpeed, 1)), 0f);
    }
}
