using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SwimmingFishController : SwimmingBackgroundAnimals
{
    [SerializeField]
    OnVisibleOffScreen onVis;

    [SerializeField]
    float inactiveWait;

    [SerializeField]
    ParticleSystem ps;

    ObjectPooler pooler;
    GameObject[] fishChunks;

    [SerializeField]
    float speedMin;
    [SerializeField]
    float speedMax;
    //ParticleSystem.MainModule mainModule;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pooler = new ObjectPooler();
    }

    private void Start()
    {
        fishChunks = PooledObjectArrays.fishChunksArrays;
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
        SetInactiveByTime();
    }

    private void FixedUpdate()
    {
        currentSpeed = Random.Range(speedMin, speedMax);
        rb.velocity = new Vector3(transform.right.x * currentSpeed * Time.deltaTime, Mathf.SmoothStep(-height, height, Mathf.PingPong(Time.time * heightSpeed, 1)), 0f);
    }
    void SetInactiveByTime()
    {
        setInactiveWait = StartCoroutine(SetInactiveWait());
    }

    IEnumerator SetInactiveWait()
    {
        yield return new WaitForSeconds(inactiveWait);
        while (onVis.isVisible)
        {
            yield return null;
        }
        ps.Stop();
        while (ps.IsAlive())
        {
            yield return null;
        }
        gameObject.SetActive(false);
        yield break;
    }

    public void Die()
    {
        pooler.PoolObjects(fishChunks, transform.position, transform.rotation, Vector3.zero);
        gameObject.SetActive(false);
    }
}
