using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameobjectsOnBool : MonoBehaviour
{
    bool activate;
    bool active;

    public GameObject[] gameObjects;

    private void OnEnable()
    {
        active = false;
        activate = BoolTrueOnEnable.activator;
    }

    private void Update()
    {
        activate = BoolTrueOnEnable.activator;

        if (!active)
        {
            if(activate)
            {
                
                foreach(GameObject go in gameObjects)
                {
                    if (!go.activeInHierarchy)
                    {
                        go.SetActive(true);
                    }
                }
                active = true;
            }
        }
    }
}
