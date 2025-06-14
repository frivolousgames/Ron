using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GoochController : MonoBehaviour
{
    [SerializeField]
    bool isWalking;
    [SerializeField]
    bool isRunning;

    Animator anim;
    [SerializeField]
    Animator playerAnim;

    ///IK///
    //L Hand
    [SerializeField]
    Transform leftHandTarget;
    [SerializeField]
    Transform leftHand;

    //R Hand
    [SerializeField]
    Transform rightHandTarget;
    [SerializeField]
    float rightHandDist;
    [SerializeField]
    Rig rightArmRig;
    Vector3 rightHandStartPos;
    [SerializeField]
    float rArmRigMulti;

    //Neck
    [SerializeField]
    Transform neckTarget;
    [SerializeField]
    Rig neckRig;
    Vector3 headStartPos;
    Quaternion headStartRot;
    Vector3 headDirection;
    Vector3 headFixedDirection;
    Quaternion headRotation;
    [SerializeField]
    float headTurnSpeed;
    float headTurnMin = 140f;
    float headTurnMax = 220f;

    //Spine
    [SerializeField]
    Transform spineTarget;
    [SerializeField]
    Rig spineRig;
    Vector3 spineDirection;
    Quaternion spineRotation;
    Quaternion spineStartRot;
    Vector3 spineStartPos;
    [SerializeField]
    float spineTurnMin;
    [SerializeField]
    float spineTurnMax;
    [SerializeField]
    float spineTurnSpeed;
    [SerializeField]
    float spineWeightMulti;

    ///Poop Throwing
    GameObject selectedTarget;
    public static Vector3 selectedTargetPos;
    GameObject[] targets;
    bool isTargetable;
    bool reLoading;
    bool isThrowing;
    [SerializeField]
    float rangeMax;
    [SerializeField]
    float rangeMin;
    [SerializeField]
    float cooldownTime;
    [SerializeField]
    Transform poopSpawn;
    //public static Vector3 poopSpawnPos;

    ObjectPooler pooler;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pooler = new ObjectPooler();
        spineStartRot = spineTarget.localRotation;
        headStartRot = neckTarget.localRotation;
        rightHandStartPos = rightHandTarget.position;
    }
    private void OnEnable()
    {
        neckRig.weight = 0f;
        rightArmRig.weight = 0f;
        targets = GameObject.FindGameObjectsWithTag("Goochable");
        StartCoroutine(TargetCheckRoutine());
    }
    private void Update()
    {
        isWalking = playerAnim.GetBool("isMoving");
        isRunning = playerAnim.GetBool("isRunning");
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("reLoading", reLoading);
        anim.SetBool("isThrowing", isThrowing);

        ///IK///
        leftHand.position = leftHandTarget.position;
        //leftHand.rotation = leftHandTarget.rotation;

        ///Poop Throwing///
        targets = GameObject.FindGameObjectsWithTag("Goochable");
        CancelThrow();
        
    }

    ///IK///

    void RotateHead(Vector3 objPos)
    {
        if (selectedTarget != null)
        {
            headFixedDirection = new Vector3(objPos.x, neckTarget.transform.position.y, objPos.z);
            headRotation = Quaternion.LookRotation(headFixedDirection - neckTarget.position).normalized;
            neckTarget.rotation = Quaternion.RotateTowards(neckTarget.rotation, headRotation, headTurnSpeed * Time.deltaTime);
            neckTarget.localRotation = Quaternion.Euler(neckTarget.localEulerAngles.x, Mathf.Clamp(neckTarget.localEulerAngles.y, headTurnMin, headTurnMax), neckTarget.localEulerAngles.z);
            //Debug.Log("Neck: " + neckTarget.rotation.y);
        }
    }
    //void RotateHead(Vector3 objPos)
    //{
    //    if (selectedTarget != null)
    //    {
    //        headFixedDirection = new Vector3(objPos.x, neckTarget.transform.position.y, objPos.z);
    //        headRotation = Quaternion.LookRotation(headFixedDirection - neckTarget.position).normalized;
    //        neckTarget.rotation = headRotation;
    //    }
    //}
    void MoveRightArm(Vector3 objPos)
    {
        Vector3 armDirection = new Vector3(objPos.x, rightHandTarget.position.y, objPos.z);
        rightHandTarget.position = armDirection;
    }

    void RotateSpine(Vector3 objPos)
    {
        if (selectedTarget != null)
        {
            spineDirection = new Vector3(objPos.x, spineTarget.transform.position.y, objPos.z);
            spineRotation = Quaternion.LookRotation(spineDirection - spineTarget.position).normalized;
            spineTarget.rotation = Quaternion.RotateTowards(spineTarget.rotation, spineRotation, spineTurnSpeed * Time.deltaTime);
            spineTarget.localRotation = Quaternion.Euler(spineTarget.localEulerAngles.x, Mathf.Clamp(spineTarget.localEulerAngles.y, spineTurnMin, spineTurnMax), spineTarget.localEulerAngles.z);
        }
    }
    ///Poop Throwing///
    IEnumerator TargetCheckRoutine()
    {
        while (true)
        {
            selectedTarget = null;
            while (!isTargetable)
            {
                CheckTargetStatus();
                yield return new WaitForSeconds(.1f);
            }
            //Debug.Log("Target Found");
            //selectedTargetPos = selectedTarget.transform.position;
            anim.SetTrigger("RefillPoop");
            spineStartPos = spineTarget.forward * 8f;
            headStartPos = neckTarget.forward * 8f;
            //Debug.Log("Spine start pos: " + spineStartPos);
            reLoading = true;
            while (reLoading)
            {
                yield return null;
                while (neckRig.weight < 1f)
                {
                    neckRig.weight += .04f;
                    RotateHead(selectedTargetPos);
                    yield return null;
                }
                RotateHead(selectedTargetPos);
                yield return null;
            }
            isThrowing = true;
            while (isThrowing)
            {
                while (spineRig.weight < 1f)
                {
                    spineRig.weight += spineWeightMulti;
                    RotateSpine(selectedTargetPos);
                    //neckRig.weight += .04f;
                    //RotateHead(selectedTargetPos);
                    yield return null;
                }
                RotateSpine(selectedTargetPos);
                //RotateHead(selectedTargetPos);
                yield return null;
            }
            while(spineRig.weight > 0f)
            {
                spineRig.weight -= spineWeightMulti;
                RotateSpine(spineStartPos);
                neckRig.weight -= .04f;
                RotateHead(headStartPos);
                yield return null;
            }
            //while(spineTarget.rotation != spineStartRot)
            //{
            //    RotateSpine(spineStartPos);
            //    yield return null;
            //}
            neckTarget.localRotation = headStartRot;
            spineTarget.localRotation = spineStartRot;
            yield return new WaitForSeconds(cooldownTime);
            isTargetable = false;
        }
    }

    //IEnumerator TargetCheckRoutine()
    //{
    //    while (true)
    //    {
    //        selectedTarget = null;
    //        while (!isTargetable)
    //        {
    //            CheckTargetStatus();
    //            yield return new WaitForSeconds(.1f);
    //        }
    //        anim.SetTrigger("RefillPoop");
    //        reLoading = true;
    //        //neckRig.weight = 1f;
    //        while (reLoading)
    //        {
    //            while(neckRig.weight < 1f)
    //            {
    //                neckRig.weight += .04f;
    //                RotateHead(selectedTarget.transform.position);
    //                yield return null;
    //            }
    //            RotateHead(selectedTarget.transform.position);
    //            yield return null;
    //        }
    //        isThrowing = true;
    //        while(isThrowing)
    //        {
    //            while(rightArmRig.weight < .5f)
    //            {
    //                rightArmRig.weight += rArmRigMulti;
    //                MoveRightArm(selectedTarget.transform.position);
    //                RotateHead(selectedTarget.transform.position);
    //                yield return null;
    //            }
    //            MoveRightArm(selectedTarget.transform.position);
    //            yield return null;
    //        }
    //        while(rightArmRig.weight > 0f)
    //        {
    //            rightArmRig.weight -= rArmRigMulti;
    //            Debug.Log("R Weight: " + rightArmRig.weight);
    //        }
    //        //while (neckRig.weight > 0f)
    //        //{
    //        //    neckRig.weight -= .04f;
    //        //    //RotateHead(headStartPos);
    //        //    yield return null;
    //        //}
    //        rightArmRig.weight = 0f;
    //        //neckTarget.position = headStartPos;
    //        isTargetable = false;
            
    //    }
        
    //}

    void CheckTargetStatus()
    {
        if(targets.Length > 0)
        {
            foreach (GameObject t in targets)
            {
                var ge = t.GetComponent<GoochEnemy>();
                if (Vector3.Distance(t.transform.position, transform.position) < rangeMax &&
                Vector3.Distance(t.transform.position, transform.position) > rangeMin &&
                ge.isTargetable && ge.inZone)
                {
                    selectedTarget = t;
                    isTargetable = true;
                    selectedTargetPos = t.transform.position;
                    //Debug.Log("Target Found");
                    break;
                }
            }
        }
    }

    public void ResetISReloading()
    {
        reLoading = false;
    }

    public void ResetIsThrowing()
    {
        isThrowing = false;
    }

    void ThrowPoop()
    {
        pooler.PoolObjects(PooledObjectArrays.goochPoopArray, poopSpawn.position, Quaternion.identity, Vector3.zero);
        //Debug.Log("Throwimg");
    }

    void CancelThrow()
    {
        if(selectedTarget != null)
        {
            if (!selectedTarget.GetComponent<GoochEnemy>().inZone)
            {
                reLoading = false;
                isThrowing = false;
            }
        }
    }
}
