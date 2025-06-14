using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectActive : MonoBehaviour
{
    [SerializeField]
    GameObject obj;

    public void SetActive()
    {
        if (obj != null)
        {
            obj.SetActive(true);
        }
    }
}
