using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class BodyColliderHit : MonoBehaviour
{
    [SerializeField]
    Collider[] bodyCols;

    [SerializeField]
    float resetWait;

    bool isHit;

    [SerializeField]
    UnityEvent hit;

    ///Gooch///
    [SerializeField]
    UnityEvent goochPoopHit;
    [SerializeField]
    UnityEvent inZone;
    [SerializeField]
    UnityEvent outZone;

    private void OnEnable()
    {
        isHit = false;
        foreach (Collider col in bodyCols)
        {
            col.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            IsHit();
        }
        if (other.gameObject.CompareTag("GoochPoop"))
        {
            goochPoopHit.Invoke();
            other.gameObject.SetActive(false);
            //Debug.Log("GoochPoopHit");
        }
        if (other.gameObject.CompareTag("GoochZone"))
        {
            inZone.Invoke();
            //Debug.Log("InZone");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GoochZone"))
        {
            outZone.Invoke();
            //Debug.Log("OutZone");
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void IsHit()
    {
        if (!isHit)
        {
            isHit = true;
            hit.Invoke();
            foreach (Collider col in bodyCols)
            {
                col.enabled = false;
                StartCoroutine(ResetCols());
            }
            //Debug.Log("Hit: " + gameObject.name);
        }
    }

    IEnumerator ResetCols()
    {
        yield return new WaitForSeconds(resetWait);
        foreach (Collider col in bodyCols)
        {
            col.enabled = true;
            yield return null;
        }
        isHit = false;
        yield break;
    }
}
