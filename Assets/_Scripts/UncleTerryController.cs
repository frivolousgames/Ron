using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UncleTerryController : EnemyController
{

    [SerializeField]
    bool isFloating;
    [SerializeField]
    bool isSpinning;
    [SerializeField]
    bool isHat;
    [SerializeField]
    bool isHootin;

    bool isClose;
    bool isFar;
    bool floatTime;

    [SerializeField]
    float closeThreshold;

    [SerializeField]
    GameObject hat;
    [SerializeField]
    GameObject shovel;

    [SerializeField]
    float floatSpeed;
    [SerializeField]
    float floatPosY;
    Vector3 floatPos;

    [SerializeField]
    GameObject waveRing;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;
    }

    private void Start()
    {
        navAgent.enabled = false;
    }

    private void OnEnable()
    {
        navAgent.enabled = false;
        StartCoroutine(AttackRoutine());
    }

    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isSpinning", isSpinning);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isHat", isHat);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isFloating", isFloating);
        anim.SetBool("isTurning", isTurning);
        anim.SetBool("isHootin", isHootin);

        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        HatThrowTemp();
        ShovelSpinTemp();
    }

    IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            floatPos = new Vector3(transform.position.x, floatPosY, transform.position.z);
            rb.isKinematic = true;
            isFloating = true;
            while (transform.position.y < floatPosY - .1f)
            {
                Float();
                yield return null;
            }
            isFloating = false;
            yield break;
        }
        yield return null;
    }

    ///Hat Throw///
    public void HatThrow()
    {
        hat.SetActive(true);
    }

    public void HatReturned()
    {
        isHat = false;
    }

    void HatThrowTemp()
    {
        if (!isHat && !hat.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                isHat = true;
            }
        }
    }

    void ShovelSpinTemp()
    {
        if (!isSpinning)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                isSpinning = true;
            }
        }
    }

    ///Shovel Throw///

    public void ShovelThrow()
    {
        shovel.SetActive(true);
    }

    ///Float///
    void Float()
    {
        rb.MovePosition(Vector3.Lerp(transform.position, floatPos, floatSpeed * Time.deltaTime));
    }

    public void SetFloatAttack()
    {
        int rando = Random.Range(0, 2);
        if (rando == 1)
        {
            anim.SetTrigger("GroundPound");
        }
        else
        {
            anim.SetTrigger("ShovelThrow");
        }
    }

    public void WaveRingAttack()
    {
        waveRing.SetActive(true);
    }


}
