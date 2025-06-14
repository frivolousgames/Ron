using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnVisibleEvent : MonoBehaviour
{
    //[SerializeField]
    //UnityEvent visible;

    //[SerializeField]
    //UnityEvent invisible;

    bool past;

    bool closed;

    private void OnBecameVisible()
    {
        if (!closed)
        {
            past = true;
            //visible.Invoke();
        }
        Debug.Log("Visible");
    }

    private void OnBecameInvisible()
    {
        if (!closed)
        {
            if (past)
            {
                past = false;
                //invisible.Invoke();
            }
        }
        Debug.Log("Invisible");
    }

    private void OnApplicationQuit()
    {
        closed = true;
    }

    private void Update()
    {
        
    }
}
