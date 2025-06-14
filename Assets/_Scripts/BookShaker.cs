using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class BookShaker : MonoBehaviour
{
    bool isShaking;

    Quaternion randomRot;

    [SerializeField]
    float rotSpeed;

    [SerializeField]
    float shakeWait;

    [SerializeField]
    float shakeResetWait;

    private void OnEnable()
    {
        StartCoroutine(RotateRandom());
    }
    void Shake()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, randomRot, Time.deltaTime * rotSpeed);
    }

    public void IsShaking()
    {
        if(!isShaking)
        {
            isShaking = true;
            StartCoroutine("ShakeReset");
        }  
    }
    IEnumerator ShakeReset()
    {
        yield return new WaitForSeconds(shakeResetWait);
    }
    IEnumerator RotateRandom()
    {
        while (true)
        {
            while (isShaking)
            {
                randomRot = Quaternion.Euler(transform.rotation.x, Random.Range(transform.rotation.y - 4f, transform.rotation.y + 4f), transform.rotation.z);
                Shake();
                yield return new WaitForSeconds(shakeWait);
            }
            yield return null;
        }
    }
}
