using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GingerbreadManController : UnfoldingEnemies
{
    [SerializeField]
    Animator whipAnim;

    [SerializeField]
    float walkSpeed;
    [SerializeField]
    bool isIdling;
    [SerializeField]
    float kickDistance;

    [SerializeField]
    bool kickFront;
    [SerializeField]
    bool kickBack;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        //rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
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
        anim.SetBool("isIdling", isIdling);
        anim.SetBool("isTurning", isTurning);
        anim.SetBool("kickFront", kickFront);
        anim.SetBool("kickBack", kickBack);

        whipAnim.SetBool("isMoving", isMoving);
        whipAnim.SetBool("isUnfolded", isUnfolded);
        whipAnim.SetBool("isHit", isHit);
        whipAnim.SetBool("isAttacking", isAttacking);
        whipAnim.SetBool("isIdling", isIdling);
        whipAnim.SetBool("isTurning", isTurning);
        whipAnim.SetBool("kickFront", kickFront);
        whipAnim.SetBool("kickBack", kickBack);

        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        Unfold();
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
            //Debug.Log("Turning");
            yield return null;
        }
        isTurning = false;
        while (isUnfolded)
        {
            isFollowing = true;
            while (playerDistance > baseAttackDistance)
            {
                FollowPlayer();
                yield return null;
            }
            EndFollow();
            playerPos = player.transform.position;
            Vector3 direction = (playerPos - transform.position).normalized;
            while (isAttacking)
            {
                yield return null;
            }
            isTurning = true;
            float i = 0;
            while (i < turnTime)
            {
                TurnAfterAttack(i);
                i += turnSpeed;
                if (transform.rotation == turnRotation)
                {
                    i = turnTime;
                }
                yield return null;
            }
            isTurning = false;
            kickFront = false;
            kickBack = false;
            Idle();
            while (isIdling)
            {
                if(playerDistance < kickDistance)
                {
                    SetHitDirection();
                    if (hitFront)
                    {
                        kickFront = true;
                        kickBack = false;
                    }
                    else
                    {
                        kickBack = true;
                        kickFront = false;
                    }
                }
                yield return null;
            }
            kickFront = false;
            kickBack = false;
            isTurning = true;
            float j = 0;
            while (j < turnTime)
            {
                TurnAfterAttack(j);
                j += turnSpeed;
                if (transform.rotation == turnRotation)
                {
                    j = turnTime;
                }
                yield return null;
            }
            isTurning = false;
            yield return null;
        }
    }

    void Idle()
    {
        isIdling = true;
    }

    public void ResetIsIdling()
    {
        isIdling = false;
    }
}
