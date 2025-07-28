using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DemonChildWatcher : MonoBehaviour
{
    GameObject player;

    Quaternion startRot;

    Animator anim;

    [SerializeField]
    bool isTurning;

    [SerializeField]
    bool isIdle;

    Vector3 playerDistance;
    [SerializeField]
    float playerOffsetX;

    Quaternion turnRotation;

    [SerializeField]
    float turnSpeed;

    [SerializeField]
    float turnDistance;

    [SerializeField]
    float resetDistance;

    [SerializeField]
    Renderer[] groundTrans;
    [SerializeField]
    Transform[] parentTrans;

    [SerializeField]
    Transform busTrans;

    float busFrontX;
    Transform newParent;

    [SerializeField]
    float busOffsetX;

    [SerializeField]
    float resetDelay;

    [SerializeField]
    int raised;

    [SerializeField]
    float endPosX;

    Vector3 startPos;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        isTurning = false;
        startRot = transform.localRotation;
        startPos = transform.position;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        playerDistance = player.transform.position - transform.position;
        
        anim.SetBool("isIdle", isIdle);
        anim.SetBool("isTurning", isTurning);
        anim.SetInteger("raised", raised);

        StartTurn();
        ResetPosition();
        //Debug.Log(playerDistance.x + "--" + currentTurnDistance);
    }

    void StartTurn()
    {
        if (!isTurning)
        {
            if (playerDistance.x < turnDistance)
            {
                isTurning = true;
                //raised = Random.Range(0, 2);
                raised = 0;
                StartCoroutine(Turn());
            }
        }
    }

    IEnumerator Turn()
    {
        Debug.Log("Turning");
        float i = 0;
        while (i < 100 || isTurning)
        {
            Vector3 playerFixedPos = new Vector3(busTrans.position.x, transform.position.y, busTrans.position.z);
            turnRotation = Quaternion.LookRotation(playerFixedPos - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, i);
            i += turnSpeed;
            yield return null;
        }
        Debug.Log("Ended");
        isTurning = false;
        yield break;
    }

    void ResetPosition()
    {
        if (transform.position.x > endPosX)
        {
            isTurning = false;
            StopCoroutine(Turn());
            SetParent();
            transform.localRotation = startRot;
            ResetRagdollPosition();
        }
    }

    public void SetParent()
    {
        float distance = -1000;
        busFrontX = busTrans.position.x - 3f;
        int i = 0;
        foreach (var g in groundTrans)
        {
            if (busFrontX - g.bounds.min.x > distance)
            {
                distance = busFrontX - g.bounds.min.x;
                //Debug.Log(g.name + ": " + distance);
                newParent = parentTrans[i];
            }
            i++;
        }
        transform.parent = newParent;
        //Debug.Log("NewParent: " + newParent);
    }

    void ResetRagdollPosition()
    {
        transform.position += new Vector3(busOffsetX, 0f, 0f);
    }
}
