using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DiverController : UnfoldingEnemies
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    Transform arrowSpawn;

    //[SerializeField]
    //bool isReloading;

    ObjectPooler pooler;

    //Arm//
    [SerializeField]
    Transform armTrans;
    Quaternion lookRot;
    bool isAiming;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player: " + player.name);
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;

        pooler = new ObjectPooler();
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
            isAiming = true;
            //StartCoroutine(ArmTurn());
            while (isAttacking)
            {
                yield return null;
            }
            //while (!isReloading)
            //{
            //    yield return null;
            //}
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
    //protected override void TurnAfterAttack(float i)
    //{
    //    //isTurning = true;
    //    Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    //    turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
    //}

    //public void ResetIsReloading()
    //{
    //    isReloading = false;
    //}
    public void Shoot()
    {
        pooler.PoolObjects(PooledObjectArrays.milkBulletArrays, arrowSpawn.position, arrowSpawn.rotation, Vector3.zero);
    }

    //private void LateUpdate()
    //{
    //    if (isAiming)
    //    {
    //        Debug.Log("Aiming");
    //        lookRot = Quaternion.RotateTowards(armTrans.localRotation, Quaternion.LookRotation(player.transform.position, Vector3.up), 360f * Time.deltaTime * turnSpeed);
    //        //armTrans.localRotation = lookRot;
    //        armTrans.localRotation = Quaternion.LookRotation(player.transform.position, Vector3.up);

    //    }
    //}
    IEnumerator ArmTurn()
    {
        while (isAiming)
        {
            lookRot = Quaternion.RotateTowards(armTrans.localRotation, Quaternion.LookRotation(player.transform.position, Vector3.up), 360f * Time.deltaTime * turnSpeed);
            //armTrans.localRotation = lookRot;
            armTrans.localRotation = Quaternion.LookRotation(player.transform.position, Vector3.up);
            yield return null;
        }
        yield break;
    }

    public void ResetIsAiming()
    {
        isAiming = false;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isAiming)
        {
            armTrans.localRotation = Quaternion.LookRotation(player.transform.position, Vector3.up);
        }
    }

    public override void Die()
    {
        isDead = true;
        if (ragdoll != null)
        {
            ragdoll.transform.position = transform.position;
            ragdoll.transform.rotation = transform.rotation;
            ragdoll.SetActive(true);
            //Debug.Log(ragdollDirection);
            gameObject.SetActive(false);
        }
    }
}
