using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;

public class DemonChildExploderController : MonoBehaviour
{
    [SerializeField]
    Animator anim;

    bool isHit;

    [SerializeField]
    GameObject ragdoll;

    Vector3 startPos;
    Vector3 ragdollNewPos;

    [SerializeField]
    Transform busTrans;

    [SerializeField]
    float busOffsetX;

    [SerializeField]
    GameObject demonChild;

    [SerializeField]
    GameObject[] bones;

    Vector3 boneStartScale;

    [SerializeField]
    GameObject[] intestines;

    [SerializeField]
    GameObject[] livers;

    Vector3 partsStartPos;

    [SerializeField]
    Rigidbody[] boneRBs;

    List<Vector3> bonePos;
    List<Quaternion> boneRot;

    [SerializeField]
    float resetWait;

    int partCount;

    [SerializeField]
    Renderer[] groundTrans;

    float busFrontX;
    Transform newParent;


    /////BLOOD/////
    [SerializeField]
    GameObject[] bloodHits;
    [SerializeField]
    float hitOffsetY;

    [SerializeField]
    GameObject[] bloodPlumes;
    [SerializeField]
    float tireHitOffsetX;

    bool explodes;
    [SerializeField]
    GameObject[] heads;
    [SerializeField]
    GameObject[] armsL;
    [SerializeField]
    GameObject[] armsR;
    [SerializeField]
    GameObject[] legsL;
    [SerializeField]
    GameObject[] legsR;

    List<GameObject[]> bodyPartArrays;

    ObjectPooler pooler;
    private void Awake()
    {
        bonePos = new List<Vector3>();
        boneRot = new List<Quaternion>();

        startPos = transform.position;

        pooler = new ObjectPooler();
        bodyPartArrays = new List<GameObject[]>();
        anim.SetInteger("Delay", 0);
        StartCoroutine(YellDelay());
    }

    private void Start()
    {
        //partCount = guts.Length;
        int i = 0;
        foreach (var b in boneRBs)
        {
            bonePos.Add(boneRBs[i].transform.position);
            boneRot.Add(boneRBs[i].transform.rotation);
            i++;
        }
        bodyPartArrays.Add(heads);
        bodyPartArrays.Add(armsL);
        bodyPartArrays.Add(armsR);
        bodyPartArrays.Add(legsL);
        bodyPartArrays.Add(legsR);
    }

    private void Update()
    {
        anim.SetBool("isHit", isHit);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            if (!isHit)
            {
                isHit = true;
                anim.enabled = false;
                SetBoneRBs();
                pooler.PoolObjects(bloodHits, new Vector3(transform.position.x, transform.position.y + hitOffsetY, transform.position.z), Quaternion.identity, Vector3.zero);
                SetExplodeBool();
                Explode();
                SpawnGuts();
                StartCoroutine(ResetRagdollWait());
                //anim.SetFloat("Delay", 0f);
            }
        }
        if(other.gameObject.layer == 16)
        {
            pooler.PoolObjects(bloodPlumes, new Vector3(transform.position.x + tireHitOffsetX, transform.position.y, transform.position.z), Quaternion.identity, Vector3.zero);
        }
    }

    IEnumerator YellDelay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        anim.SetInteger("Delay", 1);
        yield break;
    }

    IEnumerator ResetRagdollWait()
    {
        yield return new WaitForSeconds(resetWait);
        isHit = false;
        ResetBoneRBs();
        ResetRagdollPosition();
        anim.enabled = true;
        ResetBoneTransforms();
        SetParent();
        ResetBoneScales();
        //StartCoroutine(YellDelay());
        yield break;
    }

    void Explode()
    {
        if (explodes)
        {
            int j = 0;
            foreach (var b in bones)
            {
                int i = Random.Range(0, 2);
                if(i == 0)
                {
                    b.transform.localScale = Vector3.zero;
                    pooler.PoolObjects(bodyPartArrays[j], transform.position, Quaternion.identity, Vector3.zero);
                    
                }
                j++;
            }
        }

    }

    void SpawnGuts()
    {
        int i = Random.Range(0, 3);
        if(i == 0)
        {
            pooler.PoolObjects(intestines, transform.position, Quaternion.identity, Vector3.zero);
        }
        else if(i == 1)
        {
            pooler.PoolObjects(livers, transform.position, Quaternion.identity, Vector3.zero);
        }
    }

    void SetBoneRBs()
    {
        foreach(var rb in boneRBs)
        {
            rb.isKinematic = false;
        }
    }

    void ResetBoneRBs()
    {
        foreach (var rb in boneRBs)
        {
            rb.isKinematic = true;
        }
    }
    
    void ResetRagdollPosition()
    {
        transform.position = new Vector3(busTrans.position.x + busOffsetX, startPos.y, startPos.z);
    }

    void ResetBoneTransforms()
    {
        int i = 0;
        foreach (var b in boneRBs)
        {
            boneRBs[i].transform.position = bonePos[i];
            boneRBs[i].transform.rotation = boneRot[i];
            i++;
        }
    }

    void ResetBoneScales()
    {
        foreach(var b in bones)
        {
            b.transform.localScale = Vector3.one;
        }
    }

    public void SetParent()
    {
        float distance = -1000;
        busFrontX = busTrans.position.x - 3f;
        foreach (var g in groundTrans)
        {
            if(busFrontX - g.bounds.min.x > distance)
            {
                distance = busFrontX - g.bounds.min.x;
                newParent = g.gameObject.transform;
            }
            transform.parent = newParent;
        }
    }

    void SetExplodeBool()
    {
        int i = Random.Range(0, 100);

        if(i < 70)
        {
            explodes = false;
        }
        else
        {
            explodes = true;
        }
    }
}
