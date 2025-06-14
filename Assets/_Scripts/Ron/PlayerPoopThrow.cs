using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoopThrow : MonoBehaviour
{
    Transform player;
    Rigidbody rb;
    [SerializeField]
    float throwPower;
    [SerializeField]
    float throwHeight;
    [SerializeField]
    ParticleSystem ps;
    ObjectPooler pooler;

    Vector3 hitPoint;
    Vector3 hitNormal;

    bool spattered;

    Collider hitCol;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        pooler = new ObjectPooler();
        hitCol = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        ps.Play();
        Throw();

        //rb.AddForce(new Vector3(parent.forward.x, 0f, parent.forward.z).normalized * throwPower, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 2f) ||
            Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.right), out hit, 2f) ||
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 2f) ||
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2f))
        {
            if (hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 6)
            {
                hitPoint = hit.point;
                hitNormal = hit.normal;
            }
        }
    }

    void Throw()
    {
        rb.AddForce(new Vector3(player.right.x,throwHeight, player.right.z) * throwPower, ForceMode.Impulse);
    }
    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        spattered = false;
        hitCol.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HandPoop")
        {
            ps.Stop();
            pooler.PoolObjects(PooledObjectArrays.poopHitArray, transform.position, Quaternion.identity, Vector3.zero);
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        if (other.gameObject.layer == 11 || other.gameObject.layer == 6)
        {
            if (!spattered)
            {
                ps.Stop();
                pooler.PoolObjects(PooledObjectArrays.poopHitArray, transform.position, Quaternion.identity, Vector3.zero);
                pooler.PoolObjects(PooledObjectArrays.poopSpatterHitArray, hitPoint + hitNormal * Random.Range(.0001f, .0009f), Quaternion.LookRotation(hitNormal), Vector3.zero);
                spattered = true;
            }
            SetCols();
        }
        if(other.gameObject.layer == 13)
        {
            pooler.PoolObjects(PooledObjectArrays.poopHitArray, transform.position, Quaternion.identity, Vector3.zero);
            gameObject.SetActive(false);
        }
    }

    void SetCols()
    {
        hitCol.enabled = false;
    }
}
