using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterRagdollParts : MonoBehaviour
{
    [SerializeField]
    float speed;
    Vector3 direction;
    Vector3 startPos;
    Vector3 startRot;
    Vector3 rotation;
    float rotSpeed;

    [SerializeField]
    MeshRenderer meshRenderer;

    List<Color> oMatColors;
    [SerializeField]
    float fadeSpeed;
    [SerializeField]
    float fadeWait;

    private void Awake()
    {
        oMatColors = new List<Color>();
        startPos = transform.localPosition;
        startRot = transform.localEulerAngles;
        foreach (Material mat in meshRenderer.materials)
        {
            oMatColors.Add(mat.color);
        }
    }

    private void OnEnable()
    {
        direction = new Vector3(0f, Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        rotSpeed = Random.Range(2f, 4f);
        for (int i = 0; i < oMatColors.Count; i++)
        {
            StartCoroutine(FadeOut(i));
        }
        
    }
    private void Update()
    {
        transform.localPosition += direction * speed;
        transform.Rotate(rotation, Time.deltaTime + rotSpeed);
    }

    private void OnDisable()
    {
        transform.localPosition = startPos;
        transform.localEulerAngles = startRot;
        for (int i = 0; i < oMatColors.Count; i++)
        {
            meshRenderer.materials[i].color = oMatColors[i];
        }
    }

    //IEnumerator FadeOut(int index)
    //{
    //    yield return new WaitForSeconds(fadeWait);
    //    Color col = oMatColors[index];
    //    while (col.a > .001f)
    //    {
    //        col = Color.Lerp(meshRenderer.sharedMaterials[index].color, Color.clear, fadeSpeed * Time.deltaTime);
    //        meshRenderer.sharedMaterials[index].color = col;
    //        Debug.Log("Col: " + meshRenderer.sharedMaterials[index].color);
    //        yield return null;
    //    }
    //}

    IEnumerator FadeOut(int index)
    {
        yield return new WaitForSeconds(fadeWait);
        float a = 1f;
        while (a > .001f)
        {
            a = Mathf.Lerp(a, 0, fadeSpeed * Time.deltaTime);
            meshRenderer.materials[index].color = new Color(meshRenderer.materials[index].color.r, meshRenderer.materials[index].color.g, meshRenderer.materials[index].color.b, a);
            Debug.Log("Col: " + a);
            yield return null;
        }
        meshRenderer.materials[index].color = new Color(meshRenderer.materials[index].color.r, meshRenderer.materials[index].color.g, meshRenderer.materials[index].color.b, 0f);
    }
}
