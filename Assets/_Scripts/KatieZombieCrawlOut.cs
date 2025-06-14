using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatieZombieCrawlOut : MonoBehaviour
{
    [SerializeField]
    GameObject crawlingZombie;
    [SerializeField]
    GameObject katieZombie;
    [SerializeField]
    ParticleSystem ps;
    
    [SerializeField]
    float distanceThreshold;
    [SerializeField]
    [HideInInspector]
    bool isUnfolded;
    
    Transform playerTrans;
    Animator anim;
    float playerDistance;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        anim.SetBool("isUnfolded", isUnfolded);
        playerDistance = Vector3.Distance(transform.position, playerTrans.position);
        if(playerDistance < distanceThreshold )
        {
            isUnfolded = true;
        }
    }
    private void OnDisable()
    {
        isUnfolded=false;
        crawlingZombie.SetActive(false);
        katieZombie.SetActive(false);
    }

    public void SetInactive()
    {
        crawlingZombie.SetActive(false);
        katieZombie.SetActive(true);
    }

    public void DirtPlume()
    {
        ps.Play();
    }
}
