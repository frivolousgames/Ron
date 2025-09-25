using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeColorOverTimeSM : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer mr;
    Material fadeMat;
    [SerializeField]
    float fadeWait;
    [SerializeField]
    float fadeTime;
    Color originalColor;
    Coroutine fadeOut;
    [SerializeField]
    UnityEvent faded;

    private void Start()
    {
        fadeMat = mr.material;
        originalColor = fadeMat.color;
    }
    private void OnEnable()
    {
        fadeOut = StartCoroutine(FadeOut());
    }
    private void OnDisable()
    {
        StopCoroutine(fadeOut);
        fadeMat.color = originalColor;
    }
    IEnumerator FadeOut() // Set material to transparent
    {
        yield return new WaitForSeconds(fadeWait);
        float i = 1;
        while (fadeMat.color.a > 0)
        {
            i -= fadeTime;
            fadeMat.color = new Color(fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, i);
            //Debug.Log("Color: " + fadeMat.color.a);
            yield return null;
        }
        faded.Invoke();
        //gameObject.SetActive(false);
    }
}
