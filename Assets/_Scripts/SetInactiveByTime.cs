using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactiveByTime : MonoBehaviour
{
    public float delayTime;

    private void OnEnable()
    {
        StartCoroutine(SetInactive());
    }

    IEnumerator SetInactive()
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
