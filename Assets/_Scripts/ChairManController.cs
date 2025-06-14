using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class ChairManController : UnfoldingEnemies
{
    [SerializeField]
    private float kickPower;
    [SerializeField]
    protected float riseSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        hitCol = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //mainMaterial = materialObject.GetComponent<Material>();
        navAgent.enabled = false;
    }

    private void OnEnable()
    {
        navAgent.enabled = false;
    }
    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isUnfolded", isUnfolded);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isAttacking", isAttacking);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        Unfold();
    }
    

    protected override IEnumerator PlayerFollowRoutine()
    {
        float h = 0;
        while (h < 360)
        {
            TurnAfterAttack(h);
            h += turnSpeed;

            yield return null;
        }
        while (isUnfolded)
        {
            
            isFollowing = true;
            while (isFollowing)
            {
                FollowPlayer();
                if(playerDistance < baseAttackDistance)
                {
                    EndFollow();
                    yield return null;
                }
                yield return null;
            }
            Kick();
            while(isAttacking)
            {
                yield return null;
            }
            yield return new WaitForSeconds(postBaseAttackWait);
            //rb.velocity = Vector3.zero;
            float i = 0;
            while (i < 360)
            {
                TurnAfterAttack(i);
                i += turnSpeed;

                yield return null;
            }
            isFollowing = true;
            yield return null;
        }
    }

    

    protected override void TurnAfterAttack(float i)
    {
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Time.deltaTime * riseSpeed);
        isMoving = true;
        Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
    }

    void Kick()
    {
        anim.SetTrigger("Kick");
        playerPos = player.transform.position;
    }

    public void LaunchKick()
    {
        Vector3 direction = (playerPos - transform.position).normalized;
        rb.AddForce( new Vector3(direction.x, 0f, direction.z) * kickPower, ForceMode.Impulse);
        Debug.Log("Direction: " + direction);
    }

}
