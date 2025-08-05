using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnfoldingEnemies : EnemyController
{
    public bool isUnfolded;
    protected bool unfoldComplete;

    [SerializeField]
    protected float unfoldDistance;

    protected Coroutine playerFollowRoutine;

    protected virtual void Unfold() //Place in update to trigger unfolding by distance
    {
        if (playerDistance < unfoldDistance && !isFrozen)
        {
            if (!isUnfolded)
            {
                isUnfolded = true;
                StartCoroutine(UnfoldWait());
            }
        }
    }

    protected virtual IEnumerator UnfoldWait()
    {
        while(!unfoldComplete)
            {
                yield return null;
            }
        playerDetected = true;
        playerFollowRoutine = StartCoroutine(PlayerFollowRoutine());
        yield break;
    }
    
    public void UnfoldComplete() //Create animation event at end of unfold state with this function
    {
        unfoldComplete = true;
    }

    protected virtual void ResetIsFolded()
    {
        if(isReset == true)
        {
            Debug.Log("Resetting");
            isUnfolded = false;
            unfoldComplete = false;
            isAttacking = false;
            isFollowing = false;
            isMoving = false;
            isWalking = false;
            isTurning = false;
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
            if (navAgent != null)
            {
                navAgent.enabled = false;
            }
            transform.position = startSpawnPos;
            transform.rotation = startSpawnRot;
            isReset = false;
            if(playerFollowRoutine != null)
            {
                StopCoroutine(playerFollowRoutine);
            }
        }
    }
}
