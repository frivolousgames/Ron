using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRagdollController : MonoBehaviour
{
    [SerializeField]
    Transform frontTrans;
    [SerializeField]
    Transform backTrans;

    [SerializeField]
    float velocity;

    ParticleSystem ps;

    Transform player;

    float dir;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        SetDirection();
        SetParticleDirection();
    }

    void SetDirection()
    {
        if(Vector3.Distance(player.position, frontTrans.position) < Vector3.Distance(player.position, backTrans.position))
        {
            dir = -velocity;
        }
        else
        {
            dir = velocity;
        }
    }

    void SetParticleDirection()
    {
        var velocity = ps.velocityOverLifetime;
        velocity.z = dir;
    }
}
