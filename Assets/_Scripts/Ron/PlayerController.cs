using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public GameObject ronAll;

    Animator anim;
    //Animator ronAnim;

    Rigidbody rb;

    ///BASIC MOVEMENT
    public bool isGrounded;
    public bool isMoving;
    public bool isRunning;
    public bool isCrouching;
    public bool isFalling;
    public bool isStopping;
    public bool isHanging;
    public bool isClimbingUp;
    public bool hasKnife;
    public bool hasFists;
    public bool hasMeleeTwoHanded;
    public bool hasDildo;
    bool stopped;
    bool movingLeft;
    bool movingRight;
    bool movingUp;
    bool movingDown;
    bool facingLeft;
    bool facingRight;
    bool facingUp;
    bool facingDown;
    bool isJumping;

    public float xMovement;
    public float zMovement;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float jumpDelay;
    public float jumpSpeed;
    public float xLedgeOffset;
    public float yLedgeOffset;
    
    float moveSpeed;
    float yRot;
    float velocityX;
    float velocityZ;
    float stopSpeed;
    float climbSpeedX;
    float climbSpeedY;

    Vector3 climbUpPos;
    Vector3 climbVelocity;
    ///WEAPONS////

    public static int currentWeapon;

    List<bool> obtainedWeaponsList;

    public bool isShooting;
    public bool weaponRaised;
    public bool weaponSheathed;
    public bool weaponAimed;
    bool aimable;
    bool spendShells;
    bool weaponEquipped;

    public float raiseWait;
    public float unsheathWait;
    float shootWait;
    float shootPower;


    public int equippedWeapon;
    int activeWeaponLayer;

    public GameObject[] activeWeapons;
    public GameObject[] passiveWeapons;

    public int randomSelection;

    public GameObject gunArmR;
    public GameObject neck;

    public float armROffsetUp;
    public float armROffsetDown;
    float minArmRot;
    float maxArmRot;
    float currentArmRot;
    public float armSpeed;

    public float neckOffsetUp;
    public float neckOffsetDown;
    float minNeckRot;
    float maxNeckRot;
    float currentNeckRot;
    public float neckSpeed;

    bool aiming;

    public GameObject muzzleFlash;
    public Transform[] muzzleFlashSpawns;

    public GameObject[] gunSmoke;

    public GameObject[] spentShells;
    public Transform[] spentShellsSpawns;

    BulletMover bm;

    //Door
    public bool doorTriggered;
    bool nearDoor;
    bool openingDoor;
    Vector3 nearDoorOffset;
    Vector3 nearDoorPosition;
    Vector3 stepBackOffset;
    Vector3 stepBackPosition;

    private void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked; *turns off mouse*
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;

        bm = new BulletMover();

        //DOOR//
        nearDoorOffset = new Vector3(.01f, 0f, 13.75f);
        stepBackOffset = new Vector3(0f, 0f, 0.805f);


    }

    private void Start()
    {
        obtainedWeaponsList = new List<bool>(PlayerInfo.weaponID.Count());
        obtainedWeaponsList.Insert(0, true);
        obtainedWeaponsList.Insert(1, true); //Temp knife
        obtainedWeaponsList.Insert(2, true); //Temp revolver
        obtainedWeaponsList.Insert(3, true); //Temp shotgun
        obtainedWeaponsList.Insert(4, true); //Temp reamer
        obtainedWeaponsList.Insert(5, true); //Temp acoustic
        obtainedWeaponsList.Insert(6, true); //Temp punisher
        obtainedWeaponsList.Insert(7, true); //Temp uzi

        SetEquippedWeapon();
    }

    private void Update()
    {
        FreezeMovement();
        stopped = true;

        ////BASIC MOVEMENT///
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isStopping", isStopping);
        anim.SetBool("isHanging", isHanging);
        anim.SetBool("isClimbingUp", isClimbingUp);
        anim.SetFloat("xMovement", xMovement);
        anim.SetFloat("zMovement", zMovement);


        //ronAnim.SetBool("isClimbingUp", isClimbingUp);

        Run();
        Jump();
        MoveButtonsUp();
        SetOrientation();
        DropFromLedge();
        ClimbUpOnLedge();

        ////WEAPONS///
        anim.SetBool("isShooting", isShooting);
        anim.SetBool("weaponRaised", weaponRaised);
        anim.SetBool("weaponAimed", weaponAimed);
        //anim.SetBool("weaponSheathed", weaponSheathed);
        anim.SetBool("hasKnife", hasKnife);
        anim.SetBool("hasFists", hasFists);
        anim.SetBool("hasMeleeTwoHanded", hasMeleeTwoHanded);
        anim.SetBool("hasDildo", hasDildo);
        anim.SetInteger("RandomSelection", randomSelection);

        Shoot();
        LowerWeapon();
        QuickSelectWeapon();
        //Debug.Log("ShootWait: " + shootWait);
        //Debug.Log("WeaponAlwaysRaised: " + WeaponAlwaysRaised());
        currentWeapon = equippedWeapon;

        ////DOOR////
        anim.SetBool("nearDoor", nearDoor);
        OpenDoor();
        OpeningDoor();
        doorTriggered = DoorOpener.doorTriggered;
        //Debug.Log("Dif: " + (DoorOpener.currentDoor.transform.position - transform.position));
    }

    private void FixedUpdate()
    {
        Move();
        JumpFall();
        //Debug.Log("IsGrounded: " + isGrounded);
    }

    private void LateUpdate()
    {
        RotateGunArm();

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            //Debug.Log("Hit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            //Debug.Log("Left");
        }
    }

    /////BASIC MOVEMENT
    bool FreezeMovement()
    {
        if (
            openingDoor
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Move()
    {
        if (!isStopping && !isClimbingUp && !isHanging && !FreezeMovement())
        {
            if (isGrounded)
            {
                MoveButtons(moveSpeed);
            }
            else
            {
                MoveButtons(jumpSpeed);
            }
            
        }
        else
        {
            //Debug.Log("Stopping");
            //Stop();
        }
    }

    void MoveButtons(float speed)
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (movingRight)
            {
                StartCoroutine(TurnWait(movingRight));
            }
            else if (isStopping)
            {
                StartCoroutine(StopWait());
            }
            else if (movingUp)
            {
                MoveSettings(-1f, 1f, "Left");
                movingLeft = true;
                movingRight = false;
            }
            else if (movingDown)
            {
                MoveSettings(-1f, -1f, "Left");
                movingLeft = true;
                movingRight = false;
            }
            else
            {

                MoveSettings(-1f, 0f, "Left");
                movingLeft = true;
                movingRight = false;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (movingLeft)
            {
                Debug.Log("Left");
                StartCoroutine(TurnWait(movingLeft));
            }
            else if (isStopping)
            {
                StartCoroutine(StopWait());
            }
            else if (movingUp)
            {
                MoveSettings(1f, 1f, "Right");
                movingLeft = true;
                movingRight = false;
            }
            else if (movingDown)
            {
                MoveSettings(1f, -1f, "Right");
                movingLeft = true;
                movingRight = false;
            }
            else
            {
                MoveSettings(1f, 0f, "Right");
                movingLeft = false;
                movingRight = true;
            }
        }
        else if (Input.GetKey(KeyCode.W))
            if (movingLeft)
            {
                MoveSettings(-1f, 1f, "Left");
                movingUp = true;
                movingDown = false;
            }
            else if (movingRight)
            {
                MoveSettings(1f, 1f, "Right");
                movingUp = true;
                movingDown = false;
            }
            else if (isStopping)
            {
                StartCoroutine(StopWait());
            }
            else
            {
                MoveSettings(0f, 1f, "Up");
            }
        else if (Input.GetKey(KeyCode.S))
            if (movingLeft)
            {
                MoveSettings(-1f, 1f, "Left");
                movingUp = false;
                movingDown = true;
            }
            else if (movingRight)
            {
                MoveSettings(1f, 1f, "Right");
                movingUp = false;
                movingDown = true;
            }
            else if (isStopping)
            {
                StartCoroutine(StopWait());
            }
            else
            {
                MoveSettings(0f, -1f, "Down");
            }
        else
        {
            //Debug.Log("Stopping");
            movingRight = false;
            movingLeft = false;
            movingUp = false;
            movingDown = false;
            isMoving = false;
            velocityX = 0f;
            velocityZ = 0f;
            if (!stopped)
            {
                Stop(.25f);
            }
        }

        velocityX = xMovement * speed;
        velocityZ = zMovement * speed * 3;
        rb.velocity = new Vector3(velocityX, rb.velocity.y, velocityZ);
    }

    void MoveSettings(float xMovementNum, float zMovementNum, string facing)
    {
        //Debug.Log("Changed");
        stopped = false;
        isMoving = true;
        xMovement = xMovementNum;
        zMovement = zMovementNum;
        if (facing == "Left")
        {
            facingLeft = true;
            facingRight = false;
            facingUp = false;
            facingDown = false;
        }
        else if (facing == "Right")
        {
            facingLeft = false;
            facingRight = true;
            facingUp = false;
            facingDown = false;
        }
        else if(facing == "Up")
        {
            facingLeft = false;
            facingRight = false;
            facingUp = true;
            facingDown = false;
        }
        else
        {
            facingLeft = false;
            facingRight = false;
            facingUp = false;
            facingDown = true;
        }
    }

    IEnumerator TurnWait(bool movingDir)
    {
        while (movingDir)
        {
            yield return null;
        }
        movingDir = false;
    }

    IEnumerator StopWait()
    {
        
        while (isStopping)
        {
            yield return null;
        }
        yield break;
    }

    IEnumerator ShootWaitTurn()
    {
        StartCoroutine(StopWait());
        while (isShooting)
        {
            yield return null;
        }
        yield break;
    }

    void MoveButtonsUp()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            movingLeft = false;
            if(!movingRight || !movingUp || !movingDown)
            {
                //Debug.Log("Stopped A");
                Stop(.1f);
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            movingRight = false;
            if (!movingLeft || !movingUp || !movingDown)
            {
                //Debug.Log("Stopped D");
                Stop(.1f);
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            movingUp = false;
            if (!movingLeft || !movingRight || !movingDown)
            {
                //Debug.Log("Stopped W");
                Stop(.1f);
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            movingDown = false;
            if (!movingLeft || !movingRight || movingUp)
            {
                //Debug.Log("Stopped S");
                Stop(.1f);
            }
        }
    }
    public void Stop(float stopWait)
    {
        //Debug.Log("Stopping");
        StartCoroutine(StopRoutine(stopWait));
    }

    IEnumerator StopRoutine(float stopWait)
    {
        isStopping = true;
        yield return new WaitForSeconds(stopWait);
        xMovement = 0f;
        zMovement = 0f;
        rb.velocity = Vector3.zero;
        isStopping = false;
        stopped = true;
        yield break;
        //Debug.Log("Stopped");
    }

    void SetOrientation()
    {
        if (facingRight)
        {
            yRot = 0f;
        }
        else if(facingLeft)
        {
            yRot = 180f;
        }
        else if (facingUp)
        {
            yRot = -90f;
        }
        else
        {
            yRot = 90f;
        }
        transform.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
    void Run()
    {
        if(isMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if(!isRunning)
                {
                    isRunning = true;
                    moveSpeed = runSpeed;
                }
                else
                {
                    isRunning = false;
                    moveSpeed = walkSpeed;
                }
            }
        }
    }

    void Jump()
    {
        if (isGrounded && !isStopping)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                anim.SetTrigger("Jump");
                isJumping = true;
                isFalling = false;
                StartCoroutine(JumpDelay());
            }
        }
    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(jumpDelay);
        rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        jumpSpeed = moveSpeed;
        while (!isGrounded)
        {
            jumpSpeed -= .01f;
            yield return new WaitForSeconds(.1f);
        }
        
    }

    void JumpFall()
    {
        if (!isGrounded)
        {
            if(rb.velocity.y < 0f)
            {
                isFalling = true;
            }
            else
            {
                isFalling = false;
            }
            isJumping = false;
        }
        else
        {
            isFalling = false;
        }
    }



    public void GrabLedge()
    {
        isHanging = true;
        isGrounded = false;
        isMoving = false;
        anim.SetLayerWeight(activeWeaponLayer, 0);
        rb.useGravity = false;
        int i = 1;
        if (!LedgeCollider.currentFacingRight)
        {
            i = -1;
            facingLeft = true;
        }
        else
        {
            i = 1;
            facingLeft = false;
        }
        Vector3 hangPos = new Vector3(LedgeCollider.currentLedgeCornerPos.x - xLedgeOffset * i, LedgeCollider.currentLedgeCornerPos.y - yLedgeOffset, 0f);
        float xDif = transform.position.x - hangPos.x;
        float yDif = transform.position.y - hangPos.y;
        transform.position = hangPos;
        rb.velocity = Vector3.zero;
        Debug.Log("Hung");
    }

    //IEnumerator MoveTowardsLedge()
    //{
        
    //    //while (Mathf.Abs(xDif) > .01f && Mathf.Abs(yDif) > .01f)
    //    //{
    //    //    rb.AddForce(xDif, yDif, 0f);
    //    //    rb.MovePosition(hangPos);
    //    //    xDif = transform.position.x - hangPos.x;
    //    //    yDif = transform.position.y - hangPos.y;
    //    //    Debug.Log("xDif:  " + xDif);
    //    //    Debug.Log("yDif:  " + yDif);
    //    //    Debug.Log("hangPos:  " + hangPos);

    //    //    yield return null;
    //    //}
       
    //    yield break;
    //}

    void DropFromLedge()
    {
        if(isHanging)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                anim.SetLayerWeight(activeWeaponLayer, 1);
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
                isHanging = false;
            }   
        }
    }

    void ClimbUpOnLedge()
    {
        if (isHanging)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                isClimbingUp = true;
                isHanging = false;
                StartCoroutine(ClimbWait());
                StartCoroutine(SetClimbVelocity());
            }
        }
    }

    IEnumerator SetClimbVelocity()
    {
        int i;
        if (facingLeft)
        {
            i = -1;
        }
        else
        {
            i = 1;
        }
        climbSpeedX = 5f;
        climbSpeedY = 22f;
        climbVelocity  = new Vector3(climbSpeedX * i, climbSpeedY, 0);

        yield return new WaitForSeconds(.5f);
        climbSpeedX = 35f;
        climbSpeedY = 0f;
        climbVelocity = new Vector3(climbSpeedX * i, climbSpeedY, 0);
        while (isClimbingUp)
        {
            yield return null;
        }
        yield break;
    }
    IEnumerator ClimbWait()
    {
        while (isClimbingUp)
        {

            rb.velocity = climbVelocity;
            yield return null;
        }
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        anim.SetLayerWeight(activeWeaponLayer, 1);
        isHanging = false;
        yield break;
    }
    public void IsClimbingFalse()
    {
        isClimbingUp = false;
    }

    /////WEAPONS/////
    
    void AddNewWeapon(int id, bool weapon)
    {
        obtainedWeaponsList.Insert(id, weapon);
    }

    void LoadObtainedWeapons()
    {
        
    }
    
    void SelectWeaponFromMenu()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (!isShooting)
            {
                float index = 0;    
            }
        }
    }

    void QuickSelectWeapon()
    {
        if (!isShooting && !isHanging)
        {
            if (obtainedWeaponsList.Count > 1)
            {
                //Debug.Log(Input.GetAxisRaw("Mouse ScrollWheel"));
                if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
                {
                    for(int i = equippedWeapon; i < obtainedWeaponsList.Count; i++)
                    {
                        if(i != equippedWeapon)
                        {
                            if (obtainedWeaponsList[i] == true)
                            {
                                activeWeapons[equippedWeapon].SetActive(false);
                                equippedWeapon = i;
                                SetWeaponVariables(i);
                                SetAnimLayer(i);
                                weaponEquipped = true;
                                activeWeapons[i].SetActive(true);
                                weaponRaised = false;
                                break;
                            }
                        }
                        
                        if (i == obtainedWeaponsList.Count - 1)
                        {
                            i = -1;
                        }
                    }      
                }
                if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
                {
                    for (int i = equippedWeapon; i < obtainedWeaponsList.Count; i--)
                    {
                        if (i != equippedWeapon)
                        {
                            if (obtainedWeaponsList[i] == true)
                            {        
                                activeWeapons[equippedWeapon].SetActive(false);
                                equippedWeapon = i;
                                SetWeaponVariables(i);
                                SetAnimLayer(i);
                                weaponEquipped = true;
                                activeWeapons[i].SetActive(true);
                                weaponRaised = false;
                                break;
                            }
                            
                        }
                        if (i == 0)
                        {
                            i = obtainedWeaponsList.Count;
                        }

                    }
                        
                }
            }
        }
    }

    void SetEquippedWeapon()
    {
        for(int i = 0; i < activeWeapons.Length; i++)
        {
            if (activeWeapons[i].activeInHierarchy)
            {
                equippedWeapon = i;
                //Debug.Log(i);
                SetWeaponVariables(i);
                SetAnimLayer(i);
                weaponEquipped = true;

                break;
            }

        }
    }

    void SetWeaponVariables(int equippedWeapon)
    {
        shootPower = PlayerInfo.weaponPower[equippedWeapon];
        raiseWait = PlayerInfo.raiseWait[equippedWeapon];
        shootWait = PlayerInfo.shootWait[equippedWeapon];
        activeWeaponLayer = PlayerInfo.weaponLayer[equippedWeapon];
        hasFists = PlayerInfo.hasFists[equippedWeapon];
        hasKnife = PlayerInfo.hasKnife[equippedWeapon];
        hasMeleeTwoHanded = PlayerInfo.hasMeleeTwoHanded[equippedWeapon];
        hasDildo = PlayerInfo.hasDildo[equippedWeapon];
        aimable = PlayerInfo.aimable[equippedWeapon];
        spendShells = PlayerInfo.spendShells[equippedWeapon];
    }

    void SetAnimLayer(int i)
    {
        for(i = 0; i < activeWeapons.Length + 1; i++)
        {
            anim.SetLayerWeight(i, 0);
            //Debug.Log(i + ": " + anim.GetLayerWeight(i));
        }
        anim.SetLayerWeight(activeWeaponLayer, 1);
    }

    void Shoot()
    {
        if (weaponEquipped)
        {
            if (Input.GetMouseButton(0))
            {
                if (!isShooting)
                {
                    if (!weaponRaised && !WeaponAlwaysRaised())
                    {
                        weaponRaised = true;
                        StartCoroutine(RaiseWeaponWait());
                    }
                    //else if (weaponSheathed)
                    //{
                    //    weaponSheathed = false;
                    //    StartCoroutine(UnSheathWeaponWait());
                    //}
                    else
                    {
                        isShooting = true;
                        anim.SetTrigger("Shoot");
                        StartCoroutine(ShootWait());
                    }
                }
            }
        } 
    }

    IEnumerator RaiseWeaponWait()
    {
        yield return new WaitForSeconds(raiseWait);
        yield break;
    }
    IEnumerator UnSheathWeaponWait()
    {
        yield return new WaitForSeconds(unsheathWait);
        yield break;
    }
    IEnumerator ShootWait()
    {
        //shoot
        yield return new WaitForSeconds(shootWait);
        isShooting = false;
        yield break;
    }

    public void WeaponRaised()
    {
        if (weaponRaised)
        {
            weaponRaised = false;
        }
        else
        {
            weaponRaised = true;
        }
    }

    bool WeaponAlwaysRaised()
    {
        if (hasDildo)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void LowerWeapon()
    {
        if (weaponEquipped)
        {
            if (isGrounded && !isShooting)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (weaponRaised)
                    {
                        weaponRaised = false;
                    }
                    else
                    {
                        weaponSheathed = true;
                    }
                    
                }
            }
        } 
    }

    public void GetClipLength()
    {
        //raiseWait = anim.GetCurrentAnimatorClipInfo(activeWeaponLayer)[0].clip.length;
        //string name = anim.GetCurrentAnimatorClipInfo(activeWeaponLayer)[0].clip.name;

        //Debug.Log("ShootWait: " + raiseWait);
        //Debug.Log("RaiseWait: " + name);
        //shootWait = raiseWait;
    }

    public void RandomizeInt()
    {
        int i = randomSelection;
        if (isMoving)
        {
            if (i == 0)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }
        }
        else
        {
            if (i == 0)
            {
                i = 1;
            }
            else if (i == 1)
            {
                i = 2;
            }
            else
            {
                i = 0;
            }
        }
        randomSelection = i;
        //Debug.Log(i);
    }

    public void SetShootWait(float wait)
    {
        shootWait = wait;
        //punch right .343
        //kick .543
        //punch left .372
    }

    public void SheathWeapon(int activeWeapon)
    {
        if (activeWeapons[activeWeapon].activeInHierarchy)
        {
            activeWeapons[activeWeapon].SetActive(false);
            passiveWeapons[activeWeapon].SetActive(true);
        }
        else
        {
            activeWeapons[activeWeapon].SetActive(true);
            passiveWeapons[activeWeapon].SetActive(false);
        }
    }

    public void ResetStance()
    {
        
    }

    void RotateGunArm()
    {
        if(weaponRaised && aimable)
        {
            
            if (Input.GetKey(KeyCode.F))
            {
                if (!aiming)
                {
                    currentArmRot = Input.mousePosition.y;
                    minArmRot = gunArmR.transform.localRotation.x - armROffsetDown;
                    maxArmRot = gunArmR.transform.localRotation.x + armROffsetUp;

                    currentNeckRot = Input.mousePosition.y;
                    minNeckRot = neck.transform.localRotation.x - neckOffsetDown;
                    maxNeckRot = neck.transform.localRotation.x + neckOffsetUp;

                    aiming = true;
                    //Maybe find a solution for setting mouse position to center before aiming
                }
                else
                {
                    aiming = true;
                    weaponAimed = true;
                    float armRotSpeed = Time.fixedDeltaTime * armSpeed;
                    float neckRotSpeed = Time.fixedDeltaTime * neckSpeed;
                    gunArmR.transform.Rotate(Mathf.Clamp((Input.mousePosition.y - currentArmRot) * armRotSpeed , minArmRot, maxArmRot) , 0, 0);
                    neck.transform.Rotate(Mathf.Clamp((Input.mousePosition.y - currentNeckRot) * neckRotSpeed, minNeckRot, maxNeckRot), 0, 0);
                    
                }
            }
            else
            {
                weaponAimed = false;
                aiming = false;
            }
        }
    }

    public void SpawnMuzzleFlash(int spawnIndex)
    {
        muzzleFlash.transform.position = muzzleFlashSpawns[spawnIndex].position;
        muzzleFlash.transform.rotation = muzzleFlashSpawns[spawnIndex].rotation;
        muzzleFlash.SetActive(true);
        bm.PoolObjects(gunSmoke, muzzleFlash.transform.position, muzzleFlash.transform.rotation);
        if (spendShells)
        {
            bm.PoolObjects(spentShells, spentShellsSpawns[spawnIndex].position, muzzleFlash.transform.rotation);
        }
    }

    //DOOR

    void OpenDoor()
    {
        if (doorTriggered)
        {
            //Press E to open door
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (!openingDoor)
                {
                    openingDoor = true;
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("openDoor");
                    //change from set active to sheathed in future
                    activeWeapons[equippedWeapon].SetActive(false);
                    anim.SetLayerWeight(activeWeaponLayer, 0);
                    StartCoroutine(MovingTowardsTheDoor());
                } 
            }
        }
    }

    IEnumerator MovingTowardsTheDoor()
    {
        Vector3 newPos;
        float speed = 10f;
        nearDoorPosition = new Vector3(DoorOpener.currentDoor.transform.position.x + nearDoorOffset.x, transform.position.y, DoorOpener.currentDoor.transform.position.x + nearDoorOffset.z);
        //rb.isKinematic = true;
        while (transform.position != nearDoorPosition)
        {
            newPos = Vector3.MoveTowards(transform.position, nearDoorPosition, speed * Time.deltaTime);
            rb.MovePosition(newPos);
            yield return null;
        }
        nearDoor = true;
        yield break;
    }

    public void StepBackFromDoor()
    {
        StartCoroutine(StepBack());
    }

    IEnumerator StepBack()
    {
        Animator dAnim = DoorOpener.currentDoor.GetComponent<Animator>();
        dAnim.SetTrigger("openDoor");
        float speed = 2f;
        Vector3 newPos;
        stepBackPosition = transform.position - stepBackOffset;
        while(transform.position != stepBackPosition)
        {
            newPos = Vector3.MoveTowards(transform.position, stepBackPosition, speed * Time.deltaTime);
            rb.MovePosition(newPos);
            yield return null;
        }
        yield break;
    }

    public void ResetWeapon()
    {
        activeWeapons[equippedWeapon].SetActive(true);
        anim.SetLayerWeight(activeWeaponLayer, 1);
        openingDoor = false;
    }

    void OpeningDoor()
    {
        if (openingDoor)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }
}
