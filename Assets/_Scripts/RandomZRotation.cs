using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomZRotation : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, Random.Range(0f, 360f));
    }
}
