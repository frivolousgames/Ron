using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCollision : MonoBehaviour
{
    Collider hitCollider;

    GameObject[] bloodHits;
    public Transform bloodSpawn;

    GameObject[] chunkHits;

    public Vector3 spawnOffset;

    //[SerializeField]
    //int damageAmount;

    ObjectPooler pooler;

    private void Awake()
    {

        bloodHits = PooledObjectArrays.bloodHitsArray;
        chunkHits = PooledObjectArrays.chunkHitsArray;


        pooler = new ObjectPooler();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Chunk Hits: " + chunkHits[0]);
        //Debug.Log("BSPos: " + bloodSpawn.position);
        //Debug.Log("Other:  " + other.gameObject.name);
        if (other.gameObject.layer == 12)
        {
            //Vector3 spawnPos = other.ClosestPoint(bloodSpawn.position - new Vector3(other.gameObject.transform.position.x, 0f, other.gameObject.transform.position.z));\
            Vector3 spawnPos = other.ClosestPoint(bloodSpawn.position);
            pooler.PoolObjects(bloodHits, spawnPos, Quaternion.identity, spawnOffset);
        }
        if (other.gameObject.layer == 13)
        {

            pooler.PoolObjects(chunkHits, bloodSpawn.position, Quaternion.identity, spawnOffset);
        }
    }
}
