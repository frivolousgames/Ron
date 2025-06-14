using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneBulletMover : MonoBehaviour
{
    Rigidbody rb;
    public float bulletSpeed;
    Vector3 startPos;
    Vector3 hitPoint;
    Vector3 hitNormal;
    ObjectPooler pooler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        pooler = new ObjectPooler();
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.forward * bulletSpeed;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 2f, transform.TransformDirection(Vector3.forward), out hit, 15f))
        {
            //if(hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 6)
            if (hit.collider.gameObject.layer == 20)
            {
                hitPoint = hit.point;
                hitNormal = hit.normal;
                Debug.Log("Hit: " + hit.collider.gameObject.name);
            }
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            if (Vector3.Distance(hitPoint, other.ClosestPoint(transform.position)) > 5f)
            {
                other.ClosestPoint(transform.position);
                //Debug.Log("Hit Point Wrong: " + gameObject.name);
            }
            else
            {
                //Debug.Log("HitPointRight: " + gameObject.name);
            }
            pooler.PoolObjects(PooledObjectArrays.sparkHitsArray, other.ClosestPoint(hitPoint), Quaternion.identity, Vector3.zero);
            gameObject.SetActive(false);
        }
        if(other.gameObject.layer == 21)
        {
            other.gameObject.SetActive(false);
        }
    }
}
