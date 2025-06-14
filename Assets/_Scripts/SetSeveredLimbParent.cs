using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSeveredLimbParent : MonoBehaviour
{
    [SerializeField]
    Renderer[] groundTrans;
    [SerializeField]
    Transform[] parentTrans;

    [SerializeField]
    Transform busTrans;

    float busFrontX;
    Transform newParent;

    Animator anim;
    bool isWiggling;

    Rigidbody rb;

    [SerializeField]
    float upSpeedMax;
    [SerializeField]
    float upSpeedMin;
    [SerializeField]
    float sideSpeedMin;
    [SerializeField]
    float sideSpeedMax;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        isWiggling = true;
    }

    private void OnEnable()
    {
        SetParent();
        rb.AddForce(new Vector3(0, Random.Range(upSpeedMin, upSpeedMax), Random.Range(sideSpeedMin, sideSpeedMax)), ForceMode.Impulse);
    }

    private void Update()
    {
        anim.SetBool("isWiggling", isWiggling);
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
                newParent = parentTrans[i];
            }
            transform.parent = newParent;
        }
    }
}
