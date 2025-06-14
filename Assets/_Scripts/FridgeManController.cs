using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FridgeManController : UnfoldingEnemies
{
    [SerializeField]
    float attackDistance;
    [SerializeField]
    float attackSpeed;
    [SerializeField]
    float attackHeight;
    [SerializeField]
    float doorAttackDistance;
    bool doorAttack;

    public bool isRunning;
    [SerializeField]
    float runWait;
    [SerializeField]
    float runTime;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float walkSpeed;

    public bool getUp;

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
    }

    private void OnDisable()
    {
        navAgent.speed = walkSpeed;
        isRunning = false;
        isUnfolded = false;
        transform.position = startSpawnPos;
        transform.rotation = startSpawnRot;
    }

    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isUnfolded", isUnfolded);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("getUp", getUp);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log("PlayerDistance: " + playerDistance);
        Unfold();
    }

    protected override IEnumerator PlayerFollowRoutine()
    {
        isTurning = true;
        float h = 0;
        while (h < 360)
        {
            TurnAfterAttack(h);
            h += turnSpeed;

            yield return null;
        }
        while (isUnfolded)
        {
            isFollowing = true;
            while (playerDistance > attackDistance)
            {
                FollowPlayer();
                yield return null;
            }
            int j = 0;
            while (j < runWait)
            {
                FollowPlayer();
                Run();
                j++;
                yield return new WaitForSeconds(runTime);
            }
            EndFollow();
            RunAttack();
            while (isAttacking)
            {
                yield return null;
            }
            yield return new WaitForSeconds(postBaseAttackWait);
            getUp = true;
            while (getUp)
            {
                yield return null;
            }
            isTurning = true;
            float i = 0;
            while (i < 360)
            {
                TurnAfterAttack(i);
                i += turnSpeed;

                yield return null;
            }
            if(playerDistance < doorAttackDistance)
            {
                if (!doorAttack)
                {
                    DoorAttack();
                    yield return null;
                }
                yield return null;
            }
            while(doorAttack)
            {
                yield return null;
            }
            yield return null;
        }
    }

    void RunAttack()
    {
        anim.SetTrigger("Launch");
        playerPos = player.transform.position;
    }
    public void Launch()
    {
        Vector3 direction = (playerPos - transform.position).normalized;
        //rb.AddForce(new Vector3(direction.x, attackHeight, direction.z) * attackSpeed, ForceMode.Impulse);
        rb.AddForce(new Vector3(transform.forward.x, attackHeight, transform.forward.z) * attackSpeed, ForceMode.Impulse);
    }

    void Run()
    {
        isRunning = true;
        navAgent.speed = runSpeed;
    }

    void Walk()
    {
        isRunning = false;
        navAgent.speed = walkSpeed;
    }

    void GotUp()
    {
        getUp = false;
        Walk();
    }

    void DoorAttack()
    {
        doorAttack = true;
        anim.SetTrigger("doorAttack");
        isAttacking = true;
    }
    void EndDoorAttack()
    {
        doorAttack = false;
        ResetIsAttacking();
    }
}
