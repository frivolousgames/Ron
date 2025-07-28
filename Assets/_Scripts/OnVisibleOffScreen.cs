using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnVisibleOffScreen : MonoBehaviour
{
    public bool isVisible;

    private void OnEnable()
    {
        isVisible = false;
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    private void OnApplicationQuit()
    {
        isVisible = false;
    }
}
