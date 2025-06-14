using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PoopThrower : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    Transform parent;
    Transform player;
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

    [SerializeField]
    GameObject grabCol;
    Collider hitCol;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pooler = new ObjectPooler();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hitCol = GetComponent<Collider>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        ps.Play();
        StartCoroutine(ThrowWait());
        
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
        if (spattered)
        {
            rb.drag = 5f;
        }
    }

    IEnumerator ThrowWait()
    {
        yield return new WaitForSeconds(.02f);
        //Debug.Log("Position: " + transform.position);
        Vector3 throwAngle = (player.position - transform.position).normalized;
        rb.AddForce(new Vector3(throwAngle.x, throwHeight, throwAngle.z) * throwPower, ForceMode.Impulse);
        yield break;
    }
    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.drag = 0f;
        spattered = false;
        hitCol.enabled = true;
        grabCol.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 18)
        {
            ps.Stop();
            pooler.PoolObjects(PooledObjectArrays.poopHitArray, transform.position, Quaternion.identity, Vector3.zero);
            SetCols();
        }
        if(other.gameObject.layer == 11 || other.gameObject.layer == 6)
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
    }

    void SetCols()
    {
        hitCol.enabled = false;
        grabCol.SetActive(true);
    }
}
