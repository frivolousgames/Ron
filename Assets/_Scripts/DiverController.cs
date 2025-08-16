using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DiverController : UnfoldingEnemies
{
    [SerializeField]
    float moveSpeed;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player: " + player.name);
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;
    }

    private void Start()
    {
        //navAgent.enabled = false;
    }

    private void OnEnable()
    {
        //navAgent.enabled = false;
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
        ResetIsFolded();
    }

    protected override IEnumerator PlayerFollowRoutine()
    {
        Debug.Log("Started");
        isTurning = true;
        float h = 0;
        while (h < 10)
        {
            TurnAfterAttack(h);
            h += turnSpeed;

            yield return null;
        }
        isTurning = false;
        while (isUnfolded)
        {
            isFollowing = true;
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
            Shoot();
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

    void Shoot()
    {

    }
    //protected override void TurnAfterAttack(float i)
    //{
    //    //isTurning = true;
    //    Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    //    turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
    //}

    private void OnBecameInvisible()
    {
        if (isUnfolded && gameObject.activeInHierarchy)
        {
            Debug.Log("Invisible");
            invisibleRoutine = StartCoroutine(InvisibleCounter());
        }
    }

    private void OnBecameVisible()
    {
        if(isUnfolded)
        {
            if(invisibleRoutine != null)
            {
                StopCoroutine(invisibleRoutine);
            }
        }
    }

    protected override void FollowPlayer()
    {
        isMoving = true;
        rb.MovePosition(Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime));
    }

    protected override void EndFollow()
    {
        isFollowing = false;
        isMoving = false;
        isAttacking = true;
    }
}
