using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class MicrowaveManController : UnfoldingEnemies
{
    [SerializeField]
    protected Light waveLight;
    [SerializeField]
    protected GameObject wave;
    protected VisualEffect waveEffect;
    [SerializeField]
    protected float waveLightRangeMax;
    [SerializeField]
    protected float waveLightRangeMin;
    [SerializeField]
    protected float waveLightDelay;
    [SerializeField]
    protected float glowSpeed;
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
        waveEffect = wave.GetComponent<VisualEffect>();
    }
    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isUnfolded", isUnfolded);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isTurning", isTurning);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        Unfold();
    }

    private void OnEnable()
    {
        waveLight.range = waveLightRangeMin;
        navAgent.enabled = false;
    }

    protected override IEnumerator PlayerFollowRoutine()
    {
        float h = 0;
        while (h < 200)
        {
            TurnAfterAttack(h);
            h += turnSpeed;

            yield return null;
        }
        while (isUnfolded)
        {
            isFollowing = true;
            navAgent.enabled = true;
            rb.isKinematic = true;
            while (isFollowing)
            {
                FollowPlayer();
                if (playerDistance < baseAttackDistance)
                {
                    EndFollow();
                    yield return null;
                }
                yield return null;
            }
            WaveAttack();
            while (isAttacking)
            {
                yield return null;
            }
            yield return new WaitForSeconds(postBaseAttackWait);
            float i = 0;

            while (i < 200)
            {
                TurnAfterAttack(i);
                i += turnSpeed;

                yield return null;
            }
            isFollowing = true;
            yield return null;
        }
    }
    void WaveAttack()
    {
        anim.SetTrigger("waveAttack");
        playerPos = player.transform.position;
        StartCoroutine(WaveLightGlow());
    }

    IEnumerator WaveLightGlow()
    {
       
        while(waveLight.range < waveLightRangeMax)
        {
            waveLight.range += glowSpeed * Time.deltaTime;
            yield return null;
        }
        waveEffect.Play();
        yield return new WaitForSeconds(waveLightDelay);
        waveEffect.Stop();
        while (waveLight.range > waveLightRangeMin)
        {
            waveLight.range -= glowSpeed * Time.deltaTime * 2;
            yield return null;
        }
        waveLight.range = waveLightRangeMin;
        
        yield break;
    }
    protected override void TurnAfterAttack(float i)
    {
        isTurning = true;
        Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
    }
}
