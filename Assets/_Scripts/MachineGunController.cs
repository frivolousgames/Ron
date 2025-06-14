using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : MonoBehaviour, IInteractable
{
    Animator anim;

    public bool isGunning;

    public bool IsInteractable => !isInUse;

    bool isInUse;
    bool waiting;

    public Transform playerInteractTransform;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isGunning", isGunning);
    }

    public void Interact()
    {
        if(!isInUse)
        {
            isInUse = true;
            waiting = true;
            StartCoroutine(AnimWait());
        }
    }

    IEnumerator AnimWait()
    {
        while(waiting)
        {
            yield return null;
        }
        isGunning = true;
        while (isGunning)
        {
            yield return null;
        }
        yield return null;
    }

}
