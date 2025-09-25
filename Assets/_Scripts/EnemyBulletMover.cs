using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletMover : MonoBehaviour
{
    protected Rigidbody rb;
    [SerializeField]
    protected float bulletSpeed;
    protected Vector3 startPos;

    protected GameObject[] dust;
    protected Vector3 hitPoint;

    protected GameObject[] bulletHoles;

    protected GameObject[] bloodHits;
    protected GameObject[] chunkHits;
    protected Vector3 hitNormal;

    protected Vector3 spawnOffset;

    [SerializeField]
    protected int damageAmount;

    protected ObjectPooler pooler;

    protected Transform player;
    

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 6)
        {
            pooler.PoolObjects(dust, hitPoint, transform.rotation, spawnOffset);
            pooler.PoolObjects(bulletHoles, hitPoint + hitNormal * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), spawnOffset);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.layer == 18)
        {
            //pooler.PoolObjects(bloodHits, hitPoint - other.gameObject.transform.position * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), spawnOffset);
            gameObject.SetActive(false);
        }
    }
}
