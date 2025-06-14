using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentUIRotation : MonoBehaviour
{
    [SerializeField]
    GameObject image;
    [SerializeField]
    GameObject parentImage;

    RectTransform imgRectTransform;
    RectTransform parentRectTransform;
    private void Start()
    {
        imgRectTransform = image.GetComponent<RectTransform>();
        parentRectTransform = parentImage.GetComponent<RectTransform>();
    }
    private void Update()
    {
        imgRectTransform.rotation = parentRectTransform.rotation;       
    }
}
