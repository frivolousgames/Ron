using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquirt : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    Rigidbody rb;
    [SerializeField]
    float bulletSpeed;
    Vector3 startPos;

    GameObject[] squirtHits;
    Vector3 hitPoint;

    GameObject[] squirtSpatters;
    Vector3 spatterSpawnOffset;

    GameObject[] bloodHits;
    Vector3 hitNormal;

    Vector3 spawnOffset;
    GameObject spawnedPool;

    ObjectPooler pooler;

    [SerializeField]
    Transform rayPos;


    private void Awake()
    {
        rb = parent.GetComponent<Rigidbody>();
        spawnOffset = new Vector3(0f, 0f, 0f);
        spatterSpawnOffset = new Vector3(0f, -.5f, 0f);

        squirtHits = PooledObjectArrays.squirtHitsArray;
        squirtSpatters = PooledObjectArrays.squirtSpattersArray;
        pooler = new ObjectPooler();
    }

    private void OnEnable()
    {
        rb.velocity = parent.transform.right * bulletSpeed;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.SphereCast(rayPos.position, .2f, transform.TransformDirection(Vector3.left), out hit, 1f))
        {
            if (hit.collider.gameObject.layer == 6 ||
                hit.collider.gameObject.layer == 11 ||
                hit.collider.gameObject.layer == 18)
            {
                //Debug.Log("Hit: " + hit.collider.name);
                hitPoint = hit.point;
                hitNormal = hit.normal;
            }
        }
        Debug.DrawRay(rayPos.position, transform.TransformDirection(Vector3.left) * 1f);
    }


    private void OnBecameInvisible()
    {
        //Debug.Log("Invisible");
        Destroy(parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger: " + other.gameObject.name);
        if (other.gameObject.layer == 6)
        {
            foreach (GameObject squirt in squirtHits)
            {
                if(squirt.activeInHierarchy && Vector3.Distance(squirt.transform.position, hitPoint) < .05f)
                {
                    squirt.SetActive(false);
                    //Debug.Log("Deactivated");
                }
            }
            pooler.PoolObjects(squirtHits, hitPoint + hitNormal * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), spawnOffset);
            //Debug.Log(hitPoint + hitNormal);
        }
        if (other.gameObject.layer == 11 ||
            other.gameObject.layer == 18)
        {
            if(hitPoint != Vector3.zero)
            {
                pooler.PoolObjects(squirtSpatters, hitPoint, transform.rotation, spatterSpawnOffset/*spawnOffset*/);
            }
            else
            {
                pooler.PoolObjects(squirtSpatters, other.ClosestPoint(rayPos.position), transform.rotation, spatterSpawnOffset/*spawnOffset*/);
            }
            Destroy(parent);
        }
    }
}
