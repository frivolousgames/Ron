using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterRagdollMain : MonoBehaviour
{
    public void SetInactive()// Add to callback event of blood particle system
    {
        gameObject.SetActive(false);
    }
}
