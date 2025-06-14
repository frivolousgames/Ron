using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeColorOverTime : MonoBehaviour
{
    [SerializeField]
    MeshRenderer mr;
    Material fadeMat;
    [SerializeField]
    float fadeWait;
    [SerializeField]
    float fadeTime;
    Color originalColor;

    private void Start()
    {
        fadeMat = mr.material;
        originalColor = fadeMat.color;
    }
    private void OnEnable()
    {
        StartCoroutine(FadeOut());
    }
    private void OnDisable()
    {
        StopCoroutine(FadeOut());
        fadeMat.color = originalColor;
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeWait);
        float i = 1;
        while(fadeMat.color.a > 0)
        {
            i -= fadeTime;
            fadeMat.color = new Color(fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, i);
            //Debug.Log("Color: " + fadeMat.color.a);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
