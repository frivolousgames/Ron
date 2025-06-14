using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeByTime : MonoBehaviour
{
    public float fadeWait;
    public float fadeOutTime;

    public Color fadeColor;

    Material meshMaterial;
    Color startColor;
    Color newColor;

    private void Start()
    {
        meshMaterial = GetComponentInChildren<MeshRenderer>().material;
        startColor = meshMaterial.color;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeWait());
    }

    private void OnDisable()
    {
        meshMaterial.color = startColor;
    }

    IEnumerator FadeWait()
    {
        yield return new WaitForSeconds(fadeWait);
        StartCoroutine(FadeColor());
        yield break;
    }

    IEnumerator FadeColor()
    {
        newColor = startColor;
        float timeElapsed = 0;
        while(timeElapsed < fadeOutTime)
        {
            newColor.a = Mathf.Lerp(startColor.a, fadeColor.a, timeElapsed / fadeOutTime);
            timeElapsed += Time.deltaTime;
            meshMaterial.color = newColor;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
