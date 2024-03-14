using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineSceneActivator : MonoBehaviour
{
    public string sceneIn;

    private void OnEnable()
    {        
        if(sceneIn != null)
        {
            {
                SceneManager.LoadScene(sceneIn, LoadSceneMode.Additive);
                //AsyncOperation op = SceneManager.LoadSceneAsync(sceneIn, LoadSceneMode.Additive);
                //op.allowSceneActivation = false;
                //StartCoroutine(ActivateScene(op));
            }
        }
    }

    //IEnumerator ActivateScene(AsyncOperation op)
    //{
    //    yield return new WaitForSeconds(.5f);
    //    op.allowSceneActivation = true;
    //    yield break;
    //}
}
