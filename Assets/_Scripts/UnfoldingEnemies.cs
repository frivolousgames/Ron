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
        StartCoroutine(PlayerFollowRoutine());
        yield break;
    }
    
    public void UnfoldComplete() //Create animation event at end of unfold state with this function
    {
        unfoldComplete = true;
    }

    
}
