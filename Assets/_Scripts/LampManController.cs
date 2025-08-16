using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class LampManController : UnfoldingEnemies
{
    [SerializeField]
    protected Light attackLight;
    [SerializeField]
    protected GameObject lightEffectObject;
    protected VisualEffect lightEffect;
    [SerializeField]
    protected float lightRangeMax;
    [SerializeField]
    protected float lightRangeMin;
    [SerializeField]
    protected float lightDelay;
    [SerializeField]
    protected float glowSpeed;

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
        lightEffect = lightEffectObject.GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        navAgent.enabled = false;
        attackLight.range = lightRangeMin;
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
        anim.SetBool("isFrozen", isFrozen);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        isPoopedOn = !gEnemyScript.isTargetable;
        //Debug.Log("isFrozen: " + isFrozen);
        Unfold();
        ReactToPlayer();
        GoochHit(PlayerFollowRoutine());
    }

    protected override IEnumerator PlayerFollowRoutine()
    {
        while(!isFrozen)
        {
            float h = 0;
            while (h < 10 && !isFrozen)
            {
                TurnAfterAttack(h);
                h += turnSpeed;

                yield return null;
            }
            while (unfoldComplete && !isFrozen)
            {
                isFollowing = true;
                navAgent.enabled = true;
                rb.isKinematic = true;
                while (isFollowing && !isFrozen)
                {
                    //Debug.Log("Following");
                    FollowPlayer();
                    if (playerDistance < baseAttackDistance)
                    {
                        EndFollow();
                        yield return null;
                    }
                    yield return null;
                }
                LightAttack();
                while (isAttacking && !isFrozen)
                {
                    yield return null;
                }
                yield return new WaitForSeconds(postBaseAttackWait);
                float i = 0;

                while (i < 10 && !isFrozen)
                {
                    TurnAfterAttack(i);
                    i += turnSpeed;

                    yield return null;
                }
                isFollowing = true;
                yield return null;
            }
        }
        Debug.Log("Break");
        yield break;
    }

    void LightAttack()
    {
        if(!isFrozen)
        {
            anim.SetTrigger("LightAttack");
            playerPos = player.transform.position;
            StartCoroutine(LightGlow());
        } 
    }
    IEnumerator LightGlow()
    {
        while (!isFrozen)
        {
            while (attackLight.range < lightRangeMax && !isFrozen)
            {
                attackLight.range += glowSpeed * Time.deltaTime;
                yield return null;
            }
            lightEffect.Play();
            yield return new WaitForSeconds(lightDelay);
            lightEffect.Stop();
            while (attackLight.range > lightRangeMin && !isFrozen)
            {
                attackLight.range -= glowSpeed * Time.deltaTime * 2;
                yield return null;
            }
            attackLight.range = lightRangeMin;
            yield break;
        }
        attackLight.range = lightRangeMin;
        lightEffect.Stop();
        yield break;
    }
    protected override void TurnAfterAttack(float i)
    {
        if (!isFrozen)
        {
            //Debug.Log("Turning");
            isTurning = true;
            Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
        } 
    }

    protected override IEnumerator GoochReset(IEnumerator attackRoutine)
    {
        if(unfoldComplete)
        {
            yield return new WaitForSeconds(.4f);
            StartCoroutine(attackRoutine);
            yield break;
        }
        else
        {
            Debug.Log("Not Unfolded");
            yield break;
        }
    }
}
