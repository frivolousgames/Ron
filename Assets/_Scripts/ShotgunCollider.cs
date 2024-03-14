using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunCollider : MonoBehaviour
{
    BoxCollider col;
    BulletMover bm;

    public GameObject[] bloodSpurts;

    private void Awake()
    {
        bm = new BulletMover();
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            bm.PoolObjects(bloodSpurts, other.ClosestPoint(other.gameObject.transform.position), Quaternion.identity);
        }
    }
}
