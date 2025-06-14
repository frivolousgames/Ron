using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AuntPamController : UnfoldingEnemies
{
    int attackMode;
    int resetNum;
    float attackDistance;
    [SerializeField]
    float centerAttackDistance;
    [SerializeField]
    int titPunchResetNum;
    AnimatorClipInfo[] animInfo;

    [SerializeField]
    bool isTired;
    bool isTiredEnd;
    [SerializeField]
    int tiredTime;

    [SerializeField]
    bool isStomping;

    bool isTitPunching;
    bool isTitShooting;
    bool isQueefing;
    bool isChargeGrabbing;
    [SerializeField]
    bool isCharging;
    [SerializeField]
    bool isShortTurn;
    [SerializeField]
    float shortTurnThresh;

    [SerializeField]
    bool isTurnEnd;
    [SerializeField]
    float turnEndTime;
    Vector3 roomCenter;
    bool roomCenterReached;


    ///Tit Shoot Routine
    [SerializeField]
    bool isShootTurning;
    [SerializeField]
    bool isShooting;
    bool isStarting;

    [SerializeField]
    int titShootResetNum;

    [SerializeField]
    float shootTurnSpeed;

    [SerializeField]
    Transform bulletSpawnR;
    [SerializeField]
    Transform bulletSpawnL;

    ObjectPooler pooler;

    Vector3 shootPos;
    Quaternion shootRot;
    int currentTit;
    [SerializeField]
    float playerHeightOffset;

    ///Queef Attack Routine
    bool isQueefAttacking;
    [SerializeField]
    int queefNum;
    [SerializeField]
    bool isQueefTurning;
    [SerializeField]
    Transform gooTransform;

    ///ChargeGrab Attack Routine
    Coroutine chargeGrabCoroutine;
    [SerializeField]
    float chargeAttackDistance;
    [SerializeField]
    bool isGrabbing;
    [SerializeField]
    int chargeGrabResetNum;
    [SerializeField]
    float navChargeSpeed;
    [SerializeField]
    float navChargeAngSpeed;
    [SerializeField]
    float chargeAttackWait;
    bool isGrabbed;
    [SerializeField]
    float grabDelay;
    [SerializeField]
    Transform playerGrabSpawn;

    //TittySpin
    [SerializeField]
    bool isTittySpinning;
    bool isSpinning;
    [SerializeField]
    Transform pamRig;
    [SerializeField]
    int spinAmount;
    [SerializeField]
    float spinSpeed;
    [SerializeField]
    float spinDistance;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startSpawnPos = transform.position;
        startSpawnRot = transform.rotation;
        roomCenter = new Vector3(-20.56f, transform.position.y, -7.90f);
    }

    private void Start()
    {
        pooler = new ObjectPooler();
        navAgent.enabled = false;
        //attackMode = 0;
    }

    private void OnEnable()
    {
        navAgent.enabled = false;
        attackMode = 0;
        //attackMode = 3; ///TEMP
        attackDistance = baseAttackDistance;
        StartCoroutine(AttackRoutine());
    }

    private void OnDisable()
    {
        isUnfolded = false;
        transform.position = startSpawnPos;
        transform.rotation = startSpawnRot;
    }
    private void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isTired", isTired);
        anim.SetBool("isStomping", isStomping);
        anim.SetBool("isCharging", isCharging);
        anim.SetBool("isGrabbing", isGrabbing);
        anim.SetBool("isShooting", isShooting);
        anim.SetBool("isShootTurning", isShootTurning);
        anim.SetBool("isQueefAttacking", isQueefAttacking);
        anim.SetBool("isQueefTurning", isQueefTurning);
        //anim.SetBool("isHit", isHit);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isTurning", isTurning);
        anim.SetBool("isShortTurn", isShortTurn);
        anim.SetBool("isTittySpinning", isTittySpinning);
        //anim.SetBool("isFrozen", isFrozen);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        //Debug.Log("isFrozen: " + isFrozen);
        //Unfold();
        //ReactToPlayer();
        //Debug.Log("isTurnEnd: " + isTurnEnd);
    }

    IEnumerator AttackRoutine()
    {
        while (!isFrozen)
        {
            rb.isKinematic = true;
            navAgent.enabled = false;
            isShortTurn = false;
            isStomping = true;
            while (isStomping)
            {
                //Debug.Log("Stomping (Attack Routine)");
                yield return null;
            }
            if(attackMode == 1 || attackMode == 2)
            {
                //Debug.Log("SetAttackRoutine 1 & 2");
                SetAttackRoutine();
                yield break;
            }
            rb.isKinematic = false;
            Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            if (Mathf.Abs(transform.rotation.eulerAngles.y - turnRotation.eulerAngles.y) < shortTurnThresh)
            {
                isShortTurn = true;
            }
            if (transform.rotation != turnRotation)
            {
                isTurning = true;
                float h = 0;
                while (h < 20)
                {
                    TurnAfterAttack(h);
                    //Debug.Log("Turning (Attack Routine)");
                    h += turnSpeed;
                    if (transform.rotation == turnRotation)
                    {
                        h = 20;
                        yield return null;
                    }
                    yield return null;
                }
                animInfo = anim.GetCurrentAnimatorClipInfo(0);
                while (animInfo[0].clip.name != "Aunt  Pam Rig_Turn")
                {
                    animInfo = anim.GetCurrentAnimatorClipInfo(0);
                    //Debug.Log("TP AnimInfo: " + animInfo[0].clip.name);
                    yield return null;
                }
                isTurning = false;
                if (!isShortTurn)
                {
                    isTurnEnd = true;
                    while (isTurnEnd)
                    {
                        Debug.Log("TurnEnd");
                        yield return null;
                    }
                }
                yield return null;
            }
            isShortTurn = false;
            if (attackMode == 3)
            {
                SetAttackRoutine();
                Debug.Log("attackMode = 3");
                yield break;
            }
            while (playerDistance > baseAttackDistance)
            {
                Debug.Log("Following");
                FollowPlayer();
                yield return null;
            }
            EndFollow();
            //Debug.Log("EndFollow");
            //Debug.Log("attackMode: " + attackMode);
            //chargeGrabCoroutine = StartCoroutine(ChargeGrabRoutine()); //temp
            //Debug.Log("End");
            SetAttackRoutine();
            yield break;
        }
    }

    IEnumerator TitPunchRoutine()
    {
        isTitPunching = true;
        isShortTurn = false;
        resetNum = 0;
        while (resetNum < titPunchResetNum)
        {
            while (playerDistance > baseAttackDistance)
            {
                //Debug.Log("Following");
                FollowPlayer();
                yield return null;
            }
            //Debug.Log("Start: " + resetNum);
            EndFollow();
            anim.SetTrigger("TitAttack");
            while (isAttacking)
            {
                Debug.Log("Attacking" + gameObject.name, this);
                yield return null;
            }
            Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            if (Mathf.Abs(transform.rotation.eulerAngles.y - turnRotation.eulerAngles.y) < shortTurnThresh)
            {
                isShortTurn = true;
            }
            if (transform.rotation != turnRotation)
            {
                isTurning = true;
                float h = 0;
                while (h < 20)
                {
                    TurnAfterAttack(h);
                    //Debug.Log("Turning (Tit Punch): " + (transform.rotation.eulerAngles.y - turnRotation.eulerAngles.y));
                    h += turnSpeed;
                    if (transform.rotation == turnRotation)
                    {
                        h = 20;
                        yield return null;
                    }
                    yield return null;
                }
                animInfo = anim.GetCurrentAnimatorClipInfo(0);
                while (animInfo[0].clip.name != "Aunt  Pam Rig_Turn")
                {
                    animInfo = anim.GetCurrentAnimatorClipInfo(0);
                    //Debug.Log("TP AnimInfo: " + animInfo[0].clip.name);
                    yield return null;
                }
                isTurning = false;
                if (!isShortTurn)
                {
                    isTurnEnd = true;
                    while (isTurnEnd)
                    {
                        Debug.Log("TurnEnd");
                        yield return null;
                    }
                }
                yield return null;
            }
            isShortTurn = false;
            while (playerDistance > attackDistance)
            {
                FollowPlayer();
                //Debug.Log("Following");
                yield return null;
            }
            EndFollow();
            resetNum++;
            //Debug.Log("Restart Tit Punch");
            yield return null;
        }
        //Debug.Log("End");
        isAttacking = false;
        ResetAttackNumber(titPunchResetNum);
        yield break;
    }
    
    IEnumerator TitShootRoutine()
    {
        isTitShooting = true;
        resetNum = 0;
        isShortTurn = false;
        roomCenterReached = false;
        StartCoroutine(RoomCenterRoutine());
        while (!roomCenterReached)
        {
            yield return null;
        }
        isShooting = true;
        isStarting = true;
        while (isStarting)
        {
            yield return null;
        }
        isAttacking = true;
        anim.SetLayerWeight(2, 1);
        currentTit = 0;
        while (isAttacking)
        {
            while (resetNum < titShootResetNum)
            {
                float j = 0;
                isShootTurning = true;
                while (j < 20)
                {
                    TurnAfterAttack(j);
                    //Debug.Log("Turning");
                    j += turnSpeed;
                    if (transform.rotation == turnRotation)
                    {
                        j = 20;
                        isShootTurning = false;
                    }
                    yield return null;
                }
                yield return null;
            }
            isShootTurning = false;
            isShooting = false;
            anim.SetLayerWeight(2, 0);
            yield return null;
        }
        ResetAttackNumber(titShootResetNum);
        yield break;
    }

    //Tit Shoot Routine

    

    public void SpawnBullet()
    {
        if (currentTit == 1)
        {
            currentTit = 0;

            Vector3 playerFixedPos = new Vector3(player.transform.position.x, player.transform.position.y + playerHeightOffset, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - bulletSpawnR.transform.position).normalized;
            bulletSpawnR.transform.rotation = turnRotation;

            shootPos = bulletSpawnL.position;
            shootRot = bulletSpawnL.rotation;
        }
        else
        {
            currentTit = 1;

            Vector3 playerFixedPos = new Vector3(player.transform.position.x, player.transform.position.y + playerHeightOffset, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - bulletSpawnR.transform.position).normalized;
            bulletSpawnR.transform.rotation = turnRotation;
            shootPos = bulletSpawnR.position;

            shootRot = bulletSpawnR.rotation;
        }

        
        pooler.PoolObjects(PooledObjectArrays.milkBulletArrays, shootPos, shootRot, Vector3.zero); 
        pooler.PoolObjects(PooledObjectArrays.gunsSmokesArray, shootPos, shootRot, Vector3.zero);
        pooler.PoolObjects(PooledObjectArrays.muzzleFlashesArray, shootPos, shootRot, Vector3.zero);
        pooler.PoolObjects(PooledObjectArrays.shellCasingsArray, shootPos, shootRot, Vector3.zero);
    }

    ///Queef Gas Routine
    IEnumerator QueefGasRoutine()
    {
        isQueefing = true;
        resetNum = 0;
        roomCenterReached = false;
        StartCoroutine(RoomCenterRoutine());
        while (!roomCenterReached)
        {
            //Debug.Log("Center Not Reached");
            yield return null;
        }
        isQueefAttacking = true;
        isStarting = true;
        while (isStarting)
        {
            yield return null;
        }
        isAttacking = true;
        isQueefTurning = true;
        while (isQueefAttacking)
        {
            while (!isQueefTurning && isQueefAttacking)
            {
                //Debug.Log("Queefing");
                yield return null;
            }
            float j = 0;
            while (j < 20)
            {
                TurnAfterAttack(j);
                //Debug.Log("Turning");
                j += turnSpeed / 5f;
                if (transform.rotation == turnRotation)
                {
                    j = 20;
                    isQueefTurning = false;
                }
                yield return null;
            }
            isQueefTurning = false;
            yield return null;
        }
        //Debug.Log("Attacking End");
        while (isAttacking)
        {
            //Debug.Log("isAttackingn (Queef)");
            yield return null;
        }
        //Debug.Log("Restart (Queef)");
        ResetAttackNumber(queefNum);
        yield break;
    }

    public void QueefAttackEnd()// Placed at end of queef attack to decide to continue to turning or end
    {
        resetNum++;
        if (resetNum > queefNum)
        {
            //Debug.Log("End");
            isQueefAttacking = false;
            isQueefTurning = false;
        }
        else
        {
            //Debug.Log("TurnTime");
            isQueefTurning = true;
        }
    }

    public void QueefGoo()
    {
        pooler.PoolObjects(PooledObjectArrays.gooParticleArrays, gooTransform.position, gooTransform.rotation, Vector3.zero);
    }

    ///Charge Grab Routine
    IEnumerator ChargeGrabRoutine()
    {
        isChargeGrabbing = true;
        resetNum = 0;
        while (resetNum < chargeGrabResetNum)
        {
            navAgent.enabled = true;
            rb.isKinematic = true;
            isMoving = true;
            while (playerDistance > attackDistance)
            {
                //Debug.Log("Following");
                FollowPlayer();
                yield return null;
            }
            float oNavSpeed = navAgent.speed;
            float oNavRotSpeed = navAgent.angularSpeed;
            Vector3 playerTempPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            navAgent.destination = playerTempPos;
            isCharging = true;
            navAgent.speed = navChargeSpeed;
            navAgent.angularSpeed = navChargeAngSpeed;
            while (Vector3.Distance(transform.position, playerTempPos) > 1)
            {
                transform.LookAt(playerTempPos);
                //Debug.Log("Charging");
                yield return null;
            }
            navAgent.speed = oNavSpeed;
            navAgent.angularSpeed = oNavRotSpeed;
            isMoving = false;
            isGrabbing = true;
            isCharging = false;
            while(isGrabbing)
            {
                Debug.Log("Grabbing");
                if (isGrabbed)
                {
                    isGrabbing = false;
                    resetNum = chargeGrabResetNum;
                    Debug.Log("Grabbed!");
                    yield break;
                }
                yield return null;
            }
            navAgent.enabled = false;
            rb.isKinematic = false;
            isFollowing = false;
            isTired = true;
            int i = 0;
            while (i < tiredTime)
            {
                i++;
                yield return null;
            }
            isTired = false;
            isTiredEnd = true;
            while (isTiredEnd)
            {
                //Debug.Log("Dist: " + Vector3.Distance(player.transform.position, transform.position));
                if (!isTittySpinning)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < spinDistance)
                    {
                        isTittySpinning = true;
                        isTiredEnd = false;
                    }
                }
                yield return null;
            }
            while (isTittySpinning)
            {
                yield return null;
            }
            Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            if (Mathf.Abs(transform.rotation.eulerAngles.y - turnRotation.eulerAngles.y) < shortTurnThresh)
            {
                isShortTurn = true;
                //Debug.Log("ShortTurn");
            }
            if (transform.rotation != turnRotation)
            {
                isTurning = true;
                float h = 0;
                while (h < 20)
                {
                    TurnAfterAttack(h);
                    //Debug.Log("Turning (Tit Punch): " + (transform.rotation.eulerAngles.y - turnRotation.eulerAngles.y));
                    h += turnSpeed;
                    if (transform.rotation == turnRotation)
                    {
                        h = 20;
                        yield return null;
                    }
                    yield return null;
                }
                animInfo = anim.GetCurrentAnimatorClipInfo(0);
                while (animInfo[0].clip.name != "Aunt  Pam Rig_Turn")
                {
                    animInfo = anim.GetCurrentAnimatorClipInfo(0);
                    //Debug.Log("TP AnimInfo: " + animInfo[0].clip.name);
                    yield return null;
                }
                isTurning = false;
                if (!isShortTurn)
                {
                    isTurnEnd = true;
                    while (isTurnEnd)
                    {
                        Debug.Log("TurnEnd");
                        yield return null;
                    }
                }
            }
            isShortTurn = false;
            yield return new WaitForSeconds(chargeAttackWait);
        }
        ResetAttackNumber(chargeGrabResetNum);
        yield break;
    }

    public void Grab()
    {
        if (!isGrabbed)
        {
            StopCoroutine(chargeGrabCoroutine);
            isGrabbed = true;
            isAttacking = false;
            isGrabbing = false;
            isCharging = false;
            isMoving = false;
            isTurning = false;
            isTurnEnd = false;
            isTired = false;
            isTiredEnd = false;
            isTittySpinning = false;
            navAgent.enabled = false;
            rb.isKinematic = true;
            anim.SetTrigger("Grab");
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
        attackMode = 0;
        StartCoroutine(AttackRoutine());
        yield break;
    }

    public void ResetIsGrabbing()
    {
        Debug.Log("Grabbed False");
        isGrabbing = false;
    }

    public void ResetIsGrabbed()
    {
        isGrabbed = false;
    }

    public void ResetPlayer()
    {
        player.transform.position = playerGrabSpawn.position;
        player.SetActive(true);
    }

   

    //TittySpin
    public void Spin()
    {
        isSpinning = true;
        anim.speed = 0f;
        StartCoroutine(TittySpinLoop());
    }

    IEnumerator TittySpinLoop()
    {
        Quaternion oRot = transform.rotation;
        float newRot = 359f;
        float y = 0f;
        for (int i = 0; i < spinAmount; i++)
        {
            while (y < newRot)
            {

                //transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, spinSpeed * Time.deltaTime);
                transform.Rotate(0f, -spinSpeed * Time.deltaTime, 0f);
                y+= Time.deltaTime * spinSpeed;
                //Debug.Log("Spinning: " + transform.rotation.eulerAngles.y + " Y: " + y);
                yield return null;
            }
            transform.rotation = oRot;
            y = 0;
        }
        anim.speed = 1f;
        isSpinning = false;
        yield break;
    }

    public void ResetIsTittySpin()
    {
        isTittySpinning = false;
    }
    IEnumerator RoomCenterRoutine()
    {
        yield return new WaitForSeconds(.01f);
        isShortTurn = false;
        rb.isKinematic = false;
        Vector3 roomCenterPos = new Vector3(roomCenter.x, transform.position.y, roomCenter.z);
        turnRotation = Quaternion.LookRotation(roomCenter - transform.position).normalized;
        if (Mathf.Abs(transform.rotation.eulerAngles.y - turnRotation.eulerAngles.y) < shortTurnThresh)
        {
            isShortTurn = true;
        }
        if (transform.rotation != turnRotation)
        {
            float h = 0;
            while (h < 20)
            {
                TurnTowardsRoomCenter(h);
                //Debug.Log("Turning");
                h += turnSpeed;
                if (transform.rotation == turnRotation)
                {
                    h = 20;
                }
                yield return null;
            }
            animInfo = anim.GetCurrentAnimatorClipInfo(0);
            while (animInfo[0].clip.name != "Aunt  Pam Rig_Turn")
            {
                animInfo = anim.GetCurrentAnimatorClipInfo(0);
                //Debug.Log("TP AnimInfo: " + animInfo[0].clip.name);
                yield return null;
            }
            isTurning = false;
            if (!isShortTurn)
            {
                isTurnEnd = true;
                while (isTurnEnd)
                {
                    Debug.Log("TurnEnd");
                    yield return null;
                }
            }
        }
        yield return new WaitForSeconds(.01f);
        isShortTurn = false;
        rb.isKinematic = true;
        navAgent.enabled = true;
        float oNavSpeed = navAgent.speed;
        float oNavRotSpeed = navAgent.angularSpeed;
        navAgent.destination = roomCenter;
        while (Vector3.Distance(transform.position, roomCenter) > 1)
        {
            isCharging = true;
            navAgent.speed = navChargeSpeed;
            navAgent.angularSpeed = navChargeAngSpeed;
            //Debug.Log("Charging");
            yield return null;
        }
        isCharging = false;
        navAgent.speed = oNavSpeed;
        navAgent.angularSpeed = oNavRotSpeed;
        navAgent.enabled = false;
        rb.isKinematic = false;
        Vector3 playerFixedPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
        if (Mathf.Abs(transform.rotation.eulerAngles.y - turnRotation.eulerAngles.y) < shortTurnThresh)
        {
            isShortTurn = true;
        }
        if (transform.rotation != turnRotation)
        {
            isTurning = true;
            float i = 0;
            while (i < 20)
            {
                //Debug.Log("RC AnimInfo: " + animInfo[0].clip.name);
                TurnAfterAttack(i);
                //Debug.Log("Turning 2 (Room Center)");
                i += shootTurnSpeed;
                if (transform.rotation == turnRotation)
                {
                    i = 20;
                    yield return null;
                }
                yield return null;
            }
            animInfo = anim.GetCurrentAnimatorClipInfo(0);
            while (animInfo[0].clip.name != "Aunt  Pam Rig_Turn")
            {
                animInfo = anim.GetCurrentAnimatorClipInfo(0);
                //Debug.Log("TP AnimInfo: " + animInfo[0].clip.name);
                yield return null;
            }
            isTurning = false;
            if (!isShortTurn)
            {
                isTurnEnd = true;
                while (isTurnEnd)
                {
                    Debug.Log("TurnEnd");
                    yield return null;
                }
            }
        }
        isShortTurn = false;
        roomCenterReached = true;
        yield break;
    }

    public void AddInt()
    {
        resetNum++;
    }

    public void ResetIsStarting()
    {
        isStarting = false;
    }
    void TurnTowardsRoomCenter(float i)
    {
        isTurning = true;
        Vector3 roomCenterPos = new Vector3(roomCenter.x, transform.position.y, roomCenter.z);
        turnRotation = Quaternion.LookRotation(roomCenter - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
    }
    public void ResetIsStomping()
    {
        isStomping = false;
    }

    public void ResetIsTurnEnd() 
    {
        //Debug.Log("ResetTurnEnd");
        isTurnEnd = false;
    }

    public void ResetIsTiredEnd()
    {
        isTiredEnd = false;
    }

    public void ResetAttackNumber(int resetThresh) //This function is placed at the end of each attack animation and resetThresh determines length
    {
        resetNum++;
        if (resetNum >= resetThresh)
        {
            if (isTitPunching)
            {
                isTitPunching = false;
                attackMode = 1;
                attackDistance = baseAttackDistance;
            }
            else if (isTitShooting)
            {
                isTitShooting = false;
                attackMode = 2;
                attackDistance = centerAttackDistance;
            }
            else if (isQueefing)
            {
                isQueefing = false;
                attackMode = 3;
                attackDistance = chargeAttackDistance;
            }
            else if (isChargeGrabbing)
            {
                isChargeGrabbing = false;
                attackMode = 0;
                attackDistance = baseAttackDistance;
            }
            StartCoroutine(AttackRoutine());
            resetNum = 0;
        }
    }
    void SetAttackRoutine()
    {
        switch (attackMode)
        {
            case 0:
                StartCoroutine(TitPunchRoutine());
                break;

            case 1:
                StartCoroutine(TitShootRoutine());
                break;

            case 2:
                StartCoroutine(QueefGasRoutine());
                break;

            case 3:
                chargeGrabCoroutine = StartCoroutine(ChargeGrabRoutine());
                break;
            default:
                StartCoroutine(TitPunchRoutine());
                break;
        }
    }
}
