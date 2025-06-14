using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    float offset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        offset = Random.Range(0f, 1f);
        anim.SetFloat("offset", offset);
    }

    public void SetOffset()
    {
        offset = Random.Range(0f, 1f);
        anim.SetFloat("offset", offset);
    }
}
