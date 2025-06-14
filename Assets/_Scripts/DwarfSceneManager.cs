using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DwarfSceneManager : MonoBehaviour
{
    DwarfSceneManager manager;

    ///Timer
    [SerializeField]
    TMP_Text timerText;
    float maxTime;

    //Total Distance

    [SerializeField]
    TMP_Text sumText;
    [SerializeField]
    TMP_Text distanceText;


    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void EndRound()
    {

    }
}
