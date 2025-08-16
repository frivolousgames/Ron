using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BloodStreakController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float speed;
    float velocity;
    ParticleSystem ps;
    ParticleSystem.MainModule mainModule;
    [SerializeField]
    float stopDelay;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
    }

    private void OnEnable()
    {
        velocity = speed;
        mainModule.loop = true;
        StartCoroutine(StopWait());
    }

    private void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * velocity;
    }

    IEnumerator StopWait()
    {
        yield return new WaitForSeconds(stopDelay);
        mainModule.loop = false;
        velocity = 0f;
        yield break;
    }
}
