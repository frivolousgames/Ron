using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ButlerController : NpcController
{
    int coweringHitNumber = 0;
    float coweringTime = 0;
    bool coweringReset;

    public bool isGrounded;

    Vector3 springHitOffset;
    public float springForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        hitCol = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isCowering", isCowering);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetInteger("coweringHitNumber", coweringHitNumber);
        //Debug.Log("CoweringTime: " + coweringTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerWeapon")
        {
            if (!isHit)
            {
                anim.SetTrigger("Hit");
                isHit = true;
                
                if (isCowering)
                {
                    SetCoweringHitNumber();
                    coweringTime = 0;
                    
                }
            }
        }
        if (other.gameObject.tag == "SpringWeapon")
        {
            if (isCowering)
            {
                isCowering = false;
            }
            coweringTime = 0f;
            anim.SetTrigger("knockedBack");
            springHitOffset = (PunisherController.hitPoint - transform.position).normalized;
            rb.AddForce(springHitOffset * springForce, ForceMode.Impulse);
            Debug.Log("Offset: " +  springHitOffset);
        }
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    void SetCoweringHitNumber()
    {
        coweringHitNumber = Random.Range(0, 2);
    }

    IEnumerator CoweringReset()
    {
        while (coweringReset)
        {
            if (coweringTime > 10)
            {
                isCowering = false;
                coweringReset = false;
                yield break;
            }
            coweringTime++;
            yield return new WaitForSeconds(.5f);
        }
        yield break;
    }

    public void ResetIsCowering()
    {
        if(isCowering)
        {
            isCowering = false;
            coweringReset = false;
        }
        else
        {
            isCowering = true;
            if (!coweringReset)
            {
                coweringReset = true;
                coweringTime = 0;
                StartCoroutine(CoweringReset());
            }
        }
    }

    public void ResetIsHit()
    {
        isHit = false;
    }

    public override void WalkRoutine()
    {
        while (isWalking == true)
        {
            
        }
    }
}