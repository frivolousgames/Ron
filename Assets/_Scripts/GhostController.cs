using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : UnfoldingEnemies
{
    [SerializeField]
    bool isPooping;
    [SerializeField]
    bool isPeeing;
    [SerializeField]
    bool isLaughing;

    [SerializeField]
    float navWalkSpeed;
    [SerializeField]
    int navTurnSpeed;

    int attackType;
    Vector3 attackPlayerOffset;
    [SerializeField]
    float attackPosOffset;

    ObjectPooler pooler;

    //PEE//
    [SerializeField]
    Vector3 peePosOffset;
    [SerializeField]
    float peeSpeed;
    [SerializeField]
    Quaternion peeRotation;
    [SerializeField]
    float peeRotSpeed;
    [SerializeField]
    Vector3 peeReturnOffset;
    [SerializeField]
    Quaternion peeReturnRot;

    //POOP//

    [SerializeField]
    GameObject[] poop;
    [SerializeField]
    Transform poopSpawn;
    [SerializeField]
    float poopPosOffset;
    [SerializeField]
    int poopAmount;
    [SerializeField]
    float poopTurnSpeed;
    [SerializeField]
    float navPoopMoveSpeed;
    [SerializeField]
    float navPoopTurnSpeed;

    [SerializeField]
    [Tooltip("Transform that represents the farthest point of the ground area")]
    Transform groundAreaBack;
    [SerializeField]
    [Tooltip("Transform that represents the nearset point of the ground area")]
    Transform groundAreaFront;
    [SerializeField]
    [Tooltip("Offset distance from back and front of ground area. Helps determines the size of the area for the ghost to poop")]
    float groundAreaOffset;
    float zMax;
    float zMin;
    float groundMidpoint;
    int groundSection;
    float poopDestZ = 0;

    NavMeshObstacle[] navMeshObstacles;

    [SerializeField]
    [Tooltip("Offset from the player's X axis while in the post attack 'Wait' stage")]
    float waitPosOffset;
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;

        pooler = new ObjectPooler();

        zMax = groundAreaBack.position.z - groundAreaOffset;
        zMin = groundAreaFront.position.z + groundAreaOffset;
        groundMidpoint = (zMax + zMin) / 2;

        navMeshObstacles = FindObjectsOfType<NavMeshObstacle>();
    }

    private void Start()
    {
        navAgent.enabled = false;
    }

    private void OnEnable()
    {
        isDead  = false;
        navAgent.enabled = false;
        transform.position = startSpawnPos;
        transform.rotation = startSpawnRot;
        attackType = 0;
        groundSection = 0;
    }

    private void OnDisable()
    {
        isUnfolded = false;
    }

    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isLaughing", isLaughing);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isUnfolded", isUnfolded);
        anim.SetBool("isPooping", isPooping);
        anim.SetBool("isPeeing", isPeeing);
        anim.SetBool("isTurning", isTurning);

        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        Unfold();
    }

    protected override IEnumerator PlayerFollowRoutine()
    {
        isTurning = true;
        float h = 0;
        while (h < turnTime)// After unfolding (coming out of ground) turn towards player
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
        isLaughing = true;// After turning start laughing
        isAttacking = true;
        while (isAttacking)
        {
            yield return null;
        }
        isLaughing = false;
        while (isUnfolded)//Start attack loop on isUnfolded = true
        {
            if(attackType == 1) //Attack Type 1 = Pee Routine. This loops after first and only Poop Routine. 
            {
                isFollowing = true;
                while (playerDistance > baseAttackDistance)// Move towards player until basAttackDistance is reached
                {
                    FollowPlayer();
                    yield return null;
                }
                EndFollow();// turn off navAgent
                while (transform.position.y < peePosOffset.y - .5f)// Fly up to pee position
                {
                    MoveToPlayerZPosition(peePosOffset, peeSpeed);
                    MoveToRotation(peeRotation, peeRotSpeed);
                    yield return null;
                }
                isMoving = false;
                isPeeing = true;
                while (isAttacking)// Start peeing
                {
                    MoveToPlayerZPosition(peePosOffset, peeSpeed);
                    yield return null;
                }
                isPeeing = false;// Stop peeing
                yield return null;
                isMoving = true;
                while (transform.position.y > startSpawnPos.y + .05f)// Fly down to pee return position
                {
                    MoveToPlayerZPosition(peeReturnOffset, peeSpeed);
                    MoveToRotation(peeReturnRot, peeRotSpeed);
                    yield return null;
                }
                navAgent.enabled = true;// Turn on navAgent for next step
            }
            else
            {
                Debug.Log("Poop Attack Started");
                attackType = 1;// Attack Type 2 = Poop Routine. Loops through once before Pee Routine takes over
                navAgent.enabled = true;
                int j = 0;
                while(j < poopAmount)// Start pooping loop which runs for poopAmount times
                {
                    navAgent.speed = navPoopMoveSpeed;
                    navAgent.angularSpeed = navPoopTurnSpeed;
                    isAttacking = true;
                    SetPoopDestination();// Set random positions for ghost to move to
                    Debug.Log("Poop destination set");
                    if(navMeshObstacles != null)
                    {
                        foreach (NavMeshObstacle obs in navMeshObstacles)// Check for obstacles in the area and recursively change destination if necessary
                        {
                            if (Vector3.Distance(navAgent.destination, obs.transform.position) < 2)
                            {
                                SetPoopDestination();
                                yield return null;
                            }
                            yield return null;
                        }
                        yield return null;
                    }
                    while(Vector3.Distance(navAgent.destination, navAgent.transform.position) > 2)// Move towards poop spot
                    {
                        Debug.Log("Moving towards poop spot");
                        isMoving = true;
                        yield return null;
                    }
                    isMoving = false;
                    navAgent.enabled = false;
                    isTurning = true;
                    turnRotation = Quaternion.LookRotation(-Vector3.right);
                    while(transform.rotation != turnRotation)// Turn to poop rotation preparing to poop
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, poopTurnSpeed * Time.deltaTime);
                        Debug.Log("Turning into poop rotation: " + transform.rotation.eulerAngles);
                        yield return null;
                    }                  
                    isTurning = false;
                    isPooping = true;
                    while (isAttacking)// Start poop animation (lay poops)
                    {
                        Debug.Log("Pooping..");
                        yield return null;
                    }
                    isPooping = false;
                    navAgent.enabled = true;
                    j++;
                    Debug.Log("Restarting");
                    yield return null;
                }
            }
            navAgent.speed = navWalkSpeed;// Set navAgent speed back to walk speed from poop speed
            navAgent.angularSpeed = navTurnSpeed;// Set navAgent turn speed back to normal speed from poop turn speed
            navAgent.destination = player.transform.position;
            while (navAgent.pathStatus != NavMeshPathStatus.PathComplete)// Move towards wait position
            {
                SetWaitPosition();
                yield return null;
            }
            isMoving = false;
            navAgent.enabled = false;
            isTurning = true;
            float i = 0;
            while (i < turnTime)// Turn towards player
            {
                TurnAfterAttack(i);
                i += turnSpeed;
                if (transform.rotation == turnRotation)
                {
                    i = turnTime;
                }
                yield return null;
            }
            isTurning = false;
            isLaughing = true;// Start laugh animation
            isAttacking = true;
            while(isAttacking)
            {
                yield return null;
            }
            isLaughing = false;
            yield return null;
        }
    }

    protected override void FollowPlayer()//Turns on nav agent so following player is enabled.
    {
        navAgent.enabled = true;
        Debug.Log("OnMesh: " + navAgent.isOnNavMesh);
        isMoving = true;
        isTurning = false;
        attackPlayerOffset = new Vector3(player.transform.position.x + attackPosOffset, player.transform.position.y, player.transform.position.z);
        navAgent.destination = attackPlayerOffset;
        transform.LookAt(new Vector3(navAgent.destination.x, transform.position.y, navAgent.destination.z));
    }

    protected override void EndFollow() //Turns off nav agent so following player is disabled. 
    {
        navAgent.enabled = false;
        isFollowing = false;
        isAttacking = true;
    }

    private void SetPoopDestination()
    {
        if (groundSection == 0)
        {
            poopDestZ = Random.Range(groundMidpoint, zMax);
            groundSection = 1;
            Debug.Log("Max: " + poopDestZ);
        }
        else
        {
            poopDestZ = Random.Range(zMin, groundMidpoint);
            groundSection = 0;
            Debug.Log("Min: " + poopDestZ);
        }
        navAgent.destination = new Vector3(transform.position.x + poopPosOffset, transform.position.y, poopDestZ);
    }

    public void PoopCreate()
    {
        pooler.PoolObjects(poop, poopSpawn.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), Vector3.zero);
    }

    private void MoveToPlayerZPosition(Vector3 offset, float speed)
    {
        Vector3 pos = new Vector3(player.transform.position.x + offset.x, offset.y, player.transform.position.z);
        Vector3 newPos = Vector3.Slerp(transform.position, pos, speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

    private void MoveToRotation(Quaternion rotation, float speed)
    {
        Quaternion newRot = Quaternion.Lerp(transform.rotation, peeRotation, peeRotSpeed * Time.deltaTime);
        rb.MoveRotation(newRot);
    }

    private void SetWaitPosition()
    {
        navAgent.enabled = true;
        isMoving = true;
        attackPlayerOffset = new Vector3(player.transform.position.x + waitPosOffset, player.transform.position.y, player.transform.position.z);
        navAgent.destination = attackPlayerOffset;
    }

    public override void Die()
    {
        isDead = true;
        ragdoll.SetActive(true);
        gameObject.SetActive(false);
    }
}
