using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletMover : MonoBehaviour
{
    Rigidbody rb;
    public float bulletSpeed;
    Vector3 startPos;

    public GameObject[] dust;
    Vector3 hitPoint;

    public GameObject[] bulletHoles;

    public GameObject[] bloodHits;
    Vector3 hitNormal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * bulletSpeed;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 2f))
        {
            //if(hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 6)
            if(hit.collider)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 6)
        {
            //offset = other.ClosestPointOnBounds(transform.position);
            PoolObjects(dust, hitPoint, transform.rotation);
            PoolObjects(bulletHoles, hitPoint + hitNormal * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal));
            gameObject.SetActive(false);
        }
        if(other.gameObject.layer == 12)
        {
            PoolObjects(bloodHits, hitPoint - other.gameObject.transform.position * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal));
            gameObject.SetActive(false);
        }

    }

    public void PoolObjects(GameObject[]objs, Vector3 pos, Quaternion rot)
    {
        foreach (GameObject o in objs)
        {
            if (!o.activeInHierarchy)
            {
                o.SetActive(true);
                o.transform.position = pos;
                o.transform.rotation = rot;
                break;
            }
        }
    }
}
