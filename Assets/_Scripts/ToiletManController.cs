using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToiletManController : UnfoldingEnemies
{
    [SerializeField]
    bool isJumping;
    [SerializeField]
    bool isLanded;
    [SerializeField]
    bool isThrowing;
    [SerializeField]
    bool isSpraying;
    [SerializeField]
    bool isTongueOut;
    [SerializeField]
    bool isTonguing;
    [SerializeField]
    bool isYelling;
    [SerializeField]
    bool isStunned;
    bool turnAfterJump;

    ///JUMP///
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    float jumpSpeed;
    [SerializeField]
    float jumpPower;
    [SerializeField]
    float jumpEndDelay;
    [SerializeField]
    float stompDistance;
    [SerializeField]
    float stompForce;
    bool isStomping;
    Collider playerPhysCol;
    [SerializeField]
    Collider physCol;


    ///SPRAY///
    [SerializeField]
    float spinSpeed;
    bool isSpinning;
    [SerializeField]
    float sprayEndDelay;
    [SerializeField]
    int sprayTime;
    [SerializeField]
    ParticleSystem ps1;
    [SerializeField]
    ParticleSystem ps2;
    ParticleSystem.MainModule ps1Main;
    ParticleSystem.MainModule ps2Main;
    [SerializeField]
    float psStartSpeedMulti;
    [SerializeField]
    float psGravityMulti;
    [SerializeField]
    float psEmMulti;
    ParticleSystem.MinMaxCurve gCurve;
    ParticleSystem.EmissionModule emissionModule;
    float OStartSpeedMulti;
    float OEmissionMulti;
    [SerializeField]
    BoxCollider sprayCol;
    Vector3 sprayColStartPos;
    Vector3 sprayColEndPos;
    Vector3 sprayColStartSize;
    Vector3 sprayColEndSize;
    [SerializeField]
    float sprayColSpeed;
    Color psOColor;

    ///POOP THROW///
    ObjectPooler pooler;
    [SerializeField]
    GameObject poopL;
    [SerializeField]
    GameObject poopR;
    Vector3 poopPos;
    Quaternion poopRot;
    int currentPoop;
    [SerializeField]
    float poopThrowEndDelay;
    float throwDistanceMax = 9.3f;
    float throwDistanceMin = 6.6f;
    float throwDistanceCenter;
    Vector3 newThrowPos;
    Vector3 roomCenter; 
    bool centerPosReached;

    //TONGUE ATTACK///
    [SerializeField]
    float tongueRangeMin;
    [SerializeField]
    float tongueRangeMax;
    float tongueRangeCenter;
    bool midTonguing;
    bool chaseMode;
    bool isGrabbed;
    [SerializeField]
    float grabDelay;
    [SerializeField]
    Transform grabRon;
    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    float grabRotMin;
    [SerializeField]
    float grabRotMax;
    [SerializeField]
    Quaternion grabRotCenter;
    [SerializeField]
    float grabRotSpeed;
    [SerializeField]
    Transform grabCol;

    int attackMode;
    int resetNum;
    bool inFront;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;
        pooler = new ObjectPooler();
        roomCenter = new Vector3(-20.56f, transform.position.y, -7.90f);
        throwDistanceCenter = (throwDistanceMax + throwDistanceMin) / 2;
        tongueRangeCenter = (tongueRangeMax + tongueRangeMin) / 2;
        grabRotCenter = Quaternion.Euler(Vector3.zero);
        ps1Main = ps1.main;
        ps2Main = ps2.main;
        emissionModule = ps1.emission;
        OStartSpeedMulti = ps1Main.startSpeedMultiplier;
        OEmissionMulti = emissionModule.rateOverTimeMultiplier;
        gCurve = new ParticleSystem.MinMaxCurve();
    }

    private void Start()
    {
        navAgent.enabled = false;
        attackMode = 0;
        sprayColStartPos = new Vector3(0f, -0.0439f, 0f);
        sprayColEndPos = new Vector3(0f, -0.177100003f, 0f);
        sprayColStartSize = new Vector3(0.15976198f, 0.139682814f, 0.417422593f);
        sprayColEndSize = new Vector3(0.15976198f, 0.437296659f, 0.417422593f);
        sprayCol.gameObject.transform.localPosition = sprayColStartPos;
        sprayCol.size = sprayColStartSize;
        sprayCol.gameObject.SetActive(false);
        playerPhysCol = GameObject.FindGameObjectWithTag("PlayerPhysCol").GetComponent<Collider>();
        psOColor = ps1.GetComponent<Renderer>().material.color;
    }

    private void OnEnable()
    {
        navAgent.enabled = false;
        attackMode = 0;
        
        StartCoroutine(AttackRoutine());
    }
    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isTurning", isTurning);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isLanded", isLanded);
        anim.SetBool("isSpraying", isSpraying);
        anim.SetBool("isThrowing", isThrowing);
        anim.SetBool("isTongueOut", isTongueOut);
        anim.SetBool("isTonguing", isTonguing);
        anim.SetBool("isYelling", isYelling);
        anim.SetBool("isStunned", isStunned);

        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        //Debug.Log("Y: " + transform.localEulerAngles.y); //temp
    }

    IEnumerator AttackRoutine()
    {
        while(!isFrozen)
        {
            rb.isKinematic = true;
            navAgent.enabled = false;
            isYelling = true;
            while (isYelling)
            {
                yield return null;
            }

            rb.isKinematic = false;
            isTurning = true;
            float h = 0;
            while (h < 20)
            {
                TurnAfterAttack(h);
                h += turnSpeed;
                yield return null;
            }
            while (playerDistance > baseAttackDistance)
            {
                FollowPlayer();
                yield return null;
            }
            EndFollow();
            isAttacking = true;
            //StartCoroutine(SprayRoutine()); //temp
            SetAttackRoutine();
            yield break;
        } 
    }

    ///JUMP ATTACK///

    IEnumerator JumpRoutine()
    {
        isLanded = true;
        isJumping = true;
        resetNum = 0;
        while (isAttacking)
        {
            JumpAttack();
            IgnoreCollision(true);
            yield return null;
        }
        IgnoreCollision(false);
        yield return new WaitForSeconds(jumpEndDelay);
        yield break;
    }
    void JumpAttack()
    {
        if (isJumping && turnAfterJump)
        {
            isTurning = true;
            TurnAfterAttack(Time.deltaTime * turnTime);
        }
    }
    public void Jump()
    {
        //Vector3 direction = (playerPos - transform.position).normalized;
        //rb.AddForce(new Vector3(direction.x, jumpHeight, direction.z) * jumpPower, ForceMode.Impulse);
        turnAfterJump = false;
        rb.AddForce(new Vector3(transform.forward.x, jumpHeight, transform.forward.z) * jumpPower, ForceMode.Impulse);
    }

    public void StompOnPlayer()
    {
        if (isJumping && !isStomping)
        {
            isStomping = true;
            rb.velocity = Vector3.down * stompForce;
            //Debug.Log("Stomping");
        }
    }

    public void InTheAir()
    {
        if (isJumping)
        {
            isLanded = false;
            //Debug.Log("In Air");
        }
    }

    public void Land()
    {
        if (isJumping && !isLanded)
        {
            anim.SetTrigger("Landed");
            rb.velocity = Vector3.zero;
            isLanded = true;
            isStomping = false;
            //Debug.Log("Landed");
        }
    }
    public void TurnAfterLanding()
    {
        turnAfterJump = true;
    }

    void IgnoreCollision(bool tf)
    {
        Physics.IgnoreCollision(playerPhysCol, physCol, tf);
    }

    ///SPRAY ATTACK///

    IEnumerator SprayRoutine()
    {
        resetNum = 0;
        rb.isKinematic = true;
        navAgent.enabled = true;
        float oNavSpeed = navAgent.speed;
        float oNavRotSpeed = navAgent.angularSpeed;
        navAgent.destination = roomCenter;
        isMoving = true;
        while (Vector3.Distance(transform.position, roomCenter) > 1)
        {
            navAgent.speed = 30f;
            navAgent.angularSpeed = 240f;
            yield return null;
        }
        isMoving = false;
        navAgent.speed = oNavSpeed;
        navAgent.angularSpeed = oNavRotSpeed;
        navAgent.enabled = false;
        while(transform.localEulerAngles.y > 0f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, grabRotCenter, 400 * Time.deltaTime);
            //Debug.Log("Turning: " + transform.localEulerAngles.y);
            yield return null;
        }
        isSpraying = true;
        //gCurve.constantMin = .34f;
        //gCurve.constantMax = .54f;
        //ps1Main.gravityModifier = gCurve;
        ps1Main.startSpeedMultiplier = 12;
        //ps1.emission.rateOverTimeMultiplier = 
        //15:1 ratio between speed and gravity
        while (isAttacking)
        {
            sprayCol.gameObject.transform.localPosition = sprayColStartPos;
            sprayCol.size = sprayColStartSize;
            sprayCol.gameObject.SetActive(true);
            while (isSpinning)
            {
                SpraySpin();
                ResetAttackNumber(sprayTime);
                ParticleSpread();
                SprayColMover();
                //Debug.Log("AttackNum: " + resetNum);
                yield return null;
            }
            sprayCol.gameObject.SetActive(false);
            yield return null;
        }
        resetNum = 0;
        rb.isKinematic = false;
        yield return new WaitForSeconds(sprayEndDelay);
        ps1.GetComponent<Renderer>().material.color = Color.white;
        ps1Main.startSpeedMultiplier = OStartSpeedMulti;
        emissionModule.rateOverTimeMultiplier = OEmissionMulti;
        yield break;
    }
    void SpraySpin()
    {
        if (isSpinning)
        {
            transform.Rotate(new Vector3(0f, spinSpeed * Time.deltaTime, 0f));
        }
    }

    public void SetIsSpinning()
    {
        if (isSpinning)
        {
            isSpinning = false;
        }
        else
        {
            isSpinning = true;
        }
    }

    void ParticleSpread()
    {
        ps1Main.startSpeedMultiplier += psStartSpeedMulti;
        emissionModule.rateOverTimeMultiplier += psEmMulti;
    }

    public void StopParticles()
    {
        ps1.Stop();
        ps2.Stop();
        StartCoroutine(PSColorFade());
    }

    IEnumerator PSColorFade()
    {
        float i = .017f;
        while(psOColor.a > 0f)
        {
            psOColor = new Color(1f, 1f, 1f, psOColor.a - i);
            ps1.GetComponent<Renderer>().material.color = psOColor;
            //Debug.Log("Color: " + psOColor.a);
            yield return null;
        }
        yield break;
    }

    void SprayColMover()
    {
        sprayCol.gameObject.transform.localPosition = new Vector3(sprayCol.gameObject.transform.localPosition.x, sprayCol.gameObject.transform.localPosition.y - sprayColSpeed, sprayCol.gameObject.transform.localPosition.z);
        sprayCol.size = Vector3.Lerp(sprayCol.size, sprayColEndSize, sprayColSpeed * 10);
    }

    ///POOP THROW ATTACK///

    IEnumerator PoopThrowRoutine()
    {
        isThrowing = true;
        currentPoop = 0;
        isTurning = true;
        resetNum = 0;
        while (isAttacking)
        {
            TurnAfterAttack(Time.deltaTime * turnTime);
            SetThrowDistance();
            yield return null;
        }
        navAgent.enabled = false;
        yield return new WaitForSeconds(poopThrowEndDelay);
        yield break;
    }
    public void ThrowPoop()
    {
        if (currentPoop == 1)
        {
            currentPoop = 0;
            poopPos = poopL.transform.position;
            poopRot = poopL.transform.rotation;
        }
        else
        {
            currentPoop = 1;
            poopPos = poopR.transform.position;
            poopRot = poopR.transform.rotation;
        }
        pooler.PoolObjects(PooledObjectArrays.poopArray, poopPos, poopRot, Vector3.zero);
    }
    void SetThrowDistance()
    {
        if (isThrowing)
        {
            if(playerDistance > throwDistanceMax || playerDistance < throwDistanceMin)
            {
                rb.isKinematic = true;
                navAgent.enabled = true;
                if (!centerPosReached)
                {
                    SetNewThrowPos(throwDistanceMin, throwDistanceMax, throwDistanceCenter);
                }
                else
                {
                    navAgent.destination = roomCenter;
                    if(Vector3.Distance(transform.position, roomCenter) < 1)
                    {
                        centerPosReached = false;
                    }
                }
            }
            else
            {
                rb.isKinematic = false;
                navAgent.enabled = false;
            }
        }
    }

    void SetNewThrowPos(float min, float max, float center)
    {
        float distX = player.transform.position.x - center;
        float distZ = player.transform.position.z - center;
        
        if (playerDistance > max)
        {
            navAgent.destination = new Vector3(transform.forward.x + distX, transform.position.y, transform.forward.z + distZ);
        }
        else if (playerDistance < min) 
        {
            navAgent.destination = new Vector3(transform.forward.x - distX, transform.position.y, transform.forward.z - distZ);
        }
    }

    ///TONGUE ATTACK

    IEnumerator TongueRoutine()
    {
        isTongueOut = true;
        while (isTongueOut)
        {
            yield return null;
        }
        isTonguing = true;
        while (isAttacking)
        {
            chaseMode = true;
            while (chaseMode)
            {
                TongueChase();
                //Debug.Log("Chasing");
                yield return null;
            }
            isTurning = true;
            float h = 0;
            while (h < 10)
            {
                //Debug.Log("Turning");
                TurnAfterAttack(h);
                h += turnSpeed;
                yield return null;
            }
            TongueAttack();
            while (midTonguing)
            {
                //Debug.Log("Tonguing: " + midTonguing);
                yield return null;
            }
            //Debug.Log("Reset");
            yield return new WaitForSeconds(.2f);
        }
        yield break;
    }
    public void SetIsTongueOut()
    {
        isTongueOut = false;
    }

    void TongueAttack()
    {
        rb.isKinematic = false;
        navAgent.enabled = false;
        if (!midTonguing)
        {
            midTonguing = true;
            //Debug.Log("Tonguing True");
            anim.SetTrigger("Tongue");
        }
    }

    void TongueChase()
    {
        if (playerDistance < tongueRangeMin || playerDistance > tongueRangeMax)
        {
            rb.isKinematic = true;
            navAgent.enabled = true;
            isTurning = true;
            TurnAfterAttack(Time.deltaTime * turnTime);
            SetNewThrowPos(tongueRangeMin, tongueRangeMax, tongueRangeCenter);
        }
        else
        {
            chaseMode = false;
        }
    }

    public void TongueGrab()
    {
        if (!isGrabbed)
        {
            isGrabbed = true;
            isAttacking = false;
            isTonguing = false;
            midTonguing = false;
            chaseMode = true;
            anim.SetTrigger("TongueGrab");
            player.SetActive(false);
            //Debug.Log("Grabbed");
            transform.position = roomCenter;
            transform.localEulerAngles = Vector3.zero;
            StartCoroutine(GrabWait());

        }
    }

    IEnumerator GrabWait()
    {
        while (isGrabbed)
        {
            yield return null;
        }
        yield return new WaitForSeconds(grabDelay);
        resetNum = 0;
        attackMode = 3;
        StartCoroutine(AttackRoutine());
        yield break;
    }

    public void ResetIsGrabbed()
    {
        isGrabbed = false;
    }

    public void DunkSplash()
    {
        ps.Play();
    }

    public void ResetPlayer()
    {
        player.transform.position = grabCol.position;
        player.SetActive(true);
    }

    public void ResetMidTonguing()
    {
        midTonguing = false;
        //Debug.Log("Tounging False");
    }

    public void ResetIsYelling()
    {
        isYelling = false;
    }

    ///OTHER STUFF///
    
    void SetAttackRoutine()
    {
        switch (attackMode)
        {
            case 0:
                StartCoroutine(JumpRoutine());
                break;

            case 1:
                StartCoroutine(SprayRoutine());
                break;

            case 2:
                StartCoroutine(TongueRoutine());
                break;

            case 3:
                StartCoroutine(PoopThrowRoutine());
                break;
            default:
                StartCoroutine(JumpRoutine());
                break;
        }
    }

    void SetInFront()
    {
        if(Mathf.Abs(frontDistance.x) < Mathf.Abs(backDistance.x))
        {
            inFront = true;
        }
        else
        {
            inFront = false;
        }
    }

    public void ResetAttackNumber(int resetThresh) //This function is placed at the end of each attack animation and resetThresh determines length
    {
        resetNum++;
        if (resetNum >= resetThresh)
        {
            if (isJumping)
            {
                isJumping = false;
                attackMode = 1;
            }
            else if (isSpraying)
            {
                isSpraying = false;
                attackMode = 2;
            }
            else if (isTonguing)
            {
                isTonguing = false;
                attackMode = 3;
            }
            else if (isThrowing)
            {
                isThrowing = false;
                attackMode = 0;
            }
            StartCoroutine(AttackRoutine());
            resetNum = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            centerPosReached = true;
        }
    }
}
