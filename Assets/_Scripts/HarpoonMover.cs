using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonMover : EnemyBulletMover
{
    Animator anim;
    float stopSpeed;
    float moveSpeed;
    Transform oParent;
    Collider col;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        oParent = transform.parent;
        col = GetComponent<Collider>();
        spawnOffset = Vector3.zero;
        dust = PooledObjectArrays.bulletDustArray;
    }
    private void OnEnable()
    {
        moveSpeed = bulletSpeed;
        col.enabled = true;
        rb.isKinematic = false;
        if (transform.parent != oParent)
        {
            transform.parent = oParent;
        }
        hitPoint = Vector3.zero;
        //hitNormal = Vector3.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * moveSpeed;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 3f))
        {
            //if(hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 6)
            if (hit.collider)
            {
                hitPoint = hit.point;
                //hitNormal = hit.normal;
            }
        }
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 6)
        {
            if(hitPoint == Vector3.zero)
            {
                hitPoint = other.ClosestPoint(col.bounds.center);
            }
            pooler.PoolObjects(dust, hitPoint, transform.rotation, spawnOffset);
            moveSpeed = 0f;
            rb.isKinematic = true;
            anim.SetTrigger("Wiggle");
            col.enabled = false;
        }
        else if (other.gameObject.layer == 18)
        {
            //pooler.PoolObjects(bloodHits, hitPoint - other.gameObject.transform.position * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), spawnOffset);
            moveSpeed = 0f;
            rb.isKinematic = true;
            transform.parent = other.gameObject.transform;
            anim.SetTrigger("Wiggle");
            col.enabled = false;
            Debug.Log("Hit");
        }
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
