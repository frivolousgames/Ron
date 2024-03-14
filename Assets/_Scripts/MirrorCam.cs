using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCam : MonoBehaviour
{
    public GameObject mainCam;
    GameObject mirrorCam;
    public float offset;

    private void Awake()
    {
        mirrorCam = gameObject;
    }
    private void Update()
    {
        mirrorCam.transform.position = new Vector3(mirrorCam.transform.position.x, mirrorCam.transform.position.y, (-mainCam.transform.position.x + offset) / 4);
    }
}
