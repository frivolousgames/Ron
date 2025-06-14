using System.Collections;
using UnityEngine;

public class BookController : EnemyController
{
    bool isSpawned;
    Vector3 forwardPos;
    [SerializeField]
    float forwardSpeed;

    bool attackMode;
    Vector3 spawnPosExact;
    [SerializeField]
    float spawnSpeedMin;
    [SerializeField]
    float spawnSpeedMax;

    [SerializeField]
    float flyAroundTime;
    [SerializeField]
    float ySpeed;
    [SerializeField]
    float xSpeed;

    [SerializeField]
    float swoopSpawnDelay;
    [SerializeField]
    float swoopSpawnPlayerDir;
    [SerializeField]
    float swoopSpawnPlayerOffset;
    [SerializeField]
    float swoopSpawnSpeed;
    [SerializeField]
    Transform swoopSpawnPos;
    Vector3 playerPlane;
    Quaternion playerRot;

    [SerializeField]
    float attackSpeed;
    [SerializeField]
    float endPosOffset;
    Vector3 endAttackPos;
    Quaternion endAttackRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        hitCol = GetComponent<BoxCollider>();
        startSpawnPos = transform.position;
        turnRotation = Quaternion.Euler(0f, -180f, 0f);
        startSpawnRot = transform.rotation;
    }

    private void OnEnable()
    {
        spawnPosExact = spawnPos.position;
        forwardPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
        isSpawned = false;
        StartCoroutine(FlyRoutine());
    }
    private void OnDisable()
    {
        transform.position = startSpawnPos;
        transform.rotation = startSpawnRot;
        StopAllCoroutines();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        anim.SetBool("isSpawned", isSpawned);

    }
    IEnumerator FlyRoutine()
    {
        float k = 0;
        while (Mathf.Abs(transform.position.z - forwardPos.z) > .01f)
        {
            rb.MovePosition(Vector3.Slerp(transform.position, forwardPos, k));
            k += forwardSpeed * Time.deltaTime;
            yield return null;
        }
        attackMode = true;
        while (attackMode)
        {
            float i = 0;
            while (Mathf.Abs(transform.position.y - spawnPosExact.y) > .01f)
            {
                rb.MovePosition(Vector3.Slerp(transform.position, spawnPosExact, i));
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, turnRotation, i));
                i += spawnSpeedMax * Time.deltaTime;
                yield return null;
            }
            transform.rotation = startSpawnRot;
            isSpawned = true;
            float j = 0;
            while (j < 20f + swoopSpawnDelay)
            {
                FlyAround();
                j += flyAroundTime;
                yield return null;
            }
            float playerX = player.transform.position.x + swoopSpawnPlayerOffset * swoopSpawnPlayerDir;
            playerPlane = new Vector3(playerX, swoopSpawnPos.position.y, player.transform.position.z);
           
            rb.velocity = Vector3.zero;
            float l = 0f;
            while (Mathf.Abs(transform.position.x - playerX) > .01f)
            {
                AttackSwoopSpawn(l);
                l += swoopSpawnSpeed * Time.deltaTime;
                yield return null;
            }
            endAttackPos = new Vector3(transform.position.x + endPosOffset, transform.position.y, transform.position.z);
            endAttackRot = Quaternion.Euler(transform.eulerAngles.x - 80f, transform.eulerAngles.y, transform.eulerAngles.z);
            anim.SetTrigger("attack");
            float m = 0;
            while (Mathf.Abs(transform.position.x - endAttackPos.x) > .001f)
            {
                SwoopAttack(m);
                m += attackSpeed * Time.deltaTime;
                yield return null;
            }
            float playerSpawnOffset = player.transform.position.x - spawnPosExact.x;
            spawnPosExact += new Vector3(playerSpawnOffset, 0f, 0f);
            yield return null;
        }
        if(isDead)
        {
            Die();
        }
        else
        {
            float n = 0;
            while (transform.position != startSpawnPos)
            {
                rb.MovePosition(Vector3.Slerp(transform.position, startSpawnPos, n));
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, startSpawnRot, n));
                n += spawnSpeedMax * Time.deltaTime;
                yield return null;
            }
            isSpawned = false;
            gameObject.SetActive(false);
        }
        yield break;
    }

    void FlyAround()
    {
        float newX = Mathf.SmoothStep(-4, 4, Mathf.PingPong(Time.time * xSpeed, 1));
        float newY = Mathf.SmoothStep(-1.5f, 1.5f, Mathf.PingPong(Time.time * 2 * ySpeed, 1));
        rb.velocity = new Vector3(newX, newY, 0f);
    }

    void AttackSwoopSpawn(float l)
    {
        playerRot = Quaternion.LookRotation((player.transform.position - transform.position).normalized);

        rb.MovePosition(Vector3.Slerp(transform.position, playerPlane, l));
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, playerRot, l));
    }

    void SwoopAttack(float m)
    {
        rb.MovePosition(Vector3.Lerp(transform.position, Vector3.Slerp(player.transform.position, endAttackPos, m * 3), m));
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, endAttackRot, m));
    }

    public override void Die()
    {
        base.Die();
        isSpawned = false;
        transform.position = startSpawnPos;
        transform.rotation = startSpawnRot;
        gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        attackMode = false;
    }
}
