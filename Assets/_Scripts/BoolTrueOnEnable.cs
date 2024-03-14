using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolTrueOnEnable : MonoBehaviour
{
    public static bool activator;

    private void OnEnable()
    {
        activator = true;
    }

    private void OnDisable()
    {
        activator = false;
    }
}
