using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyIdleOffset : MonoBehaviour
{
    Animator anim;
    public static bool isAlive;
    public bool _isAlive;

    public AudioSource slam;
    public AudioClip[] slamClips;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetFloat("idleOffset", Random.Range(0f, 1f));
    }

    private void Update()
    {
        _isAlive = isAlive;
        anim.SetBool("isAlive", _isAlive);
    }

    public void IsAlive()
    {
        isAlive = true;
    }

    public void SlamDoorSound()
    {
        slam.pitch = Random.Range(.85f, 1.15f);
        slam.PlayOneShot(slamClips[Random.Range(0, slamClips.Length)]);
    }
}
