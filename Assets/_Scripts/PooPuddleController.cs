using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooPuddleController : MonoBehaviour
{
    [SerializeField]
    float delayTime;

    Animator anim;
    [SerializeField]
    bool delayOver;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        StartCoroutine(StopDelay());
    }

    private void OnDisable()
    {
        delayOver = false;
    }

    IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(delayTime);
        delayOver = true;
    }

    private void Update()
    {
        anim.SetBool("delayOver", delayOver);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
