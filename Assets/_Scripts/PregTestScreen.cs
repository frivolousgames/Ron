using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregTestScreen : MonoBehaviour
{
    public Material screenMaterial;

    private void OnEnable()
    {
        screenMaterial.SetFloat("_lineOpacity", 0);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float i = .1f;
        yield return new WaitForSeconds(.2f);
        while (i < .8f)
        {
            screenMaterial.SetFloat("_lineOpacity", i);
            i += .03f;
            yield return null;
        }
    }
}
