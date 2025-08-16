using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterBulletMover : PlayerBulletMover
{
    GameObject[] bloodStreaks;
    //int bulletDirection;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnOffset = new Vector3(0f, 0f, -.5f);

        bloodHits = PooledObjectArrays.bloodHitsArray;
        chunkHits = PooledObjectArrays.chunkHitsArray;
        bulletHoles = PooledObjectArrays.bulletHolesArray;
        dust = PooledObjectArrays.bulletDustArray;
        bloodStreaks = PooledObjectArrays.fractureHitsArray;

        pooler = new ObjectPooler();
    }

    private void Update()
    {
        //if(transform.localEulerAngles.y == 0f)
        //{
        //    bulletDirection = 1;
        //}
        //else
        //{
        //    bulletDirection = -1;
        //}
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * bulletSpeed;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 2f))
        {
            //if(hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 6)
            if (hit.collider)
            {
                hitPoint = hit.point;
                hitNormal = hit.normal;
            }
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            //offset = other.ClosestPointOnBounds(transform.position);
            pooler.PoolObjects(dust, hitPoint, transform.rotation, spawnOffset);
            gameObject.SetActive(false);
        }
        if (other.gameObject.layer == 6)
        {
            //offset = other.ClosestPointOnBounds(transform.position);
            pooler.PoolObjects(dust, hitPoint, transform.rotation, spawnOffset);
            pooler.PoolObjects(bulletHoles, hitPoint + hitNormal * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), spawnOffset);
            gameObject.SetActive(false);
        }
        if (other.gameObject.layer == 12)
        {
            //Vector3 offset = 
            pooler.PoolObjects(bloodHits, other.ClosestPoint(transform.position), Quaternion.identity, spawnOffset);
            pooler.PoolObjects(bloodStreaks, other.ClosestPoint(transform.position), transform.rotation, spawnOffset);
            //gameObject.SetActive(false);
        }

        if (other.gameObject.layer == 13)
        {
            pooler.PoolObjects(chunkHits, hitPoint - other.gameObject.transform.position * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), spawnOffset);
            gameObject.SetActive(false);
        }
    }
}
