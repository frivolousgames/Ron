using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class CloudLightning : MonoBehaviour
{
    //[SerializeField]
    //GameObject lightningObject;
    //VisualEffect lightningFX;
    [SerializeField]
    float lifetime;

    bool litUp;
    [SerializeField]
    Material glowMat;
    [SerializeField]
    float endMetallic;
    [SerializeField]
    float startMetallic;
    [SerializeField]
    float glowSpeed;

    private void Start()
    {
        //lightningFX = lightningObject.GetComponent<VisualEffect>();
        //StartCoroutine(LightningStrike());
        StartCoroutine(CloudLightUp());
    }

    //IEnumerator LightningStrike()
    //{
    //    yield return new WaitForSeconds(Random.Range(1f, 5f));
    //    while (true)
    //    {
    //        float i = Random.Range(1f, 5f);
    //        lightningFX.Play();
    //        yield return new WaitForSeconds(lifetime);
    //        lightningFX.Stop();
    //        yield return new WaitForSeconds(i);
    //    }
    //}

    //IEnumerator CloudLightUp()
    //{
    //    while (true)
    //    {
    //        int k = Random.Range(1, 5);
    //        for (int j = 0; j < k; j++)
    //        {
    //            float a = Random.Range(glowMat.color.a, endColor.a);
    //            Color endRandom = new Color(endColor.r, endColor.g, endColor.b, a);
    //            //Debug.Log("ColorRandom: " + r + g + b + a);
    //            while (glowMat.color != endRandom)
    //            {
    //                glowMat.color = Color.Lerp(glowMat.color, endRandom , glowSpeed * Time.deltaTime);
    //                yield return null;
    //            }
    //            while (glowMat.color != startColor)
    //            {
    //                glowMat.color = Color.Lerp(glowMat.color, startColor, glowSpeed * Time.deltaTime);
    //                yield return null;
    //            }
    //            yield return null;
    //        }
    //        yield return new WaitForSeconds(Random.Range(.5f, 1.2f));
    //    }   
    //}
    IEnumerator CloudLightUp()
    {
        while (true)
        {
            int k = Random.Range(1, 5);
            for (int j = 0; j < k; j++)
            {
                float a = Random.Range(glowMat.GetFloat("_Metallic"), endMetallic);
                //Debug.Log("Start");
                while (glowMat.GetFloat("_Metallic") != a)
                {
                    glowMat.SetFloat("_Metallic", Mathf.Lerp(glowMat.GetFloat("_Metallic"), a, glowSpeed * Time.deltaTime));
                    yield return null;
                }
                while (glowMat.GetFloat("_Metallic") != startMetallic)
                {
                    glowMat.SetFloat("_Metallic", Mathf.Lerp(glowMat.GetFloat("_Metallic"), startMetallic, glowSpeed * Time.deltaTime));
                    yield return null;
                }
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(.5f, 1.2f));
        }
    }
}
