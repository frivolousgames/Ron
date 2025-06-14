using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class DemonChildClimber : UnfoldingEnemies
{

    [SerializeField]
    bool playerReady;

    [SerializeField]
    bool isClimbing;

    [SerializeField]
    bool atTop;

    [SerializeField]
    float climbSpeed;

    [SerializeField]
    float atTopSpeed;

    [SerializeField]
    float atTopInSpeed;

    [SerializeField]
    float topPosY;
    [SerializeField]
    float topPosZ;

    [SerializeField]
    float attackDistance;

    [SerializeField]
    float attackForce;

    [SerializeField]
    GameObject physCol;

    private void Awake()
    {
        
    }

    private void OnEnable()

    {
        isClimbing = true;
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ClimbRoutine());
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        anim.SetBool("isClimbing", isClimbing);
        anim.SetBool("atTop", atTop);
        anim.SetBool("isTurning", isTurning);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("playerActivated", playerReady);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        playerReady = JunkyardSceneController.playerReady;
    }

    IEnumerator ClimbRoutine()
    {
        navAgent.enabled = false;
        
        while (isClimbing)
        {
            Climb();
            yield return null;
        }
        while (transform.localPosition.y < topPosY)
        {
            ClimbUp();
            yield return null;
        }
        float i = 0;
        while (i < topPosZ)
        {
            ClimbIn();
            i++;
            yield return null;
        }
        atTop = false;
        isTurning = true;
        float j = 0;
        while (j < 5)
        {
            TurnAfterAttack(j);
            j += turnSpeed;
            yield return null;
        }
        isTurning = false;
        
        while (!playerReady)
        {
            yield return null;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        isUnfolded = true;
        physCol.SetActive(true);
        attackDistance = Random.Range(.8f, 1f);
        yield return new WaitForSeconds(Random.Range(0f, .4f));
        while (isUnfolded)
        {
            while (Mathf.Abs(playerDistance) > attackDistance)
            {
                FollowPlayer();
                Debug.Log("Following...");
                yield return null;
            }
            Debug.Log("End Following");
            EndFollow();
            Debug.Log("Attack");
            Attack();
            while (isAttacking)
            {
                Debug.Log("Attacking...");
                yield return null;
            }
            yield return new WaitForSeconds(postBaseAttackWait);
            isTurning = true;
            float k = 0;
            while (k < 5)
            {
                Debug.Log("Turning...");
                TurnAfterAttack(k);
                k += turnSpeed;

                yield return null;
            }
            isTurning = false;
            Debug.Log("Restart");
            yield return null;
        }
        yield return null;
    }

    void Climb()
    {
        transform.localPosition += transform.up * climbSpeed * Time.deltaTime;
    }

    void ClimbUp()
    {
        transform.localPosition += transform.up * atTopSpeed * Time.deltaTime;
    }
    void ClimbIn()
    {
        transform.localPosition += transform.forward * atTopInSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isClimbing)
        {
            if (other.gameObject.layer == 7)
            {
                isClimbing = false;
                Debug.Log("ClimbingFalse");
                atTop = true;
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("attack");
        playerPos = player.transform.position;
    }
    public void KnifeLunge()
    {
        Vector3 direction = (playerPos - transform.position).normalized;
        rb.AddForce(new Vector3(direction.x, 0f, direction.z) * attackForce, ForceMode.Impulse);
    }
    //protected override void FollowPlayer()
    //{
    //    navAgent.enabled = true;
    //    if (rb != null)
    //    {
    //        rb.isKinematic = true;

    //    }
    //    isMoving = true;
    //    isTurning = false;
    //    navAgent.destination = player.transform.position;
    //    transform.LookAt(new Vector3(navAgent.destination.x, transform.position.y, navAgent.destination.z));
    //}

    //protected override void EndFollow()
    //{
    //    navAgent.enabled = false;
    //    if (rb != null)
    //    {
    //        rb.isKinematic = false;

    //    }
    //    isFollowing = false;
    //    isMoving = false;
    //    isAttacking = true;
    //}
}
