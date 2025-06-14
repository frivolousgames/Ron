using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class BroomManController : UnfoldingEnemies
{
    [SerializeField]
    float sweepTurnSpeed;

    [SerializeField]
    GameObject vfxObject;
    VisualEffect sweepFX;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;
    }

    private void Start()
    {
        navAgent.enabled = false;
        sweepFX = vfxObject.GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        navAgent.enabled = false;
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
        anim.SetBool("isTurning", isTurning);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        Unfold();
    }

    protected override IEnumerator PlayerFollowRoutine()
    {
        isTurning = true;
        float h = 0;
        while (h < 10)
        {
            TurnAfterAttack(h);
            h += turnSpeed;

            yield return null;
        }
        while (isUnfolded)
        {
            isFollowing = true;
            navAgent.enabled = true;
            rb.isKinematic = true;
            while (isFollowing)
            {
                FollowPlayer();
                if (playerDistance < baseAttackDistance)
                {
                    EndFollow();
                    yield return null;
                }
                yield return null;
            }
            SweepAttack();
            while (isAttacking)
            {
                yield return null;
            }
            yield return new WaitForSeconds(postBaseAttackWait);
            float i = 0;
            isTurning = true;
            while (i < 10)
            {
                TurnAfterAttack(i);
                i += turnSpeed;

                yield return null;
            }
            isFollowing = true;
            yield return null;
        }
    }

    void SweepAttack()
    {
        anim.SetTrigger("baseAttack");
        playerPos = player.transform.position;
        StartCoroutine(SweepFollow(sweepTurnSpeed * Time.deltaTime));
    }

    IEnumerator SweepFollow(float i)
    {
        while(isAttacking)
        {
            Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
            yield return null;
        }
        yield break;
    }

    public void SweepDust()
    {
        sweepFX.Play();
    }
}

