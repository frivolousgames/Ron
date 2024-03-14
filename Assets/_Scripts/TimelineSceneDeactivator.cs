using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineSceneDeactivator : MonoBehaviour
{
    public string sceneOut;

    private void OnDisable()
    {
        if (sceneOut != null)
        {
            {
                SceneManager.UnloadSceneAsync(sceneOut);
            }
        }
    }
}
