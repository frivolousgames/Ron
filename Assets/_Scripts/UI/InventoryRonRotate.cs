using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryRonRotate : MonoBehaviour
{
    [SerializeField]
    Transform ronTrans;
    float startRotY;
    float mouseStartPosX;
    float clickStartY;
    bool isClicked;
    [SerializeField]
    GameObject resetText;
    bool isDragging;
    bool isEntered;

    [SerializeField]
    GameObject arrows;

    private void Awake()
    {
        startRotY = ronTrans.eulerAngles.y;
    }
    public void OnPointerDown()
    {
        mouseStartPosX = Input.mousePosition.x;
        clickStartY = ronTrans.eulerAngles.y;
        arrows.SetActive(true);
    }
    public void OnPointerUp()
    {
        isDragging = false;
        if(isEntered && ronTrans.eulerAngles.y != startRotY)
        {
            resetText.SetActive(true);
        }
        arrows.SetActive(false);
    }

    public void OnPointerDrag()
    {
        isDragging = true;
        float yRot = (Input.mousePosition.x - mouseStartPosX) / 10f;
        ronTrans.rotation = Quaternion.Euler(ronTrans.eulerAngles.x, clickStartY - yRot, ronTrans.eulerAngles.z);
    }

    public void OnPointerClick()
    {
        if(!isClicked)
        {
            isClicked = true;
            StartCoroutine(DoubleClickTimer());
        }
        else
        {
            ronTrans.rotation = Quaternion.Euler(ronTrans.eulerAngles.x, startRotY, ronTrans.eulerAngles.z);
            resetText.SetActive(false);
        }
    }
    IEnumerator DoubleClickTimer()
    {
        yield return new WaitForSeconds(.3f);
        isClicked = false;
        yield break;
    }

    public void OnPointerEnter()
    {
        isEntered = true;
        if(!isDragging)
        {
            if (ronTrans.eulerAngles.y != startRotY)
            {
                resetText.SetActive(true);
            }
            else
            {
                resetText.SetActive(false);
            }
        }
    }

    public void OnPointerExit()
    {
        isEntered = false;
        if(resetText.activeInHierarchy)
        {
            resetText.SetActive(false);
        }
    }
}
