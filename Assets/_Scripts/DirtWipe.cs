using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtWipe : MonoBehaviour
{
    public GameObject dirtPic;
    Material dirtMat;
    public float speed;
    float dirtOffset;
    private void Awake()
    {
        dirtOffset = 0;
    }

    private void Start()
    {
        dirtMat = dirtPic.GetComponent<MeshRenderer>().material;
        dirtMat.SetFloat("_dirtOffset", .5f);
        dirtOffset = dirtMat.GetFloat("_dirtOffset");
    }

    public void Wipe()
    {
        StartCoroutine(WipeRoutine());
    }

   IEnumerator WipeRoutine()
    {
        //dirtOffset = dirtMat.GetFloat("_dirtOffset");
        while (dirtOffset > 0f)
        {
            float i = dirtMat.GetFloat("_dirtOffset");
            dirtMat.SetFloat("_dirtOffset", i - speed);
            dirtOffset = dirtMat.GetFloat("_dirtOffset");
            yield return new WaitForSeconds(.01f);
        }
        Debug.Log("BREAK");
        yield break;
    }
}
