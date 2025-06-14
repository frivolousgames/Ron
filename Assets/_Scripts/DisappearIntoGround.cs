using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearIntoGround : MonoBehaviour
{
    float delay;
    float distance;
    //[SerializeField]
    float speed;

    [SerializeField]
    Collider[] cols;
    [SerializeField]
    Rigidbody[] rbs;

    private void Awake()
    {
        speed = 5f;
        delay = 4f;
    }
    private void OnEnable()
    {
        StartCoroutine(Disappear());
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down * 6, out hit, Mathf.Infinity, 6))
        {
            distance = Vector3.Distance(transform.position, hit.point);
        }
    }

    private void OnDisable()
    {
        foreach (Collider col in cols)
        {
            col.enabled = true;
        }
        foreach (Rigidbody r in rbs)
        {
            r.isKinematic = false;
        }
    }
    private void OnBecameInvisible()
    {
        StopCoroutine(Disappear());
        gameObject.SetActive(false);
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(delay);
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
        foreach (Rigidbody r in rbs)
        {
            r.isKinematic = true;
        }
        
        
        while(true)
        {
            transform.position += (Vector3.down * Time.deltaTime * (speed + distance));
            yield return null;
        }
    }
}
