using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpermShipController : MonoBehaviour
{
    ObjectPooler pooler;

    float shootWait;
    Coroutine shootRoutine;

    [SerializeField]
    Transform bulletSpawn;
    [SerializeField]
    GameObject[] muzzleFlashes;
    float muzzleFlashX;
    float muzzleFlashZ;

    //Movement//
    Vector3 movement;
    Rigidbody rb;
    [SerializeField]
    float velocity;
    [SerializeField]
    float moveSpeed;
    float speed;
    [SerializeField]
    float moveSpeedMulti;
    Quaternion lookRot;
    float rotDifference;
    Quaternion lookRotation;
    Vector3 rotMovement;
    [SerializeField]
    float turnSpeed;
    float dirX;
    float dirY;
    [SerializeField]
    ParticleSystem bodyPS;
    ParticleSystem.EmissionModule bodyPSEmission;
    float bodyPSStartAmount = 50;
    float bodyPSEndAmount = 150;

    //Propeller//
    [SerializeField]
    Transform propeller;
    [SerializeField]
    float maxPropSpeed;
    float currentPropSpeed;
    [SerializeField]
    float propSpeedMulti;
    bool isAccelerating;
    Coroutine acc;
    Coroutine dec;
    [SerializeField]
    ParticleSystem propPS;
    ParticleSystem.MainModule propPSMain;
    ParticleSystem.MinMaxCurve propPSStartCurve;
    [SerializeField]
    ParticleSystem.MinMaxCurve propPSTurnCurve;


    //Prop Post//
    [SerializeField]
    Transform propPost;
    float propPostRotMaxX;
    float propPostRotMinX;
    float propPostRotMaxZ;
    float propPostRotMinZ;
    Vector3 propPostRotO;
    bool isTurningX;
    bool isTurningZ;
    float currentPostAngleX;
    float currentPostAngleZ;
    [SerializeField]
    float maxPostAngleX;
    [SerializeField]
    float maxPostAngleZ;
    [SerializeField]
    float postSpeedMulti;
    float selectedPostXDir;
    float selectedPostZDir;
    Coroutine postTurnXAcc;
    Coroutine postTurnZAcc;
    Coroutine postTurnXDec;
    Coroutine postTurnZDec;
    //-103
    //-77

    public static int directionX;
    [SerializeField]
    Transform frontTrans;
    [SerializeField]
    Transform backTrans;
    float startTurnDistance;
    float currentTurnDistance;
    public static bool isTurning;
    bool midTurn;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        pooler = new ObjectPooler();
        currentPropSpeed = 0f;
        currentPostAngleX = 0f;
        currentPostAngleZ = 0f;
        propPostRotO = propPost.localEulerAngles;
        propPostRotMinX = propPost.localEulerAngles.x - 13f;
        propPostRotMaxX = propPost.localEulerAngles.x + 13f;
        propPostRotMinZ = propPost.localEulerAngles.z - 13f;
        propPostRotMaxZ = propPost.localEulerAngles.z + 13f;

        propPSMain = propPS.main;
        propPSStartCurve = propPSMain.startSpeed;
        bodyPSEmission = bodyPS.emission;
        bodyPSEmission.rateOverTime = bodyPSStartAmount;

        startTurnDistance = Mathf.Abs(frontTrans.position.x - backTrans.position.x);
        muzzleFlashX = muzzleFlashes[0].transform.localScale.x;
        muzzleFlashZ = muzzleFlashes[0].transform.localScale.z;

        isTurning = false;
        Setdirection();

    }
    private void Start()
    {
        shootWait = PlayerInfo.vehicleWeaponShootWait[1];
        shootRoutine = StartCoroutine(Shoot());
    }

    private void OnDisable()
    {
        StopCoroutine(shootRoutine);
    }

    private void Update()
    {
        StopTurningMovement();
        //rotMovement = new Vector3(0f, 0f, Input.GetAxisRaw("Horizontal"));
        SetDirX();
        SetDirY();
        
        PropellerRotate();
        PropPostMoverX();
        PropPostMoverZ();
        propPost.localEulerAngles = new Vector3(-90f + (currentPostAngleZ * movement.y), 0f, 0f/*(currentPostAngleX * dirX)*/);

        Setdirection();
        currentTurnDistance = Mathf.Abs(frontTrans.position.x - backTrans.position.x);
        SetIsTurning();
        //Debug.Log("IsTurning: " + isTurning);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);

    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if(movement.magnitude > 0)
        {
            lookRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotMovement, Vector3.up), 360f * Time.deltaTime * turnSpeed);
            rb.MoveRotation(lookRotation);
            lookRot = Quaternion.LookRotation(rotMovement, Vector3.up);
            rotDifference = transform.eulerAngles.y - lookRot.eulerAngles.y;
            if (Mathf.Abs(rotDifference) > 35)
            {
                speed = 0f;
            }
            else
            {
                speed = moveSpeed;
            }
            if (!isTurning)
            {
                Vector3 moveX = transform.right + movement * speed * Time.deltaTime * moveSpeedMulti;
                Vector3 moveY = transform.up + movement * speed * Time.deltaTime * moveSpeedMulti;
                Vector3 move = movement * speed * Time.deltaTime * moveSpeedMulti;
                rb.velocity = move;
            }
            //rb.velocity = new Vector3(moveX.x, moveY.y, 0f);
        }
    }

    void StopTurningMovement()
    {
        if(!isTurning)
        {
            midTurn = false;
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;
            rotMovement = new Vector3(0f, 0f, dirX);
        }
        else
        {
            int dir = -directionX;
            if (!midTurn)
            {
                midTurn = true;
                movement = new Vector3(dir, Input.GetAxisRaw("Vertical"), 0f);
                rotMovement = new Vector3(0f, 0f, dir);
            }
        }
    }

    void SetDirX()
    {
        if(movement.x > 0f)
        {
            dirX = 1f;
        }
        else if (movement.x < 0f)
        {
            dirX = -1f;
        }
    }
    void SetDirY()
    {
        if (movement.y > 0f)
        {
            dirY = 1f;
        }
        else if (movement.y < 0f)
        {
            dirY = -1f;
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            while (Input.GetMouseButton(0))
            {
                pooler.PoolObjects(PooledObjectArrays.spermBulletArrays, bulletSpawn.position, bulletSpawn.rotation, Vector3.zero);
                int i = 0;
                foreach(GameObject flash in muzzleFlashes)
                {
                    muzzleFlashes[i].SetActive(true);
                    float xy = Random.Range(muzzleFlashX - 0.2f, muzzleFlashX + 0.2f);
                    muzzleFlashes[i].transform.localScale = new Vector3(xy, xy, Random.Range(muzzleFlashZ - .15f, muzzleFlashZ + .15f));
                    i++;
                }
                yield return new WaitForSeconds(shootWait);
            }
            yield return null;
        }
    }

    void PropellerRotate()
    {
        if (movement.magnitude > 0)
        {
            if (!isAccelerating)
            {
                if(dec != null)
                {
                    StopCoroutine(dec);
                }
                acc = StartCoroutine(PropMoving());
            }
        }
        else
        {
            if (isAccelerating)
            {
                if (acc != null)
                {
                    StopCoroutine(acc);
                }
                dec = StartCoroutine(PropStopped());
            }
        }
        propeller.localEulerAngles += new Vector3(0f, propeller.localEulerAngles.y * currentPropSpeed, 0f);
    }

    IEnumerator PropMoving()
    {
        isAccelerating = true;
        float f = 0f;
        propPSMain.loop = true;
        propPS.Play();
        bodyPSEmission.rateOverTime = bodyPSEndAmount;
        while (currentPropSpeed < maxPropSpeed)
        {
            f += propSpeedMulti;
            float speed = Mathf.Lerp(0f, maxPropSpeed, f);
            currentPropSpeed = speed;
            //propPSMain.startSpeed = f;
            yield return null;
        }
        yield break;
    }

    IEnumerator PropStopped()
    {
        isAccelerating = false;
        float f = 0f;
        float j = 1f;
        float k = propSpeedMulti/10;
        while (currentPropSpeed > 0f)
        {
            if(currentPropSpeed > 2f)
            {
                f += propSpeedMulti;
            }
            else
            {
                f += k;
            }
            j -= propSpeedMulti;
            float speed = Mathf.SmoothStep(maxPropSpeed, 0f, f);
            currentPropSpeed = speed;
            if(j > 1f)
            {
                //propPSMain.startSpeed = j;
            }
            else
            {
                propPSMain.loop = false;
                bodyPSEmission.rateOverTime = bodyPSStartAmount;
            }
            yield return null;
        }
        yield break;
    }

    void PropPostMoverX()
    {
        if (movement.x == dirX)
        {
            if (!isTurningX)
            {
                if (postTurnXDec != null)
                {
                    StopCoroutine(postTurnXDec);
                }
                postTurnXAcc = StartCoroutine(TurnXAcc());
            }
            
        }
        else
        {
            if (isTurningX)
            {
                if (postTurnXAcc != null)
                {
                    StopCoroutine(postTurnXAcc);
                }
                postTurnXDec = StartCoroutine(TurnXDec());
            }
        }
    }
    void PropPostMoverZ()
    {
        if (movement.y != 0)
        {
            if (!isTurningZ)
            {
                if (postTurnZDec != null)
                {
                    StopCoroutine(postTurnZDec);
                }
                postTurnZAcc = StartCoroutine(TurnZAcc());
            }

        }
        else
        {
            if (isTurningZ)
            {
                if (postTurnZAcc != null)
                {
                    StopCoroutine(postTurnZAcc);
                }
                postTurnZDec = StartCoroutine(TurnZDec());
            }
        }
    }

    IEnumerator TurnXAcc()
    {
        isTurningX = true;
        float f = 0f;
        while(currentPostAngleX < 34)
        {
            f += postSpeedMulti;
            float speed = Mathf.Lerp(0f, 34, f);
            currentPostAngleX = speed;
            yield return null;
        }
        yield break;
    }

    IEnumerator TurnXDec()
    {
        isTurningX = false;
        float f = 0f;
        while (currentPostAngleX > 0f)
        {
            f += postSpeedMulti;
            float speed = Mathf.Lerp(34f, 0f, f);
            currentPostAngleX = speed;
            
            yield return null;
        }
        yield return null;
    }

    IEnumerator TurnZAcc()
    {
        isTurningZ = true;
        float f = 0f;
        while (currentPostAngleZ < maxPostAngleZ)
        {
            f += postSpeedMulti;
            float speed = Mathf.Lerp(0f, maxPostAngleZ, f);
            currentPostAngleZ = speed;
            propPSMain.startSpeed = propPSTurnCurve;
            yield return null;
        }
        yield break;
    }

    IEnumerator TurnZDec()
    {
        isTurningZ = false;
        float f = 0f;
        while (currentPostAngleZ > 0f)
        {
            f += postSpeedMulti;
            float speed = Mathf.Lerp(maxPostAngleZ, 0f, f);
            currentPostAngleZ = speed;
            propPSMain.startSpeed = propPSStartCurve;
            yield return null;
        }
        yield return null;
    }

    void Setdirection()
    {
        if (frontTrans.position.x > backTrans.position.x)
        {
            directionX = 1;
        }
        else
        {
            directionX = -1;
        }
    }

    void SetIsTurning()
    {
        if(currentTurnDistance + 0.06f < startTurnDistance)
        {
            isTurning = true;
        }
        else
        {
            isTurning = false;
        }
    }
}
