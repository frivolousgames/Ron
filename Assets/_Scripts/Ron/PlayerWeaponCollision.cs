using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCollision : MonoBehaviour
{
    BoxCollider hitCollider;
    BulletMover bm;

    public GameObject[] bloodHits;
    public Transform bloodSpawn;
    private void Awake()
    {
        bm = new BulletMover();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            bm.PoolObjects(bloodHits, bloodSpawn.position, Quaternion.identity);
        }
    }
}
