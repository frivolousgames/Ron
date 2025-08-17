using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //public GameObject ronAll;

    Animator anim;

    [SerializeField]
    Collider physCol;

    Rigidbody rb;

    [SerializeField]
    GameObject mainPPVolume;

    ///BASIC MOVEMENT
    public bool isGrounded;
    public bool isMoving;
    public bool isRunning;
    public bool isCrouching;
    public bool isFalling;
    public bool isStopping;
    public bool isHanging;
    public bool isClimbingUp;

    bool stopped;
    bool facingLeft;
    bool isJumping;
    bool isJumpDelaying;
    bool freezeMovement;

    Vector3 movement;
    Quaternion lookRotation;
    Quaternion lookRot;
    public float xMovement;
    public float zMovement;
    Vector3 rotMovement;
    float rotDifference;

    [SerializeField]
    float turnSpeed;
    [SerializeField]
    float speedMulti;
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

    [SerializeField]
    float groundRayDist;

    ///WEAPONS////
    public bool hasKnife;
    public bool hasFists;
    public bool hasMeleeTwoHanded;
    public bool hasDildo;
    public static int currentWeapon;

    List<bool> obtainedWeaponsList;

    public bool isShooting;
    bool isContinueShooting;
    bool shootDelay;
    bool isShootEnd;
    public bool weaponRaised;
    bool isRaising;
    public bool weaponSheathed;
    public bool weaponAimed;
    bool aimable;
    bool automatic;
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

    [SerializeField]
    GameObject muzzleFlash;
    public Transform[] muzzleFlashSpawns;

    GameObject[] gunSmoke;

    GameObject[] spentShells;
    public Transform[] spentShellsSpawns;

    ObjectPooler pooler;

    ///Vehicle Weapon
    public static int selectedVehicleWeapon;
    int vehicleWeaponID;
    float vehicleWeaponPower;
    float vehicleWeaponShootWait;

    //Idle
    public bool isIdle;
    int idleWait;
    bool alreadyIdle;
    bool isFlipping;

    public GameObject[] devilSticks;

    public int idleStateRandom;
    bool idleStateSelected;

    Quaternion idleRotation;

    ////INTERACTABLE/////
    List<GameObject> interactables;
    public static GameObject interactable;
    public static Transform playerInteractTransform;
    bool isInteractable;
    bool isInteracting;
    bool inInteractableRange;
    MonoBehaviour interScript;
    Vector3 playerInteractPos;
    Quaternion playerInteractRot;
    [SerializeField]
    float playerInteractPosSpeed;
    [SerializeField]
    float playerInteractRotSpeed;
    Vector3 startInteractPos;
    Quaternion startInteractRot;
    [SerializeField]
    GameObject interactCanvas;
    [SerializeField]
    float interactCanvasY;
    bool ePressed;

    //Door
    public bool doorTriggered;
    bool nearDoor;
    bool openingDoor;
    Vector3 nearDoorOffset;
    Vector3 nearDoorPosition;
    Vector3 stepBackOffset;
    Vector3 stepBackPosition;

    //Candy Machine Gun Stand
    [SerializeField]
    bool isCandyGunning;
    [SerializeField]
    GameObject candyGunCam;
    [SerializeField]
    GameObject candyAirplaneCam;

    MachineGunController mgcScript;

    //Item Pickup
    bool isPickupRoutine;
    [SerializeField]
    bool isPickingUp;
    [SerializeField]
    bool isPickupWeapon;
    GameObject currentPickupItem;
    GameObject currentInterPickupItem;
    bool isPickedUpWeapon;
    bool isThrowingWeapon;
    bool pickUpEnd;
    //Items
    [SerializeField]
    GameObject poop;
    

    //Hit Control
    [SerializeField]
    bool isHit;
    [SerializeField]
    bool isAirborneHit;
    [SerializeField]
    Transform frontTrans;
    [SerializeField]
    Transform backTrans;
    bool hitFront;
    float frontDistance;
    float backDistance;
    [SerializeField]
    float airHitHeight;
    [SerializeField]
    float airHitPower;
    int airHitDirection;
    [SerializeField]
    int airborneHitThreshold;
    bool isGrabbed;
    float hitDirection;
    [SerializeField]
    UnityEvent damage;

    
 
    private void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked; *turns off mouse*
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;

        //INTERACTABLE
        interactables = new List<GameObject>();

        //DOOR//
        nearDoorOffset = new Vector3(.01f, 0f, 13.75f);
        stepBackOffset = new Vector3(0f, 0f, 0.805f);
        idleRotation = Quaternion.Euler(0f, 90f, 0f);

        pooler = new ObjectPooler();
    }

    private void OnEnable()
    {
        if (PersistantPlayerData.isGrabbed)
        {
            isAirborneHit = true;
            isShooting = false;
            weaponRaised = false;
            StartCoroutine(GrabWait());
        }
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

        gunSmoke = PooledObjectArrays.gunsSmokesArray;
        spentShells = PooledObjectArrays.shellCasingsArray;

        SetEquippedWeapon();

        StartCoroutine(IdleTimer());
    }

    private void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        rotMovement = new Vector3(-Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));

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

        if (!freezeMovement)
        {
            Run();
            Jump();
        }
        if(!isAirborneHit)
        {
            Stopped();
        }
        //DropFromLedge();
        //ClimbUpOnLedge();

        ////WEAPONS///
        anim.SetBool("isShooting", isShooting);
        //anim.SetBool("isContinueShooting", isContinueShooting);
        anim.SetBool("weaponRaised", weaponRaised);
        anim.SetBool("isRaising", isRaising);
        anim.SetBool("weaponAimed", weaponAimed);
        //anim.SetBool("weaponSheathed", weaponSheathed);
        anim.SetBool("hasKnife", hasKnife);
        anim.SetBool("hasFists", hasFists);
        anim.SetBool("hasMeleeTwoHanded", hasMeleeTwoHanded);
        anim.SetBool("hasDildo", hasDildo);
        anim.SetInteger("RandomSelection", randomSelection);

        if (!freezeMovement)
        {
            Shoot();
            LowerWeapon();
            QuickSelectWeapon();
        }
        currentWeapon = equippedWeapon;

        ////IDLE////
        anim.SetBool("isFlipping", isFlipping);
        anim.SetInteger("idleStateRandom", idleStateRandom);

        ///INTERACTABLE///

        SetInteractable(); //checks if there are interactables within range via OnTriggerEnter/Exit data
        anim.SetBool("inInteractableRange", inInteractableRange);
        anim.SetBool("isInteracting", isInteracting);

        Interact();

        ///CANDY MACHINE GUN///
        anim.SetBool("isCandyGunning", isCandyGunning);
        //ResetCandyGunning();

        ////DOOR////
        anim.SetBool("nearDoor", nearDoor);
        OpenDoor();
        OpeningDoor();
        doorTriggered = DoorOpener.doorTriggered;

        ///ITEM PICKUP///
        anim.SetBool("isPickingUp", isPickingUp);
        anim.SetBool("isPickupWeapon", isPickupWeapon);
        ThrowPickupWeapon();

        ///HIT CONTROL///
        anim.SetBool("isAirborneHit", isAirborneHit);
        //Debug.Log("IsAirborneHit: " + isAirborneHit);
        
    }

    private void FixedUpdate()
    {
        if(!freezeMovement)
        {
            Movement();
        }
        JumpFall();
        if(isFalling)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 9f);
        }
        
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, -transform.up, out hit, groundRayDist))
        //{
        //    if (hit.collider.gameObject.layer == 6)
        //    {
        //        transform.up = hit.normal;
        //        //isGrounded = true;
        //        //Debug.Log("Grounded");
        //    }
        //    else
        //    {
        //        //isGrounded = false;
        //    }
        //}
        //Debug.DrawRay(transform.position, -transform.up * groundRayDist, Color.red);
    }

    private void LateUpdate()
    {
        if(!freezeMovement)
        {
            RotateGunArm();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 6)
        {
            isGrounded = true;
        }
        if(other.gameObject.layer == 19 && !isPickupRoutine) //if interactables are collided with this adds them to interactables list
        {
            if (!interactables.Contains(other.gameObject))
            {
                interactables.Add(other.gameObject);
                if (!isInteracting)//if no objects are currently interacting sets isInteractable to true
                {
                    //Debug.Log("Canvas Active: " + interactables[0].transform.parent.gameObject.name);
                    isInteractable = true;
                    interactCanvas.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            isGrounded = false;
            //Debug.Log("Grounded");
        }
        if (other.gameObject.layer == 19)//if interactables exit collision area this removes them from interactables list
        {
            if(interactables.Count == 1) //sets isInteractable to false when last object removed from list
            {
                isInteractable = false;
                interactCanvas.SetActive(false);
                //Debug.Log("No Interactables");
            }
            interactables.Remove(other.gameObject);
        }
    }

    /////BASIC MOVEMENT
    
    void Movement()
    {
        if (!isStopping && !isClimbingUp && !isHanging && !freezeMovement && !isAirborneHit)
        {
            
            if (movement.magnitude > 0f)
            {
                isMoving = true;
                lookRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotMovement, Vector3.up), 360f * Time.deltaTime * turnSpeed);
                rb.MoveRotation(lookRotation);
                lookRot = Quaternion.LookRotation(rotMovement, Vector3.up);
                rotDifference = transform.eulerAngles.y - lookRot.eulerAngles.y;
                //rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
                //rb.velocity = transform.right + movement * moveSpeed * Time.deltaTime * moveSpeedMulti;
                Vector3 move = transform.right + movement * moveSpeed * Time.deltaTime * speedMulti;
                rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
                //rb.AddForce(transform.right + movement * moveSpeed * moveSpeedMulti, ForceMode.Force);
                ResetIdle();
                if (!isGrounded)
                {
                    moveSpeed = jumpSpeed;
                }
                else if(isRunning)
                {
                    if (Mathf.Abs(rotDifference) > 35)
                    {
                        moveSpeed = 0f;
                    }
                    else
                    {
                        moveSpeed = runSpeed;
                    } 
                }
                else
                {
                    if(Mathf.Abs(rotDifference) > 35)
                    {
                        moveSpeed = 0f;
                    }
                    else
                    {
                        moveSpeed = walkSpeed;
                    }
                    //Debug.Log("rotDifference: " + rotDifference);

                }
            }
            else
            {
                isMoving = false;
            }
        }
        else
        {
            isMoving = false;
        }

    }
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
    //void Move()
    //{
    //    if (!isStopping && !isClimbingUp && !isHanging && !FreezeMovement())
    //    {
    //        if (isGrounded)
    //        {
    //            MoveButtons(moveSpeed);
    //        }
    //        else
    //        {
    //            MoveButtons(jumpSpeed);
    //        }

    //    }
    //    else
    //    {
    //        //Debug.Log("Stopping");
    //        //Stop();
    //    }
    //}

    //void MoveButtons(float speed)
    //{
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        if (movingRight)
    //        {
    //            StartCoroutine(TurnWait(movingRight));
    //        }
    //        else if (isStopping)
    //        {
    //            StartCoroutine(StopWait());
    //        }
    //        else if (movingUp)
    //        {
    //            MoveSettings(-1f, 1f, "Left");
    //            movingLeft = true;
    //            movingRight = false;
    //        }
    //        else if (movingDown)
    //        {
    //            MoveSettings(-1f, -1f, "Left");
    //            movingLeft = true;
    //            movingRight = false;
    //        }
    //        else
    //        {

    //            MoveSettings(-1f, 0f, "Left");
    //            movingLeft = true;
    //            movingRight = false;
    //        }

    //    }
    //    else if (Input.GetKey(KeyCode.D))
    //    {
    //        if (movingLeft)
    //        {
    //            Debug.Log("Left");
    //            StartCoroutine(TurnWait(movingLeft));
    //        }
    //        else if (isStopping)
    //        {
    //            StartCoroutine(StopWait());
    //        }
    //        else if (movingUp)
    //        {
    //            MoveSettings(1f, 1f, "Right");
    //            movingLeft = true;
    //            movingRight = false;
    //        }
    //        else if (movingDown)
    //        {
    //            MoveSettings(1f, -1f, "Right");
    //            movingLeft = true;
    //            movingRight = false;
    //        }
    //        else
    //        {
    //            MoveSettings(1f, 0f, "Right");
    //            movingLeft = false;
    //            movingRight = true;
    //        }
    //    }
    //    else if (Input.GetKey(KeyCode.W))
    //    {
    //        if (movingLeft)
    //        {
    //            MoveSettings(-1f, 1f, "Left");
    //            movingUp = true;
    //            movingDown = false;
    //        }
    //        else if (movingRight)
    //        {
    //            MoveSettings(1f, 1f, "Right");
    //            movingUp = true;
    //            movingDown = false;
    //        }
    //        else if (isStopping)
    //        {
    //            StartCoroutine(StopWait());
    //        }
    //        else
    //        {
    //            MoveSettings(0f, 1f, "Up");
    //        }
    //    }

    //    else if (Input.GetKey(KeyCode.S))
    //    {
    //        if (movingLeft)
    //        {
    //            MoveSettings(-1f, 1f, "Left");
    //            movingUp = false;
    //            movingDown = true;
    //        }
    //        else if (movingRight)
    //        {
    //            MoveSettings(1f, 1f, "Right");
    //            movingUp = false;
    //            movingDown = true;
    //        }
    //        else if (isStopping)
    //        {
    //            StartCoroutine(StopWait());
    //        }
    //        else
    //        {
    //            MoveSettings(0f, -1f, "Down");
    //        }
    //    }

    //    else
    //    {
    //        //Debug.Log("Stopping");
    //        movingRight = false;
    //        movingLeft = false;
    //        movingUp = false;
    //        movingDown = false;
    //        isMoving = false;
    //        velocityX = 0f;
    //        velocityZ = 0f;
    //        if (!stopped)
    //        {
    //            Stop(.25f);
    //        }
    //    }

    //    velocityX = xMovement * speed;
    //    velocityZ = zMovement * speed * 3;
    //    rb.velocity = new Vector3(velocityX, rb.velocity.y, velocityZ);
    //}

    //void MoveSettings(float xMovementNum, float zMovementNum, string facing)
    //{
    //    //Debug.Log("Changed");
    //    stopped = false;
    //    isMoving = true;
    //    xMovement = xMovementNum;
    //    zMovement = zMovementNum;

    //    //IDLE//
    //    ResetIdle();

    //    if (facing == "Left")
    //    {
    //        facingLeft = true;
    //        facingRight = false;
    //        facingUp = false;
    //        facingDown = false;
    //    }
    //    else if (facing == "Right")
    //    {
    //        facingLeft = false;
    //        facingRight = true;
    //        facingUp = false;
    //        facingDown = false;
    //    }
    //    else if(facing == "Up")
    //    {
    //        facingLeft = false;
    //        facingRight = false;
    //        facingUp = true;
    //        facingDown = false;
    //    }
    //    else
    //    {
    //        facingLeft = false;
    //        facingRight = false;
    //        facingUp = false;
    //        facingDown = true;
    //    }
    //}

    //IEnumerator TurnWait(bool movingDir)
    //{
    //    while (movingDir)
    //    {
    //        yield return null;
    //    }
    //    movingDir = false;
    //}

    //IEnumerator StopWait()
    //{

    //    while (isStopping)
    //    {
    //        yield return null;
    //    }
    //    yield break;
    //}

    //IEnumerator ShootWaitTurn()
    //{
    //    StartCoroutine(StopWait());
    //    while (isShooting)
    //    {
    //        yield return null;
    //    }
    //    yield break;
    //}

    //void MoveButtonsUp()
    //{
    //    if (Input.GetKeyUp(KeyCode.A))
    //    {
    //        movingLeft = false;
    //        if(!movingRight || !movingUp || !movingDown)
    //        {
    //            //Debug.Log("Stopped A");
    //            Stop(.1f);
    //        }
    //    }
    //    if (Input.GetKeyUp(KeyCode.D))
    //    {
    //        movingRight = false;
    //        if (!movingLeft || !movingUp || !movingDown)
    //        {
    //            //Debug.Log("Stopped D");
    //            Stop(.1f);
    //        }
    //    }
    //    if (Input.GetKeyUp(KeyCode.W))
    //    {
    //        movingUp = false;
    //        if (!movingLeft || !movingRight || !movingDown)
    //        {
    //            //Debug.Log("Stopped W");
    //            Stop(.1f);
    //        }
    //    }
    //    if (Input.GetKeyUp(KeyCode.S))
    //    {
    //        movingDown = false;
    //        if (!movingLeft || !movingRight || movingUp)
    //        {
    //            //Debug.Log("Stopped S");
    //            Stop(.1f);
    //        }
    //    }
    //}
    public void Stop(float stopWait)
    {
        //Debug.Log("Stopping");
        StartCoroutine(StopRoutine(stopWait));
    }

    void Stopped()
    {
        if (movement.magnitude < .01f)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    IEnumerator StopRoutine(float stopWait)
    {
        isStopping = true;
        yield return new WaitForSeconds(stopWait);
        xMovement = 0f;
        zMovement = 0f;
        isStopping = false;
        stopped = true;
        yield break;
        //Debug.Log("Stopped");
    }

    void KillStopRoutine()
    {
        StopCoroutine("StopRoutine");
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

                }
                else
                {
                    isRunning = false;

                }
            }
        }
    }

    void Jump()
    {
        if (isGrounded && !freezeMovement)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                anim.SetTrigger("Jump");
                isJumping = true;
                isFalling = false;
                moveSpeed = jumpSpeed;
                ResetIdle();
                KillStopRoutine();
                StartCoroutine(JumpDelay());
            }
        }
    }

    IEnumerator JumpDelay()
    {
        isJumpDelaying = true;
        float jumpWait = 0;
        while(jumpWait < jumpDelay)
        {
            jumpWait += Time.deltaTime;
            rb.velocity = Vector3.zero;
            yield return null;
        }
        isJumpDelaying = false;
        isStopping = false;
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
        //Debug.Log("Hung");
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
        if (!isShooting && !isHanging && !isPickupWeapon)
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
        automatic = PlayerInfo.automatic[equippedWeapon];
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
            if (!weaponRaised && !WeaponAlwaysRaised())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!isRaising)
                    {
                        isRaising = true;
                        StartCoroutine(RaiseWeaponWait());
                    }  
                }
            }
            //else if (weaponSheathed)//Add when weapon sheathing added
            //                //{
            //                //    weaponSheathed = false;
            //                //    StartCoroutine(UnSheathWeaponWait());
            //                //}
            else
            {
                if(!isShooting && !isShootEnd)
                {
                    if (automatic)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            isShooting = true;
                            StartCoroutine(ShootWait());
                            Debug.Log("Shoot");
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            isShooting = true;
                            isShootEnd = true;
                            StartCoroutine(ContinueShooting());
                            Debug.Log("Shoot");
                        }
                    }
                }
                
            }
        }

    }
    //void Shoot()
    //{

    //    if (weaponEquipped)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            ResetIdle();
    //            if (!isShooting && !freezeMovement)
    //            {
    //                if (!weaponRaised && !WeaponAlwaysRaised())
    //                {
    //                    weaponRaised = true;
    //                    StartCoroutine(RaiseWeaponWait());
    //                }
    //                //else if (weaponSheathed)
    //                //{
    //                //    weaponSheathed = false;
    //                //    StartCoroutine(UnSheathWeaponWait());
    //                //}
    //                else
    //                {
    //                    Debug.Log("Shoot");
    //                    isShooting = true;
    //                    StartCoroutine(ContinueShooting());
    //                    //anim.SetTrigger("Shoot");
    //                    //StartCoroutine(ShootWait());
    //                }
    //            }
    //        }
    //    } 
    //}

    IEnumerator RaiseWeaponWait()
    {
        yield return new WaitForSeconds(raiseWait);
        weaponRaised = true;
        isRaising = false;
        //Debug.Log("Raising");
        yield break;
    }
    IEnumerator UnSheathWeaponWait()
    {
        yield return new WaitForSeconds(unsheathWait);
        yield break;
    }

    IEnumerator ContinueShooting() // Decides whether or not youve hit the attack button again
    {
        shootDelay = true;
        yield return new WaitForSeconds(.01f);
        while(shootDelay)
        {
            if(!isContinueShooting && Input.GetMouseButtonDown(0))
            {
                isContinueShooting = true;
                //Debug.Log("Clicked");
                yield break;
            }
            //Debug.Log(i);
            yield return null;
        }
        isContinueShooting = false;
        isShooting = false;
        //Debug.Log("Stop");
        yield break;
    }

    public void SetIsContinueShooting() //Put on second to last frame of attack animation to decide if attack continues
    {
        if(isContinueShooting)
        {
            //Debug.Log("Continue");
            StartCoroutine(ContinueShooting());
            isContinueShooting = false;
        }
        else
        {
            shootDelay = false;
            isShooting = false;
        }
    }
    IEnumerator ShootWait()
    {
        //shoot
        yield return new WaitForSeconds(shootWait);
        isShooting = false;
        yield break;
    }

    public void ResetIsShootEnd()// Put as event at end of attack animations
    {
        isShootEnd = false;
    }

    public void ResetShootDelay()// Put as event on second to last frame of last animation in series to stop attack
    {
        shootDelay = false;
        isShooting = false;
        isContinueShooting = false;
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

        ////Debug.Log("ShootWait: " + raiseWait);
        ////Debug.Log("RaiseWait: " + name);
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

    void ActivePassiveWeapons(int equippedWeapon)
    {
        if (activeWeapons[equippedWeapon].activeInHierarchy)
        {
            passiveWeapons[equippedWeapon].SetActive(false);
        }
        else
        {
            passiveWeapons[equippedWeapon].SetActive(true);
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
        pooler.PoolObjects(gunSmoke, muzzleFlash.transform.position, muzzleFlash.transform.rotation, Vector3.zero);
        if (spendShells)
        {
            pooler.PoolObjects(spentShells, spentShellsSpawns[spawnIndex].position, muzzleFlash.transform.rotation, Vector3.zero);
        }
    }

    ///Vehicle Weapons
    void SetVehicleWeaponInfo(int selectedVehicleWeapon)
    {
        vehicleWeaponID = PlayerInfo.vehicleWeaponID[selectedVehicleWeapon];
        vehicleWeaponPower = PlayerInfo.vehicleWeaponPower[selectedVehicleWeapon];
        vehicleWeaponShootWait = PlayerInfo.vehicleWeaponShootWait[selectedVehicleWeapon];
    }

    /////IDLE/////
    public void IsIdle()
    {
        if (!isIdle && !freezeMovement && !isPickupWeapon)
        {
            isIdle = true;
            //StartCoroutine(IdleTimer()); temp disabled
            //Debug.Log("Idle Starting");
        }
    }

    IEnumerator IdleTimer()
    {
        idleWait = 0;
        while (isIdle)
        {
            idleWait++;
            if (idleWait == 15)
            {
                idleWait = 0;
                StartLongIdle();
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
        //Debug.Log("Ended Idle Timer");
        yield break;
    }

    void StartLongIdle()
    {
        if (!freezeMovement)
        {
            anim.SetTrigger("idleChange");
            activeWeapons[equippedWeapon].SetActive(false);
            anim.SetLayerWeight(activeWeaponLayer, 0);
            //create random int to select new idle state eg. devil sticks, hackeysack etc.
            isFlipping = true; //temp: move to new method that selects idle state
            ActivateFlipSticks(); //temp: move also
            StartCoroutine(IdleRotate());
        }
    }
    IEnumerator IdleRotate()
    {
        while (rb.rotation != idleRotation)
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, idleRotation, Time.deltaTime * turnSpeed * 2f));
            yield return null;
        }
        yield break;
    }

    void ResetIdle()
    {
        if (isIdle && !isInteracting)
        {
            isIdle = false;
            alreadyIdle = false;
            isFlipping = false;
            activeWeapons[equippedWeapon].SetActive(true);
            anim.SetLayerWeight(activeWeaponLayer, 1);
            DeActivateFlipSticks();
            StopCoroutine(IdleTimer());
            ResetIdleStateSelected();
            //Debug.Log("Reset Idle");
        }
    }
    void ResetIdlePickupWeapon()
    {
        isIdle = false;
        alreadyIdle = false;
        isFlipping = false;
        DeActivateFlipSticks();
        StopCoroutine(IdleTimer());
        ResetIdleStateSelected();
    }

    void ActivateFlipSticks()
    {
        foreach (GameObject stick in devilSticks)
        {
            stick.SetActive(true);
        }
    }
    void DeActivateFlipSticks()
    {
        foreach (GameObject stick in devilSticks)
        {
            stick.SetActive(false);
        }
    }

    public void IdleStateRandomInt()
    {
        if (!idleStateSelected)
        {
            idleStateSelected = true;
            idleStateRandom = Random.Range(0, 3);
        }

    }
    public void ResetIdleStateSelected()
    {
        idleStateSelected = false;
    }

    /////INTERACTABLE/////////////////
    ///

    void Interact()
    {
        if(isGrounded && !isShooting && !isPickupRoutine && !ePressed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ePressed = true;
                //Debug.Log("E Pressed");
                if (interactables.Count > 0 && isInteractable)
                {
                    interactCanvas.SetActive(false);
                    SetInteractableData(interactable.tag);
                    //Debug.Log("Close Canvas");
                }
                else if (isInteracting)
                {
                    ResetInteractableData(interactable.tag);
                    interactCanvas.SetActive(true);
                    //Debug.Log("Reset");
                }
                StartCoroutine(ResetInteractKey());
            }
        }
    }

    IEnumerator ResetInteractKey()
    {
        yield return new WaitForSeconds(.1f);
        ePressed = false;
    }
    void SetInteractable() //sets static variable interactable to first Gameobject in interactable list
    {
        if (interactables.Count > 0)
        {
            interactable = interactables[0];
            interactCanvas.transform.position = new Vector3(interactable.transform.position.x, interactCanvasY, interactable.transform.position.z);
            //pop up canvas with "E" prompt
        }
        //else
        //{
        //    interactable = null;
        //}
    }

    void SetInteractableData(string tag) //sets static interactable data based on its tag and starts relevant coroutines and methods
    {
        isInteractable = false;
        physCol.enabled = false;
        freezeMovement = true;
        isInteracting = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        activeWeapons[equippedWeapon].SetActive(false);
        anim.SetLayerWeight(activeWeaponLayer, 0);
        switch (tag)
        {
            case "CandyMachineGun":
                StartCoroutine(CandyGunInteract());
                //Debug.Log("Started");
                break;

            case "WeaponItem":
                isPickupRoutine = true;
                currentInterPickupItem = interactables[0];
                StartCoroutine(WeaponItemInteract(currentInterPickupItem.name));
                //Debug.Log("Pickup Routine");
                break;
        }
    }

    void ResetInteractableData(string tag)
    {
        physCol.enabled = true;
        activeWeapons[equippedWeapon].SetActive(true);
        anim.SetLayerWeight(activeWeaponLayer, 1);
        rb.isKinematic = false;
        inInteractableRange = false;
        isInteracting = false;
        isInteractable = true;
        freezeMovement = false;

        switch (tag)
        {
            case "CandyMachineGun":
                mgcScript.isGunning = false;
                isCandyGunning = false;
                mainPPVolume.SetActive(true);
                candyAirplaneCam.SetActive(false);
                StopCoroutine(CandyGunInteract());
                break;

            //case "WeaponItem":

        }
    }

    //Candy Machine Gun
    IEnumerator CandyGunInteract()
    {
        //set colliders to inactive
        isCandyGunning = true;
        anim.SetTrigger("CandyGun");
        mgcScript = interactable.GetComponent<MachineGunController>();
        playerInteractTransform = mgcScript.playerInteractTransform;
        playerInteractPos = playerInteractTransform.position;
        playerInteractRot = playerInteractTransform.rotation;

        //while (transform.rotation != playerInteractRot && transform.position != playerInteractPos) // move to interact rotation
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, playerInteractRot, Time.deltaTime * playerInteractRotSpeed);
        //    transform.position = Vector3.Lerp(transform.position, playerInteractPos, Time.deltaTime * playerInteractPosSpeed);
        //    if (Vector3.Distance(transform.position, playerInteractPos) < .1f)
        //    {
        //        transform.position = playerInteractPos;
        //    }
        //    yield return null;
        //}
        float i = 0;
        Quaternion oRot = transform.rotation;
        while (transform.rotation != playerInteractRot) // move to interact rotation
        {
            transform.rotation = Quaternion.Lerp(oRot, playerInteractRot, i);
            i += playerInteractRotSpeed;
            //Debug.Log("Rot: " + Mathf.Abs(transform.rotation.y - playerInteractRot.y));
            if (Mathf.Abs(transform.rotation.y - playerInteractRot.y) < 1.5f)
            {
                transform.rotation = playerInteractRot;
            }
            yield return null;
        }
        float j = 0;
        Vector3 oPos = transform.position;
        while (transform.position != playerInteractPos) // move to interact rotation
        {
            transform.position = Vector3.Lerp(oPos, playerInteractPos, j);
            j += playerInteractPosSpeed;
            //Debug.Log("Pos: " + Vector3.Distance(transform.position, playerInteractPos));
            if (Vector3.Distance(transform.position, playerInteractPos) < .1f)
            {
                transform.position = playerInteractPos;
            }
            yield return null;
        }
        inInteractableRange = true;
        mgcScript.isGunning = true; //starts interactable objects animation
        yield return new WaitForSeconds(.3F);
        candyGunCam.SetActive(true);
        yield return new WaitForSeconds(.3F);
        candyGunCam.SetActive(false);
        candyAirplaneCam.SetActive(true);
        mainPPVolume.SetActive(false);
        selectedVehicleWeapon = 0;
        SetVehicleWeaponInfo(selectedVehicleWeapon);
        while (isCandyGunning)
        {
            yield return null;
        }
        yield return null;
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

    ///ITEM PICKUP///
    IEnumerator WeaponItemInteract(string name) //Items picked up that can be thrown as weapons
    {
        switch (name) //sets current pickup item based on collider object tag
        {
            case "Poop":
                currentPickupItem = poop;
                break;
        }
        anim.SetTrigger("ItemPickUp");
        isInteracting = false;
        isPickupRoutine = true;
        isPickingUp = true;
        isPickupWeapon = true;
        Vector3 lookRot = currentInterPickupItem.transform.parent.position - transform.position; //sets rotation target as pickup item
        Vector3 lookRotFixed = new Vector3(lookRot.x, 0f, lookRot.z);
        float rotSpeed = .1f;
        //Vector3 lookRotFixed = new Vector3()
        while (isPickingUp) // rotates player toward item on ground while bending over animation plays
        {
            rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookRotFixed, Vector3.up), rotSpeed)); //rotate towards pickup item
            yield return null;
        }
        isPickedUpWeapon = true;
        pickUpEnd = true;
        ResetIdlePickupWeapon();
        currentPickupItem.SetActive(true);
        currentInterPickupItem.transform.parent.gameObject.SetActive(false);
        while (pickUpEnd)
        {
            yield return null;
        }
        anim.SetLayerWeight(9, 1); // sets pickup arms layer to active
        physCol.enabled = true;
        rb.isKinematic = false;
        freezeMovement = false;
        yield break;
    }

    public void ResetPickUpEnd()
    {
        pickUpEnd = false;
    }

    void ThrowPickupWeapon()
    {
        if (isPickedUpWeapon)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetLayerWeight(9, 0f);
                anim.SetTrigger("ItemWeaponThrow");
                isThrowingWeapon = true;
                StartCoroutine(ItemWeaponThrowWait());

            }
        }
    }

    IEnumerator ItemWeaponThrowWait()
    {
        while (isThrowingWeapon)
        {
            freezeMovement = true;
            yield return null;
        }
        yield break;
    }
    public void ResetIsThrowingWeapon()
    {
        isThrowingWeapon = false;
        freezeMovement = false;
    }

    public void ResetIsPickingUp()
    {
        isPickingUp = false;
    }
    public void ResetPickupItemData()
    {
        StopCoroutine(WeaponItemInteract(currentInterPickupItem.name));
        freezeMovement = false;
        inInteractableRange = false;
        currentPickupItem.SetActive(false);
        isInteracting = false;
        activeWeapons[equippedWeapon].SetActive(true);
        anim.SetLayerWeight(activeWeaponLayer, 1);
        anim.SetLayerWeight(9, 0);
        isPickupWeapon = false;
        isPickingUp = false;
        pickUpEnd = false;
        isPickedUpWeapon = false;
        isThrowingWeapon = false;
        isPickupRoutine = false;
    } // Resets all data related to picking up items when hit or as an event at the end of pick up animations
    public void SpawnThrowItem()
    {
        switch(currentInterPickupItem.name)
        {
            case "Poop":
                pooler.PoolObjects(PooledObjectArrays.throwPoopArray, currentPickupItem.transform.position, currentPickupItem.transform.rotation, Vector3.zero);
                break;
        }
    }

    ///HIT///  Need to set up airborneHitNum that increases on hit to decide when to go airborne.
    
    public void HitController()
    {
        if (!isHit)
        {
            isHit = true;
            //Debug.Log("Hit");
            SetHitDirection();
            SetAirborneHit();
            ResetIdle();
            if (isAirborneHit)
            {
                anim.SetLayerWeight(activeWeaponLayer, 0);
                Vector3 dir  = (transform.position - PlayerHitController.hitPos).normalized;
                rb.AddForce(new Vector3(dir.x, airHitHeight, dir.z) * airHitPower, ForceMode.Impulse);
                TotalReset();
                anim.SetTrigger("airborne");
                //Debug.Log("Airborne Dir: " + new Vector3(dir.x, airHitHeight, dir.z));
            }
            if (hitFront)
            {
                anim.SetTrigger("hitFront");
                Debug.Log("Hit Front");
            }
            else
            {
                anim.SetTrigger("hitBack");
                Debug.Log("Hit Back");
            }
            if (isPickupRoutine)
            {
                ResetPickupItemData();
                Debug.Log("Reset Pickup Data");
            }
            damage.Invoke();
            StartCoroutine(HitWait());
        }
    }

    public void AirborneHit()
    {
        isAirborneHit = true;
    }

    IEnumerator HitWait()
    {
        while (isHit)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10f);
            //Debug.Log("VEL: " + rb.velocity.magnitude);
            yield return null;
        }
        StartCoroutine(WeaponLayerAdd());
        isAirborneHit = false;
        PersistantPlayerData.isGrabbed = false;
        PlayerHitController.isHit = false;
        yield break;
    }

    public void ResetIsHit()
    {
        isHit = false;
        //Debug.Log("Hit Reset");
    }

    void SetHitDirection()
    {
        //Debug.Log("SetHitDir  Pos: " + PlayerHitController.hitPos + " " + "FrontTrans: " + frontTrans.position + " " + "BackTrans: " + backTrans.position);
        //frontDistance = frontTrans.position - PlayerHitController.hitPos;
        //backDistance = backTrans.position - PlayerHitController.hitPos;
        frontDistance = (frontTrans.position - PlayerHitController.hitPos).magnitude;
        backDistance = (backTrans.position - PlayerHitController.hitPos).magnitude;
        //Debug.Log($"{frontDistance} {backDistance}");
        //if (Mathf.Abs(frontDistance.x) < Mathf.Abs(backDistance.x))
        if(Mathf.Abs(frontDistance) < Mathf.Abs(backDistance))
        {
            hitFront = true;
            hitDirection = -1f;
        }
        else
        {
            hitFront = false;
            hitDirection = 1f;
        }
    }
    void SetAirborneHit()
    {
        if(PlayerHitController.isAirborne)
        {
            isAirborneHit = true;
        }
    }

    void TotalReset()//Add anything here that needs to revert before going airborne
    {
        isShootEnd = false;
        isShooting = false;
        isContinueShooting = false;
        isThrowingWeapon = false;
    }

    IEnumerator GrabWait()
    {
        yield return new WaitForSeconds(.01f);
        HitController();
        yield break;
        //Debug.Log("Grabbed :" + PersistantPlayerData.otherGameObject.name);
    }

    IEnumerator WeaponLayerAdd()
    {
        float i = .05f;
        float j;
        while(anim.GetLayerWeight(activeWeaponLayer) < 1f)
        {
            j = anim.GetLayerWeight(activeWeaponLayer);
            anim.SetLayerWeight(activeWeaponLayer, j + i);
            yield return null;
        }
    }
}
