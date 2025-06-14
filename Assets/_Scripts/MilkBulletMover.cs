using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBulletMover : EnemyBulletMover
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnOffset = new Vector3(0f, 0f, -.5f);

        bloodHits = PooledObjectArrays.milkSpatterArrays;
        bulletHoles = PooledObjectArrays.milkMarkArrays;
        dust = PooledObjectArrays.milkSpatterArrays;

        //bloodHits = PooledObjectArrays.bloodHitsArray;
        //bulletHoles = PooledObjectArrays.bulletHolesArray;
        //dust = PooledObjectArrays.bulletDustArray;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        pooler = new ObjectPooler();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * bulletSpeed;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f))
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
        Debug.Log("Invisible");
        gameObject.SetActive(false);
    }
}
