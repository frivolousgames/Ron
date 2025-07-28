using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class FollowParallax : MonoBehaviour
{
    Transform mainCam;

    float boundsX;
    float distance;

    private void Awake()
    {
        boundsX = GetComponent<Renderer>().bounds.size.x;
        //Debug.Log("BoundsX: " + boundsX);
    }

    private void Start()
    {
        mainCam = Camera.main.gameObject.transform;
    }
    private void Update()
    {
        Parallax();
        //Debug.Log("Distance: " + distance);
    }

    void Parallax()
    {
        distance = mainCam.position.x - transform.position.x;
        if(distance > boundsX)
        {
            transform.position += new Vector3(boundsX * 2, 0f, 0f);
        }
        if (distance < -boundsX)
        {
            transform.position -= new Vector3(boundsX * 2, 0f, 0f);
        }
    }
}
