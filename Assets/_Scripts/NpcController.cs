using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    protected bool isIdle;
    protected bool isWalking;
    protected bool isInteracting;
    protected bool isTalking;
    protected bool isMovingRight;
    protected bool isFollowing;
    protected bool isDestinationReached;
    protected bool isHit;
    protected bool isCowering;
    protected bool isDead;

    protected Vector3 destination;
    protected Vector3 followPosition;

    protected float followOffsetX;


    protected Animator anim;
    protected Rigidbody rb;
    protected BoxCollider hitCol;


    public virtual void WalkRoutine()
    {
       
    }

    public virtual void StartInteraction()
    {

    }

    public virtual void Talk()
    {

    }

     public virtual void Die()
    {

    }
}
