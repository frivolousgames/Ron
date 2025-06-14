using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetUIParticleInactive : MonoBehaviour
{
    [SerializeField]
    RectTransform boundaryRect;
    float boundaryY;
    Vector3 boundaryPos;
    void Update()
    {
        boundaryY = boundaryRect.rect.yMax;
        boundaryPos = boundaryRect.TransformPoint(new Vector3(0f, boundaryY, 0f));
        if(transform.position.y > boundaryPos.y)
        {
            gameObject.SetActive(false);
            //Debug.Log("Pos: " + transform.position.y);
            //Debug.Log("BPos: " +  boundaryPos.y);
        }
    }
}
