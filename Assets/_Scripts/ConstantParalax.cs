using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantParalax : MonoBehaviour
{
    Vector3 resetPos;
    Renderer r;
    float boundsX;
    float endX;
    float resetX;
    [SerializeField]
    float speed;
    float transX;

    [SerializeField]
    float offset;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        r = GetComponent<Renderer>();
        boundsX = r.bounds.max.x - r.bounds.min.x;
        endX =  boundsX * 2;
        resetX = -boundsX * 3;
        resetPos = new Vector3(resetX + offset, 0, 0);
        //Debug.Log(boundsX);
    }
    private void Update()
    {
        //RBParallax();
        if (transform.localPosition.x > endX)
        {
            transform.localPosition += resetPos;
        }
        transX = transform.localPosition.x;
        transX -= speed * Time.deltaTime;
        transform.localPosition = new Vector3(transX, transform.localPosition.y, transform.localPosition.z);

    }

    //void RBParallax()
    //{
    //    rb.velocity = Vector3.left * speed * Time.deltaTime;
    //    if (transform.localPosition.x > endX)
    //    {
    //        transform.localPosition += resetPos;
    //    }
    //}
}
