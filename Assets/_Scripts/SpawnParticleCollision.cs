
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnParticleCollision : MonoBehaviour
{
    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;
    GameObject[] hits;
    GameObject[] acidHits;
    ObjectPooler pooler;
    private void Awake()
    {

        
        //bulletHoles = PooledObjectArrays.milkMarkArrays;
        //dust = PooledObjectArrays.milkSpatterArrays;
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        pooler = new ObjectPooler();
    }
    private void Start()
    {
        hits = PooledObjectArrays.gooSpatterArrays;
        acidHits = PooledObjectArrays.acidSmokeArrays;
        //pooler.PoolObjects(hits, transform.position, transform.rotation, Vector3.zero);

    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        Vector3 hitNormal = collisionEvents[0].normal;
        //for (int i = 0; i < numCollisionEvents; i++)
        //{
            Debug.Log("Hit");
            pooler.PoolObjects(hits, collisionEvents[0].intersection, Quaternion.LookRotation(hitNormal), Vector3.zero);
            pooler.PoolObjects(acidHits, collisionEvents[0].intersection + hitNormal * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), Vector3.zero);
        //}

        //pooler.PoolObjects(dust, transform.rotation, spawnOffset);
        //pooler.PoolObjects(bulletHoles, hitPoint + hitNormal * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), spawnOffset);
    }

}
