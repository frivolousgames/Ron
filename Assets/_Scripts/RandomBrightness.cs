using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrightness : MonoBehaviour
{
    Light light1;

    private void Awake()
    {
        light1 = GetComponent<Light>();
        StartCoroutine(LightRandom());
    }

    IEnumerator LightRandom()
    {
        while (true)
        {
            light1.intensity = Random.Range(24192f, 31011f);
            yield return new WaitForSeconds(.1f);
        }
    }
}
