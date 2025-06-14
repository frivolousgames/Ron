using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    bool isDissolving;
    bool dissolved;
    Vector3 dissolveDirection;
    float dissolveSpeed;
    [SerializeField]
    float speedMin;
    [SerializeField]
    float speedMax;
    [SerializeField]
    float movementMulti;

    Vector3 rotation;
    [SerializeField]
    float rotSpeed;

    private void Awake()
    {
        dissolveSpeed = Random.Range(speedMin, speedMax);
        dissolveDirection = new Vector3(Random.Range(-.005f, .005f), Random.Range(-.005f, .005f), dissolveSpeed);
        rotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
    }
    private void Update()
    {
        isDissolving = DissolvingBrickWallController.isDissolving;
        Dissolve();
    }

    void Dissolve()
    {
        if (isDissolving)
        {
            if (!dissolved)
            {
                dissolved = true;
                float i = 0f;
                StartCoroutine(ScaleDown(i));
            }

        }
    }

    IEnumerator ScaleDown(float i)
    {
        //while(transform.localScale.x > 0f)
        while(transform.localScale.x > 0f)
        {
            //dissolveDirection = new Vector3(Random.Range(-.005f, .005f), Random.Range(.005f, .010f), dissolveSpeed);
            transform.localPosition += (dissolveDirection * i * Time.deltaTime);
            i+= movementMulti;
            transform.Rotate(rotation * Time.deltaTime * rotSpeed);
            //i -= scaleMulti;
            transform.localScale -= new Vector3(.25f, .25f, .25f) * Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        yield break;
    }
}
