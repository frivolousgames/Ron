using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeByTime : MonoBehaviour
{
    public float fadeWait;
    public float fadeOutTime;

    public Color fadeColor;
    Color startColor;
    Color newColor;

    private void Start()
    {
        startColor = GetComponentInChildren<MeshRenderer>().material.color;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeWait());
    }

    private void OnDisable()
    {
        GetComponentInChildren<MeshRenderer>().material.color = startColor;
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
            newColor.a = Mathf.Lerp(startColor.a, fadeColor.a, timeElapsed);
            timeElapsed += Time.deltaTime;
            GetComponentInChildren<MeshRenderer>().material.color = newColor;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
