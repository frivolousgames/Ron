using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonChildTrailerController : MonoBehaviour
{
    Animator anim;
    public bool isAppearing;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isAppearing", isAppearing);
    }

    public void SetIsAppearing()
    {
        isAppearing = true;
    }
}
