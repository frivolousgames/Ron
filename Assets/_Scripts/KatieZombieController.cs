using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KatieZombieController : UnfoldingEnemies
{
    [SerializeField]
    float attackDistance;
    [SerializeField]
    float attackSpeed;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    bool isScreaming;
    bool attackMove;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;
    }

    private void Start()
    {
        navAgent.enabled = false;
    }

    private void OnEnable()
    {
        navAgent.enabled = false;
        Unfold();
    }

    private void OnDisable()
    {
        navAgent.speed = walkSpeed;
        isUnfolded = false;
        transform.position = startSpawnPos;
        transform.rotation = startSpawnRot;
    }

    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isUnfolded", isUnfolded);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isScreaming", isScreaming);
        anim.SetBool("isTurning", isTurning);

        playerDistance = Vector3.Distance(transform.position, player.transform.position);
    }


    protected override IEnumerator PlayerFollowRoutine()
    {
        isTurning = true;
        float h = 0;
        while (h < turnTime)
        {
            TurnAfterAttack(h);
            h += turnSpeed;
            if (transform.rotation == turnRotation)
            {
                h = turnTime;
            }
            Debug.Log("Turning");
            yield return null;
        }
        isTurning = false;
        while (isUnfolded)
        {
            isFollowing = true;
            while (playerDistance > attackDistance)
            {
                FollowPlayer();
                yield return null;
            }
            EndFollow();
            playerPos = player.transform.position;
            Vector3 direction = (playerPos - transform.position).normalized;
            while (isAttacking)
            {
                while (attackMove)
                {
                    BaseAttack(direction);
                    yield return null;
                }
                yield return null;
            }
            //yield return new WaitForSeconds(postBaseAttackWait);
            Scream();
            while (isScreaming)
            {
                yield return null;
            }
            isTurning = true;
            float i = 0;
            while (i < turnTime)
            {
                TurnAfterAttack(i);
                i += turnSpeed;
                if(transform.rotation == turnRotation)
                {
                    i = turnTime;
                }
                yield return null;
            }
            isTurning = false;
            yield return null;
        }
    }
    protected override void Unfold()
    {
        if (!isUnfolded)
        {
            isUnfolded = true;
            isScreaming = true;
            StartCoroutine(UnfoldWait());
        }
    }

    public void SetAttackMove()
    {
        if(attackMove)
        {
            attackMove = false;
        }
        else
        {
            attackMove = true;
        }
    }
    void BaseAttack(Vector3 direction)
    {
        rb.velocity = new Vector3(direction.x, 0f, direction.z ) * attackSpeed;
        //Debug.Log("Direction: " + direction);
    }

    void Scream()
    {
        isScreaming = true;
    }

    public void ResetIsScreaming()
    {
        isScreaming = false;
    }
}
