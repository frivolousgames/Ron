using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyIdleOffset : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetFloat("idleOffset", Random.Range(0f, 1f));
    }
}
