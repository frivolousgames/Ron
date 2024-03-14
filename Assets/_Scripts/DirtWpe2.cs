using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtWpe2 : MonoBehaviour
{

    public GameObject dirtPic;
    Material dirtMat;
    float dirtOffset;
    private void Awake()
    {
        dirtOffset = 0f;
        transform.localPosition = new Vector3(.5f, 0f, 0f);
    }

    private void Start()
    {
        dirtMat = dirtPic.GetComponent<MeshRenderer>().material;
        dirtMat.SetFloat("_dirtOffset", .5f);
        
    }

    private void Update()
    {
        dirtMat.SetFloat("_dirtOffset", transform.localPosition.x);
    }
}
