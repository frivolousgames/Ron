using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloatAnim : MonoBehaviour
{
    Animator anim;

    [HideInInspector]
    public float offset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        offset = Random.Range(0f, 1f);
    }

    private void Update()
    {
        anim.SetFloat("offset", offset);
    }
}
