using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody rb;
    protected BoxCollider hitCol;
    protected NavMeshAgent navAgent;
    [SerializeField]
    protected Transform spawnPos;
    protected Quaternion startSpawnRot;
    protected Vector3 startSpawnPos;

    [SerializeField]
    protected LayerMask playerRaycastLayer;
    protected GameObject player;
    protected Vector3 playerPos;
    protected float playerDistance;
    [SerializeField]
    protected GameObject playerRagdoll;

    protected bool isIdle;
    protected bool isMoving;
    protected bool isWalking;
    protected bool isTurning;
    protected bool isFollowing;
    protected bool isAttacking;
    protected bool isDestinationReached;
    protected bool isFrozen;
    protected bool isDead;
    protected bool playerDetected;

    protected bool isHit;
    protected bool hitWait;
    protected bool hitFront;
    protected float hitDirection;
    [SerializeField]
    protected float hitBackPower;
    [SerializeField]
    protected Transform frontTrans;
    [SerializeField]
    protected Transform backTrans;
    protected Vector3 frontDistance;
    protected Vector3 backDistance;

    protected Quaternion turnRotation;
    protected Vector3 turnDirection;
    [SerializeField]
    protected float turnSpeed;
    [SerializeField]
    protected float turnTime;
    [SerializeField]
    protected float baseAttackDistance;
    [SerializeField]
    protected float postBaseAttackWait;
    [SerializeField]
    protected float chaseDistanceMin;
    [SerializeField]
    protected float chaseDistanceMax;

    //Material Color
    [SerializeField]
    protected SkinnedMeshRenderer[] renderers;
    [SerializeField]
    protected UnityEngine.Color hitColor;
    protected List<UnityEngine.Color> originalColors;
    [SerializeField]
    protected float hitColorWaitTime;

    ///Die///
    [SerializeField]
    protected GameObject ragdoll;
    [SerializeField]
    protected Rigidbody[] ragdollRbs;

    [SerializeField]
    protected float ragdollSpeed;
    public static Vector3 ragdollDirection;

    //OnInvisible
    [SerializeField]
    protected float invisibleTimeThresh;
    protected float invisbleTime;
    protected bool isInvisible;
    protected bool isReset;
    protected Coroutine invisibleRoutine;

    ///Gooch Poop
    protected bool isPoopedOn;
    protected bool poopWait;
    [SerializeField]
    protected GoochEnemy gEnemyScript;

    protected virtual void ReactToPlayer()
    {
        if (playerDistance < chaseDistanceMin && !isFrozen && !playerDetected)
        {
            playerDetected = true;
            isFrozen = false;
            StartCoroutine(PlayerFollowRoutine());
        }
        if (playerDistance > chaseDistanceMax && !isFrozen)
        {
            playerDetected = false;
            isFrozen = true;
        }
    }
    public virtual void IsHit() //Put BodyColliderHit script on all hit colliders. Link this to the events on each body collider script
    {
        if (!hitWait)
        {
            StartCoroutine(HitWaitReset());
            isHit = true;
            hitWait = true;
            SetHitDirection();
            HitColor();
            if (hitFront)
            {
                hitDirection = -1f;
                anim.SetTrigger("hitFront");
                //Debug.Log("Hit Front");
            }
            else
            {
                hitDirection = 1f;
                anim.SetTrigger("hitBack");
                //Debug.Log("Hit Back");
            }
            //HitKnockBack();
        }
    }
    protected virtual void SetHitDirection()
    {
        frontDistance = frontTrans.position - player.transform.position;
        backDistance = backTrans.position - player.transform.position;
        if (Mathf.Abs(frontDistance.x) < Mathf.Abs(backDistance.x))
        {
            hitFront = true;
        }
        else
        {
            hitFront = false;
        }
    }

    protected virtual IEnumerator HitWaitReset()
    {
        yield return new WaitForSeconds(.1f);
        hitWait = false;
    }
    //Put Animation event at end of both "Hit" animations with this function
    public virtual void ResetIsHit()
    {
        isHit = false; //below variables commented out until HitKnockBack is worked out
        //rb.isKinematic = true;
        //rb.velocity = Vector3.zero;
        //if (navAgent != null)
        //{
        //    navAgent.enabled = true;
        //}
    }
    //Put Animation event at end of "Attack" animation with this function
    public virtual void ResetIsAttacking()
    {
        isAttacking = false;
    }
    //Create 2 transforms and place at front and back of character
    

    protected virtual void HitKnockBack()
    {
        if (rb != null)
        {
            rb.isKinematic = false;

        }
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }
        Vector3 direction = player.transform.position - transform.position;
        rb.AddForce(new Vector3(direction.x, 0, direction.z).normalized * -hitBackPower, ForceMode.Impulse);
        //Debug.Log("Knock Back Dir: " + new Vector3(direction.x, 0, direction.z).normalized * hitDirection * hitBackPower);
    }

    protected virtual void TurnAfterAttack(float i)
    {
        //isTurning = true;
        Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
    }

    public virtual void Die()
    {
        isDead = true;
        if(ragdoll != null)
        {
            ragdoll.transform.position = transform.position;
            ragdoll.transform.rotation = transform.rotation;
            ragdollDirection = (player.transform.position - transform.position).normalized;
            ragdoll.SetActive(true);
            foreach(Rigidbody rb in ragdollRbs)
            {
                rb.AddForce(-ragdollDirection * ragdollSpeed, ForceMode.Impulse);
            }
            //Debug.Log(ragdollDirection);
            gameObject.SetActive(false);
        }
    }

    protected virtual void HitColor()
    {
        foreach (SkinnedMeshRenderer smr in renderers)
        {
            foreach (Material mat in smr.materials)
            {
                originalColors.Add(mat.color);
                mat.color = hitColor;
            }
        }
        StartCoroutine(HitColorReset());
    }
    protected virtual IEnumerator HitColorReset()
    {
        yield return new WaitForSeconds(hitColorWaitTime);
        int i = 0;
        foreach (SkinnedMeshRenderer smr in renderers)
        {
            foreach (Material mat in smr.materials)
            {
                mat.color = originalColors[i];
                i++;
                yield return null;
            }
            yield return null;
        }
        yield break;
    }

    ///Player Follow
    protected virtual IEnumerator PlayerFollowRoutine() //Base follow and attack coroutine overridden by each enemy
    {
        while (true)
        {
            yield return null;
        }
    }

    protected virtual void FollowPlayer()//Turns on nav agent so following player is enabled.
    {
        navAgent.enabled = true;
        //Debug.Log("OnMesh: " + navAgent.isOnNavMesh);
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        isMoving = true;
        isTurning = false;
        navAgent.destination = player.transform.position;
        transform.LookAt(new Vector3(navAgent.destination.x, transform.position.y, navAgent.destination.z));
    }

    protected virtual void EndFollow() //Turns off nav agent so following player is disabled. 
    {
        navAgent.enabled = false;
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        isFollowing = false;
        isMoving = false;
        isAttacking = true;
    }

    //Invisible

    protected virtual IEnumerator InvisibleCounter()
    {
        yield return new WaitForSeconds(invisibleTimeThresh);
        isReset = true;
        yield break;
    }

    ///Gooch Poop
    protected virtual void GoochHit(IEnumerator attackRoutine)
    {
        if (isPoopedOn && !poopWait)
        {
            poopWait = true;
            isFrozen = true;
            navAgent.enabled = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            isFollowing = false;
            anim.speed = 0f;
            isAttacking = false;
            isMoving = false;
            StartCoroutine(GoochHitWait(attackRoutine));
        }
    }

    protected virtual IEnumerator GoochHitWait(IEnumerator attackRoutine)
    {
        while(isPoopedOn)
        {
            navAgent.enabled = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            isFollowing = false;
            isAttacking = false;
            isMoving = false;
            yield return null;
        }
        poopWait = false;
        isFrozen = false;
        anim.speed = 1f;
        StartCoroutine(GoochReset(attackRoutine));
        Debug.Log("Reset");
        yield break;
    }

    protected virtual IEnumerator GoochReset(IEnumerator attackRoutine)
    {
        yield return new WaitForSeconds(.4f);
        StartCoroutine(attackRoutine);
        yield break;
    }

    public void ResetIsFrozen()
    {
        isFrozen = false;
    }
}
