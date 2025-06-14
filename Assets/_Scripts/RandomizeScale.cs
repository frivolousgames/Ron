using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RandomizeScale : MonoBehaviour
{
    [SerializeField]
    float min;
    [SerializeField]
    float max;
    float randF;

    private void OnEnable()
    {
        randF = Random.Range(min, max);
        transform.localScale = new Vector3(randF, randF, randF);
    }
}
