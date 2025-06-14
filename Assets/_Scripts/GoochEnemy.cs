using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoochEnemy : MonoBehaviour
{
    public bool isTargetable;
    public bool inZone;
    [SerializeField]
    float resetTime;

    [SerializeField]
    UnityEvent goochPoopHit;

    [SerializeField]
    GameObject poopCovering;
    Animator poopCoverAnim;

    private void Start()
    {
        poopCoverAnim = poopCovering.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        isTargetable = true;
        inZone = false;
    }

    private void OnDisable()
    {
        StopCoroutine(TargetReset());
    }

    public void GoochPoopHit()
    {
        if (isTargetable)
        {
            isTargetable = false;
            poopCovering.SetActive(true);
            goochPoopHit.Invoke();
            StartCoroutine(TargetReset());
        }
    }

    IEnumerator TargetReset()
    {
        yield return new WaitForSeconds(resetTime);
        poopCoverAnim.SetTrigger("End");
        isTargetable = true;
        //Debug.Log("Reset Gooch Object");
        yield break;
    }   

    public void InTheZone()
    {
        inZone = true;
    }
    public void OutOfZone()
    {
        inZone = false;
    }
}
