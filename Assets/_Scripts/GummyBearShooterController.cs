using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GummyBearShooterController : EnemyController
{
    //[SerializeField]
    //bool isShooting;
    [SerializeField]
    bool isRaising;
    [SerializeField]
    bool isLowering;
    [SerializeField]
    bool isGunRaised;
    bool isReady;

    ObjectPooler pooler;

    [SerializeField]
    [Tooltip("Wait time before initial shooting starts")]
    float shootStartWait;
    [SerializeField]
    [Tooltip("Minimum wait time for random shooting")]
    float shootWaitMin;
    [SerializeField]
    [Tooltip("Maximum wait time for random shooting")]
    float shootWaitMax;

    //Shooting//
    [SerializeField]
    Transform squirtSpawn;

    //GameObject[] squirts;
    [SerializeField]
    GameObject squirt;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;

        //squirts = PooledObjectArrays.squirtsArray;

        pooler = new ObjectPooler();
    }
    private void OnEnable()
    {
        isDead = false;
        transform.position = startSpawnPos;
        transform.rotation = startSpawnRot;
    }


    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isRaising", isRaising);
        anim.SetBool("isLowering", isLowering);
        //anim.SetBool("isShooting", isShooting);
        anim.SetBool("isTurning", isTurning);

        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        AttackStance();
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = false;
        isLowering = false;
        //isShooting = false;

        isTurning = true;
        float h = 0;
        while (h < turnTime)// Turn towards player
        {
            TurnAfterAttack(h);
            
            h += turnSpeed;
            
            if (transform.rotation == turnRotation)
            {
                h = turnTime;

            }
            yield return null;
        }
        isTurning = false;

        isRaising = true;// While raising gun wait
        while (isRaising)
        {
            yield return null;
        }
        isRaising = false;

        isAttacking = true;// After gun raised start attacking
        StartCoroutine(AttackTurn());
        yield return new WaitForSeconds(shootStartWait);
        while (isAttacking)//Shoot gun at random intervals
        {
            anim.SetTrigger("Shoot");
            yield return new WaitForSeconds(Random.Range(shootWaitMin, shootWaitMax));
        }
    }

    private IEnumerator EndAttackRoutine()
    {
        isRaising = false;
        isAttacking = false;
        isTurning = false;

        isLowering = true;
        while (isLowering)
        {
            yield return null;
        }
        isLowering = false;

        isTurning = true;
        float i = 0;
        while (i < turnTime)// Turn back to idle rotation
        {
            //TurnAfterAttack(i);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startSpawnRot, i);
            i += turnSpeed;
            //Debug.Log("Turning: " + i);
            //Debug.Log("TurnRotation: " + turnRotation + " " + startSpawnRot);
            if (transform.rotation == startSpawnRot)
            {
                //Debug.Log("Turnt");
                i = turnTime;
            }
            yield return null;
        }
        isTurning = false;
    }

    void AttackStance()
    {
        if (playerDistance < baseAttackDistance)
        {
            if (!isReady)
            {
                isReady = true;
                StopCoroutine(EndAttackRoutine());
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            if (isReady)
            {
                isReady = false;
                StopCoroutine(AttackRoutine());
                StartCoroutine(EndAttackRoutine());
            }
        }
    }
    protected override void TurnAfterAttack(float i)
    {
        if (isReady)
        {
            Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
        }
    }
    void TurnWhileAttacking(float i)
    {
        //Debug.Log("Turning: " + transform.rotation + " " + turnRotation);
        isTurning = true;
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
    }
    IEnumerator AttackTurn()
    {
        float i = 0;
        while (isAttacking)
        {
            Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            if(turnRotation != transform.rotation)
            {
                TurnWhileAttacking(i);
                i += turnSpeed;
            }
            else
            {
                isTurning = false;
            }
            yield return null;
        }
    }
     public void ResetIsRaising()
    {
        isRaising = false;
    }
    public void ResetIsLowering()
    {
        isLowering = false;
    }

    ///Shooting///
    
    public void Shoot()
    {
        //pooler.PoolObjects(squirts, squirtSpawn.position, squirtSpawn.rotation, Vector3.zero);
        Instantiate(squirt, squirtSpawn.position, squirtSpawn.rotation);
    }
}
