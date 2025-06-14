using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunCollider : MonoBehaviour
{
    BoxCollider col;

    [SerializeField]
    int damageAmount;


    GameObject[] bloodSpurts;
    GameObject[] chunkHits;

    ObjectPooler pooler;

    [SerializeField]
    Vector3 spawnOffset;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        pooler = new ObjectPooler();
    }

    private void Start()
    {
        bloodSpurts = PooledObjectArrays.bloodHitsArray;
        chunkHits = PooledObjectArrays.chunkHitsArray;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            pooler.PoolObjects(bloodSpurts, other.ClosestPoint(other.gameObject.transform.position), Quaternion.LookRotation(-transform.parent.forward, transform.parent.up), spawnOffset);
        }
        if (other.gameObject.layer == 13)
        {
            pooler.PoolObjects(chunkHits, other.ClosestPoint(other.gameObject.transform.position), Quaternion.LookRotation(-transform.parent.forward, transform.parent.up), spawnOffset);
        }
    }
}
