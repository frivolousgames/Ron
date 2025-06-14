using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutRoller : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float speed;

    [SerializeField]
    Transform mesh;

    [SerializeField]
    float rotSpeed;

    Transform player;

    float playerDistance;
    [SerializeField]
    float attackDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float zRot = rb.velocity.x * rotSpeed;
        mesh.Rotate(new Vector3(0f, 0f, zRot));
        playerDistance = Vector3.Distance(transform.position, player.position);
    }

    private void FixedUpdate()
    {
        StartRolling();
    }

    void StartRolling()
    {
        if(playerDistance < attackDistance)
        {
            rb.velocity = new Vector3(transform.forward.x * speed, rb.velocity.y, 0f);
        }
    }
}
