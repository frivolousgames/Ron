using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineSceneActivatorAsync : MonoBehaviour
{
    public string sceneIn;

    private void OnEnable()
    {
        if (sceneIn != null)
        {
            {   
                SceneManager.LoadSceneAsync(sceneIn, LoadSceneMode.Additive);
            }
        }
    }
}
